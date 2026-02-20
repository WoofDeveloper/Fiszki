using Microsoft.Data.Sqlite;
using Fiszki.Models;

namespace Fiszki.Data;

public class FlashcardRepository
{
    private readonly string _dbPath;

    public FlashcardRepository()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, "flashcards.db");
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Flashcards (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EnglishWord TEXT NOT NULL,
                PolishTranslation TEXT NOT NULL,
                Example TEXT,
                CategoryId INTEGER,
                CreatedDate TEXT NOT NULL,
                LastReviewed TEXT NOT NULL,
                NextReview TEXT NOT NULL,
                TimesReviewed INTEGER NOT NULL DEFAULT 0,
                CorrectAnswers INTEGER NOT NULL DEFAULT 0,
                IncorrectAnswers INTEGER NOT NULL DEFAULT 0,
                RepetitionLevel INTEGER NOT NULL DEFAULT 0
            )
        ";
        command.ExecuteNonQuery();

        MigrateDatabase(connection);
    }

    private void MigrateDatabase(SqliteConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandText = "PRAGMA table_info(Flashcards)";

        using var reader = command.ExecuteReader();
        var columns = new List<string>();

        while (reader.Read())
        {
            columns.Add(reader.GetString(1));
        }

        reader.Close();

        if (!columns.Contains("CategoryId"))
        {
            var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE Flashcards ADD COLUMN CategoryId INTEGER";
            alterCommand.ExecuteNonQuery();
        }

        if (!columns.Contains("NextReview"))
        {
            var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE Flashcards ADD COLUMN NextReview TEXT NOT NULL DEFAULT '" + DateTime.Now.ToString("O") + "'";
            alterCommand.ExecuteNonQuery();
        }

        if (!columns.Contains("RepetitionLevel"))
        {
            var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE Flashcards ADD COLUMN RepetitionLevel INTEGER NOT NULL DEFAULT 0";
            alterCommand.ExecuteNonQuery();
        }
    }

    public async Task<List<Flashcard>> GetAllFlashcardsAsync()
    {
        var flashcards = new List<Flashcard>();

        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Flashcards ORDER BY CreatedDate DESC";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            flashcards.Add(ReadFlashcard(reader));
        }

        return flashcards;
    }

    private Flashcard ReadFlashcard(System.Data.Common.DbDataReader reader)
    {
        var flashcard = new Flashcard
        {
            Id = reader.GetInt32(0),
            EnglishWord = reader.GetString(1),
            PolishTranslation = reader.GetString(2),
            Example = reader.IsDBNull(3) ? null : reader.GetString(3),
            CategoryId = reader.IsDBNull(4) ? null : reader.GetInt32(4),
            CreatedDate = DateTime.Parse(reader.GetString(5)),
            LastReviewed = DateTime.Parse(reader.GetString(6)),
            TimesReviewed = reader.GetInt32(8),
            CorrectAnswers = reader.GetInt32(9),
            IncorrectAnswers = reader.GetInt32(10)
        };

        flashcard.NextReview = reader.FieldCount > 7 && !reader.IsDBNull(7) 
            ? DateTime.Parse(reader.GetString(7)) 
            : DateTime.Now;

        flashcard.RepetitionLevel = reader.FieldCount > 11 && !reader.IsDBNull(11) 
            ? reader.GetInt32(11) 
            : 0;

        return flashcard;
    }

    public async Task<Flashcard?> GetFlashcardByIdAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Flashcards WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return ReadFlashcard(reader);
        }

        return null;
    }

    public async Task<int> AddFlashcardAsync(Flashcard flashcard)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Flashcards (EnglishWord, PolishTranslation, Example, CategoryId, CreatedDate, LastReviewed, NextReview, TimesReviewed, CorrectAnswers, IncorrectAnswers, RepetitionLevel)
            VALUES (@englishWord, @polishTranslation, @example, @categoryId, @createdDate, @lastReviewed, @nextReview, @timesReviewed, @correctAnswers, @incorrectAnswers, @repetitionLevel);
            SELECT last_insert_rowid();
        ";

        command.Parameters.AddWithValue("@englishWord", flashcard.EnglishWord);
        command.Parameters.AddWithValue("@polishTranslation", flashcard.PolishTranslation);
        command.Parameters.AddWithValue("@example", flashcard.Example ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@categoryId", flashcard.CategoryId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@createdDate", flashcard.CreatedDate.ToString("O"));
        command.Parameters.AddWithValue("@lastReviewed", flashcard.LastReviewed.ToString("O"));
        command.Parameters.AddWithValue("@nextReview", flashcard.NextReview.ToString("O"));
        command.Parameters.AddWithValue("@timesReviewed", flashcard.TimesReviewed);
        command.Parameters.AddWithValue("@correctAnswers", flashcard.CorrectAnswers);
        command.Parameters.AddWithValue("@incorrectAnswers", flashcard.IncorrectAnswers);
        command.Parameters.AddWithValue("@repetitionLevel", flashcard.RepetitionLevel);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task UpdateFlashcardAsync(Flashcard flashcard)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Flashcards 
            SET EnglishWord = @englishWord, 
                PolishTranslation = @polishTranslation, 
                Example = @example,
                CategoryId = @categoryId,
                LastReviewed = @lastReviewed,
                NextReview = @nextReview,
                TimesReviewed = @timesReviewed, 
                CorrectAnswers = @correctAnswers, 
                IncorrectAnswers = @incorrectAnswers,
                RepetitionLevel = @repetitionLevel
            WHERE Id = @id
        ";

        command.Parameters.AddWithValue("@id", flashcard.Id);
        command.Parameters.AddWithValue("@englishWord", flashcard.EnglishWord);
        command.Parameters.AddWithValue("@polishTranslation", flashcard.PolishTranslation);
        command.Parameters.AddWithValue("@example", flashcard.Example ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@categoryId", flashcard.CategoryId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@lastReviewed", flashcard.LastReviewed.ToString("O"));
        command.Parameters.AddWithValue("@nextReview", flashcard.NextReview.ToString("O"));
        command.Parameters.AddWithValue("@timesReviewed", flashcard.TimesReviewed);
        command.Parameters.AddWithValue("@correctAnswers", flashcard.CorrectAnswers);
        command.Parameters.AddWithValue("@incorrectAnswers", flashcard.IncorrectAnswers);
        command.Parameters.AddWithValue("@repetitionLevel", flashcard.RepetitionLevel);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteFlashcardAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Flashcards WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Flashcard>> GetRandomFlashcardsAsync(int count)
    {
        var flashcards = new List<Flashcard>();

        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM Flashcards ORDER BY RANDOM() LIMIT {count}";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            flashcards.Add(ReadFlashcard(reader));
        }

        return flashcards;
    }

    public async Task<List<Flashcard>> GetFlashcardsByCategoryAsync(int categoryId)
    {
        var flashcards = new List<Flashcard>();

        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Flashcards WHERE CategoryId = @categoryId ORDER BY CreatedDate DESC";
        command.Parameters.AddWithValue("@categoryId", categoryId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            flashcards.Add(ReadFlashcard(reader));
        }

        return flashcards;
    }

    public async Task<int> GetTotalCountAsync()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Flashcards";

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<List<Flashcard>> GetFlashcardsForLearningAsync(LearningSessionConfig config)
    {
        var flashcards = new List<Flashcard>();

        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();

        if (config.SelectedFlashcardIds != null && config.SelectedFlashcardIds.Any())
        {
            var ids = string.Join(",", config.SelectedFlashcardIds);
            command.CommandText = $"SELECT * FROM Flashcards WHERE Id IN ({ids})";
        }
        else
        {
            var whereClause = new List<string>();

            if (config.CategoryId.HasValue)
            {
                whereClause.Add("CategoryId = @categoryId");
                command.Parameters.AddWithValue("@categoryId", config.CategoryId.Value);
            }

            if (config.OnlyDueForReview)
            {
                whereClause.Add("NextReview <= @now");
                command.Parameters.AddWithValue("@now", DateTime.Now.ToString("O"));
            }

            var where = whereClause.Any() ? "WHERE " + string.Join(" AND ", whereClause) : "";

            if (config.PrioritizeWrongAnswers)
            {
                command.CommandText = $@"
                    SELECT * FROM Flashcards 
                    {where}
                    ORDER BY 
                        CASE WHEN CorrectAnswers = 0 THEN 0
                             ELSE CAST(IncorrectAnswers AS REAL) / (CorrectAnswers + IncorrectAnswers) 
                        END DESC,
                        NextReview ASC,
                        RANDOM()
                    LIMIT @count";
            }
            else
            {
                command.CommandText = $@"
                    SELECT * FROM Flashcards 
                    {where}
                    ORDER BY NextReview ASC, RANDOM()
                    LIMIT @count";
            }

            command.Parameters.AddWithValue("@count", config.Count);
        }

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            flashcards.Add(ReadFlashcard(reader));
        }

        return flashcards;
    }

    public DateTime CalculateNextReview(bool wasCorrect, int currentLevel)
    {
        if (wasCorrect)
        {
            var intervals = new[] { 1, 3, 7, 14, 30, 60, 120 };
            var intervalIndex = Math.Min(currentLevel, intervals.Length - 1);
            return DateTime.Now.AddDays(intervals[intervalIndex]);
        }
        else
        {
            return DateTime.Now.AddMinutes(10);
        }
    }

    public async Task<LearningStatistics> GetStatisticsAsync()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var stats = new LearningStatistics();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Flashcards";
        stats.TotalFlashcards = Convert.ToInt32(await command.ExecuteScalarAsync());

        command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Flashcards WHERE CorrectAnswers > 0 AND CAST(CorrectAnswers AS REAL) / (CorrectAnswers + IncorrectAnswers) >= 0.8";
        stats.MasteredFlashcards = Convert.ToInt32(await command.ExecuteScalarAsync());

        command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Flashcards WHERE NextReview <= @now";
        command.Parameters.AddWithValue("@now", DateTime.Now.ToString("O"));
        stats.DueForReview = Convert.ToInt32(await command.ExecuteScalarAsync());

        command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Flashcards WHERE date(LastReviewed) = date('now')";
        stats.StudiedToday = Convert.ToInt32(await command.ExecuteScalarAsync());

        command = connection.CreateCommand();
        command.CommandText = "SELECT SUM(TimesReviewed) FROM Flashcards";
        var totalReviews = await command.ExecuteScalarAsync();
        stats.TotalReviews = totalReviews != DBNull.Value ? Convert.ToInt32(totalReviews) : 0;

        command = connection.CreateCommand();
        command.CommandText = "SELECT AVG(CASE WHEN TimesReviewed > 0 THEN CAST(CorrectAnswers AS REAL) / TimesReviewed * 100 ELSE 0 END) FROM Flashcards";
        var avgSuccess = await command.ExecuteScalarAsync();
        stats.AverageSuccessRate = avgSuccess != DBNull.Value ? Convert.ToDouble(avgSuccess) : 0;

        return stats;
    }
}
