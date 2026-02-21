namespace Fiszki.Models;

/// <summary>
/// Model reprezentujący pojedynczą fiszkę (flashcard) w aplikacji.
/// Przechowuje słówko angielskie, polskie tłumaczenie oraz dane o postępach w nauce.
/// </summary>
public class Flashcard
{
    /// <summary>Unikalny identyfikator fiszki w bazie danych</summary>
    public int Id { get; set; }

    /// <summary>Słówko po angielsku (np. "cat")</summary>
    public string EnglishWord { get; set; } = string.Empty;

    /// <summary>Tłumaczenie na polski (np. "kot")</summary>
    public string PolishTranslation { get; set; } = string.Empty;

    /// <summary>Przykładowe zdanie wykorzystujące dane słówko (opcjonalne)</summary>
    public string? Example { get; set; }

    /// <summary>ID kategorii do której należy fiszka (opcjonalne)</summary>
    public int? CategoryId { get; set; }

    /// <summary>Data utworzenia fiszki</summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>Data ostatniego przeglądu/powtórki fiszki</summary>
    public DateTime LastReviewed { get; set; }

    /// <summary>Data następnej planowanej powtórki (używane w algorytmie rozłożonych powtórzeń)</summary>
    public DateTime NextReview { get; set; }

    /// <summary>Ile razy fiszka była powtarzana</summary>
    public int TimesReviewed { get; set; }

    /// <summary>Liczba poprawnych odpowiedzi</summary>
    public int CorrectAnswers { get; set; }

    /// <summary>Liczba niepoprawnych odpowiedzi</summary>
    public int IncorrectAnswers { get; set; }

    /// <summary>Poziom w algorytmie rozłożonych powtórzeń (0-6, wyższy = dłuższe przerwy między powtórkami)</summary>
    public int RepetitionLevel { get; set; }

    /// <summary>
    /// Procent poprawnych odpowiedzi (0-100%).
    /// Obliczany jako: (poprawne / wszystkie) * 100
    /// </summary>
    public double SuccessRate => TimesReviewed > 0 ? (double)CorrectAnswers / TimesReviewed * 100 : 0;

    /// <summary>
    /// Czy fiszka wymaga powtórki (czy nadszedł już czas NextReview).
    /// True jeśli NextReview <= DateTime.Now
    /// </summary>
    public bool NeedsReview => DateTime.Now >= NextReview;

    /// <summary>
    /// Ile dni pozostało do następnej powtórki.
    /// Wartość ujemna oznacza że termin już minął.
    /// </summary>
    public int DaysUntilReview => (NextReview - DateTime.Now).Days;
}

/// <summary>
/// Model wykorzystywany podczas importu fiszek z pliku JSON.
/// Zawiera tylko podstawowe pola bez danych o postępach.
/// </summary>
public class FlashcardImport
{
    /// <summary>Słówko po angielsku</summary>
    public string EnglishWord { get; set; } = string.Empty;

    /// <summary>Tłumaczenie na polski</summary>
    public string PolishTranslation { get; set; } = string.Empty;

    /// <summary>Przykładowe zdanie (opcjonalne)</summary>
    public string? Example { get; set; }

    /// <summary>Nazwa kategorii (opcjonalna)</summary>
    public string? Category { get; set; }
}

/// <summary>
/// Kontener dla listy importowanych fiszek.
/// Używany podczas deserializacji pliku JSON.
/// </summary>
public class FlashcardImportData
{
    /// <summary>Lista fiszek do zaimportowania</summary>
    public List<FlashcardImport> Flashcards { get; set; } = new();
}

/// <summary>
/// Konfiguracja sesji nauki - określa jakie fiszki mają być wylosowane do nauki.
/// </summary>
public class LearningSessionConfig
{
    /// <summary>ID kategorii (null = wszystkie kategorie)</summary>
    public int? CategoryId { get; set; }

    /// <summary>Ile fiszek ma być wylosowanych do sesji (domyślnie 10)</summary>
    public int Count { get; set; } = 10;

    /// <summary>Czy wybrać tylko fiszki wymagające powtórki (NeedsReview == true)</summary>
    public bool OnlyDueForReview { get; set; }

    /// <summary>Czy priorytetyzować fiszki z większą liczbą błędów</summary>
    public bool PrioritizeWrongAnswers { get; set; }

    /// <summary>Lista konkretnych ID fiszek do nauki (jeśli podana, ignoruje inne filtry)</summary>
    public List<int>? SelectedFlashcardIds { get; set; }
}

/// <summary>
/// Statystyki nauki użytkownika.
/// Zawiera różne metryki dotyczące postępów w nauce fiszek.
/// </summary>
public class LearningStatistics
{
    /// <summary>Całkowita liczba fiszek w bazie</summary>
    public int TotalFlashcards { get; set; }

    /// <summary>Liczba opanowanych fiszek (success rate >= 80%)</summary>
    public int MasteredFlashcards { get; set; }

    /// <summary>Liczba fiszek wymagających powtórki (NeedsReview == true)</summary>
    public int DueForReview { get; set; }

    /// <summary>Liczba fiszek przejrzanych dzisiaj</summary>
    public int StudiedToday { get; set; }

    /// <summary>Całkowita liczba wszystkich powtórek (suma TimesReviewed)</summary>
    public int TotalReviews { get; set; }

    /// <summary>Średni procent poprawnych odpowiedzi wszystkich fiszek</summary>
    public double AverageSuccessRate { get; set; }

    /// <summary>Słownik: nazwa kategorii -> liczba fiszek w tej kategorii</summary>
    public Dictionary<string, int> CategoryCounts { get; set; } = new();
}
