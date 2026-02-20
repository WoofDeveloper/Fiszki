namespace Fiszki.Models;

public class Flashcard
{
    public int Id { get; set; }
    public string EnglishWord { get; set; } = string.Empty;
    public string PolishTranslation { get; set; } = string.Empty;
    public string? Example { get; set; }
    public int? CategoryId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastReviewed { get; set; }
    public DateTime NextReview { get; set; }
    public int TimesReviewed { get; set; }
    public int CorrectAnswers { get; set; }
    public int IncorrectAnswers { get; set; }
    public int RepetitionLevel { get; set; }

    public double SuccessRate => TimesReviewed > 0 ? (double)CorrectAnswers / TimesReviewed * 100 : 0;

    public bool NeedsReview => DateTime.Now >= NextReview;

    public int DaysUntilReview => (NextReview - DateTime.Now).Days;
}

public class FlashcardImport
{
    public string EnglishWord { get; set; } = string.Empty;
    public string PolishTranslation { get; set; } = string.Empty;
    public string? Example { get; set; }
    public string? Category { get; set; }
}

public class FlashcardImportData
{
    public List<FlashcardImport> Flashcards { get; set; } = new();
}

public class LearningSessionConfig
{
    public int? CategoryId { get; set; }
    public int Count { get; set; } = 10;
    public bool OnlyDueForReview { get; set; }
    public bool PrioritizeWrongAnswers { get; set; }
    public List<int>? SelectedFlashcardIds { get; set; }
}

public class LearningStatistics
{
    public int TotalFlashcards { get; set; }
    public int MasteredFlashcards { get; set; }
    public int DueForReview { get; set; }
    public int StudiedToday { get; set; }
    public int TotalReviews { get; set; }
    public double AverageSuccessRate { get; set; }
    public Dictionary<string, int> CategoryCounts { get; set; } = new();
}
