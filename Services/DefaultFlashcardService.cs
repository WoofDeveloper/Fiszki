using System.Text.Json;
using Fiszki.Data;
using Fiszki.Models;

namespace Fiszki.Services;

public class DefaultFlashcardService
{
    private readonly FlashcardRepository _flashcardRepository;
    private readonly CategoryRepository _categoryRepository;
    private const string DefaultFlashcardsFileName = "default_flashcards.json";
    private const string VersionKey = "DefaultFlashcardsVersion";
    private const int CurrentVersion = 1; // Zwiększ tę wartość gdy dodasz nowe domyślne fiszki

    public DefaultFlashcardService(FlashcardRepository flashcardRepository, CategoryRepository categoryRepository)
    {
        _flashcardRepository = flashcardRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task LoadDefaultFlashcardsIfNeededAsync()
    {
        try
        {
            var installedVersion = Preferences.Get(VersionKey, 0);

            if (installedVersion >= CurrentVersion)
            {
                System.Diagnostics.Debug.WriteLine($"Default flashcards already loaded (version {installedVersion})");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Loading default flashcards (current version: {installedVersion}, new version: {CurrentVersion})");

            await LoadDefaultFlashcardsAsync();

            Preferences.Set(VersionKey, CurrentVersion);
            System.Diagnostics.Debug.WriteLine("Default flashcards loaded successfully");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading default flashcards: {ex.Message}");
        }
    }

    private async Task LoadDefaultFlashcardsAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(DefaultFlashcardsFileName);
        if (stream == null)
        {
            System.Diagnostics.Debug.WriteLine("Default flashcards file not found");
            return;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = await JsonSerializer.DeserializeAsync<FlashcardImportData>(stream, options);
        if (data?.Flashcards == null || !data.Flashcards.Any())
        {
            System.Diagnostics.Debug.WriteLine("No flashcards found in default file");
            return;
        }

        // Pobierz wszystkie istniejące fiszki
        var existingFlashcards = await _flashcardRepository.GetAllFlashcardsAsync();
        var existingSet = new HashSet<string>(
            existingFlashcards.Select(f => $"{f.EnglishWord.ToLower()}|{f.PolishTranslation.ToLower()}")
        );

        var categories = await _categoryRepository.ListAsync();
        var categoryDict = categories.ToDictionary(c => c.Title, c => c.ID);

        int addedCount = 0;
        int skippedCount = 0;

        foreach (var flashcardImport in data.Flashcards)
        {
            // Sprawdź czy fiszka już istnieje (po EnglishWord + PolishTranslation)
            var key = $"{flashcardImport.EnglishWord.ToLower()}|{flashcardImport.PolishTranslation.ToLower()}";
            if (existingSet.Contains(key))
            {
                skippedCount++;
                continue;
            }

            int? categoryId = null;
            if (!string.IsNullOrEmpty(flashcardImport.Category))
            {
                if (!categoryDict.TryGetValue(flashcardImport.Category, out var existingCategoryId))
                {
                    var newCategory = new Category
                    {
                        Title = flashcardImport.Category,
                        Color = "#4CAF50" // Domyślny zielony kolor dla kategorii
                    };
                    existingCategoryId = await _categoryRepository.SaveItemAsync(newCategory);
                    categoryDict[flashcardImport.Category] = existingCategoryId;
                }
                categoryId = existingCategoryId;
            }

            var flashcard = new Flashcard
            {
                EnglishWord = flashcardImport.EnglishWord,
                PolishTranslation = flashcardImport.PolishTranslation,
                Example = flashcardImport.Example,
                CategoryId = categoryId,
                CreatedDate = DateTime.Now,
                LastReviewed = DateTime.Now,
                NextReview = DateTime.Now,
                TimesReviewed = 0,
                CorrectAnswers = 0,
                IncorrectAnswers = 0,
                RepetitionLevel = 0
            };

            await _flashcardRepository.AddFlashcardAsync(flashcard);
            addedCount++;
        }

        System.Diagnostics.Debug.WriteLine($"Default flashcards import complete: {addedCount} added, {skippedCount} skipped (already exist)");
    }
}
