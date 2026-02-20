using System.Text.Json;
using Fiszki.Models;

namespace Fiszki.Services;

public class FlashcardImportService
{
    private readonly FlashcardRepository _flashcardRepository;
    private readonly FlashcardCategoryRepository _categoryRepository;

    public FlashcardImportService(FlashcardRepository flashcardRepository, FlashcardCategoryRepository categoryRepository)
    {
        _flashcardRepository = flashcardRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<(int imported, int failed, string message)> ImportFromJsonAsync(string jsonContent)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var importData = JsonSerializer.Deserialize<FlashcardImportData>(jsonContent, options);

            if (importData == null || importData.Flashcards == null || importData.Flashcards.Count == 0)
            {
                return (0, 0, "Brak danych do importu lub nieprawidlowy format JSON");
            }

            int imported = 0;
            int failed = 0;
            var errors = new List<string>();

            foreach (var flashcardImport in importData.Flashcards)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(flashcardImport.EnglishWord) || 
                        string.IsNullOrWhiteSpace(flashcardImport.PolishTranslation))
                    {
                        failed++;
                        errors.Add($"Puste slowo: {flashcardImport.EnglishWord ?? "brak"}");
                        continue;
                    }

                    int? categoryId = null;

                    if (!string.IsNullOrWhiteSpace(flashcardImport.Category))
                    {
                        var category = await _categoryRepository.GetCategoryByNameAsync(flashcardImport.Category);

                        if (category == null)
                        {
                            category = new Category { Title = flashcardImport.Category, Color = "#2196F3" };
                            categoryId = await _categoryRepository.AddCategoryAsync(category);
                        }
                        else
                        {
                            categoryId = category.ID;
                        }
                    }

                    var flashcard = new Flashcard
                    {
                        EnglishWord = flashcardImport.EnglishWord,
                        PolishTranslation = flashcardImport.PolishTranslation,
                        Example = flashcardImport.Example,
                        CategoryId = categoryId,
                        CreatedDate = DateTime.Now,
                        LastReviewed = DateTime.Now,
                        NextReview = DateTime.Now
                    };

                    await _flashcardRepository.AddFlashcardAsync(flashcard);
                    imported++;
                }
                catch (Exception ex)
                {
                    failed++;
                    errors.Add($"Blad: {ex.Message}");
                }
            }

            var errorMessage = errors.Any() ? $"\nBledy: {string.Join(", ", errors.Take(3))}" : "";
            return (imported, failed, $"Zaimportowano: {imported}, Bledy: {failed}{errorMessage}");
        }
        catch (Exception ex)
        {
            return (0, 0, $"Blad JSON: {ex.Message}");
        }
    }

    public async Task<string> ExportToJsonAsync()
    {
        var flashcards = await _flashcardRepository.GetAllFlashcardsAsync();
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        
        var exportData = new List<FlashcardImport>();
        
        foreach (var flashcard in flashcards)
        {
            var category = flashcard.CategoryId.HasValue 
                ? categories.FirstOrDefault(c => c.ID == flashcard.CategoryId.Value) 
                : null;

            exportData.Add(new FlashcardImport
            {
                EnglishWord = flashcard.EnglishWord,
                PolishTranslation = flashcard.PolishTranslation,
                Example = flashcard.Example,
                Category = category?.Title
            });
        }

        var data = new FlashcardImportData { Flashcards = exportData };
        
        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        return JsonSerializer.Serialize(data, options);
    }

    public string GetSampleJson()
    {
        var sample = new FlashcardImportData
        {
            Flashcards = new List<FlashcardImport>
            {
                new FlashcardImport 
                { 
                    EnglishWord = "hello", 
                    PolishTranslation = "cześć", 
                    Example = "Hello, how are you?",
                    Category = "Ogólne"
                },
                new FlashcardImport 
                { 
                    EnglishWord = "run", 
                    PolishTranslation = "biegać", 
                    Example = "I run every morning.",
                    Category = "Czasowniki"
                },
                new FlashcardImport 
                { 
                    EnglishWord = "book", 
                    PolishTranslation = "książka",
                    Category = "Rzeczowniki"
                }
            }
        };

        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        return JsonSerializer.Serialize(sample, options);
    }
}
