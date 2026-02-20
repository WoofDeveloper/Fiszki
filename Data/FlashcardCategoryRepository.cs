using Microsoft.Data.Sqlite;
using Fiszki.Models;

namespace Fiszki.Data;

public class FlashcardCategoryRepository
{
    private readonly string _dbPath;

    public FlashcardCategoryRepository()
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
            CREATE TABLE IF NOT EXISTS FlashcardCategories (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Color TEXT NOT NULL DEFAULT '#2196F3'
            )
        ";
        command.ExecuteNonQuery();
        
        SeedDefaultCategories();
    }

    private void SeedDefaultCategories()
    {
        var existingCount = GetAllCategoriesAsync().Result.Count;
        if (existingCount > 0) return;

        var defaultCategories = new List<Category>
        {
            new Category { Title = "Ogólne", Color = "#2196F3" },
            new Category { Title = "Czasowniki", Color = "#4CAF50" },
            new Category { Title = "Rzeczowniki", Color = "#FF9800" },
            new Category { Title = "Przymiotniki", Color = "#9C27B0" },
            new Category { Title = "Zwroty", Color = "#F44336" }
        };

        foreach (var category in defaultCategories)
        {
            AddCategoryAsync(category).Wait();
        }
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var categories = new List<Category>();
        
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM FlashcardCategories ORDER BY Title";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            categories.Add(new Category
            {
                ID = reader.GetInt32(0),
                Title = reader.GetString(1),
                Color = reader.GetString(2)
            });
        }

        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM FlashcardCategories WHERE ID = @id";
        command.Parameters.AddWithValue("@id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Category
            {
                ID = reader.GetInt32(0),
                Title = reader.GetString(1),
                Color = reader.GetString(2)
            };
        }

        return null;
    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM FlashcardCategories WHERE Title = @name COLLATE NOCASE";
        command.Parameters.AddWithValue("@name", name);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Category
            {
                ID = reader.GetInt32(0),
                Title = reader.GetString(1),
                Color = reader.GetString(2)
            };
        }

        return null;
    }

    public async Task<int> AddCategoryAsync(Category category)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO FlashcardCategories (Title, Color)
            VALUES (@title, @color);
            SELECT last_insert_rowid();
        ";
        
        command.Parameters.AddWithValue("@title", category.Title);
        command.Parameters.AddWithValue("@color", category.Color);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM FlashcardCategories WHERE ID = @id";
        command.Parameters.AddWithValue("@id", id);

        await command.ExecuteNonQueryAsync();
    }
}
