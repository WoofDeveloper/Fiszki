using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fiszki.Data;
using Fiszki.Models;
using Fiszki.Services;
using System.Collections.ObjectModel;

namespace Fiszki.PageModels;

public partial class FlashcardListPageModel : ObservableObject
{
    private readonly FlashcardRepository _repository;
    private readonly FlashcardCategoryRepository _categoryRepository;
    private readonly UpdateService _updateService;

    [ObservableProperty]
    private ObservableCollection<Flashcard> flashcards = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private int totalCount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Flashcards))]
    private string searchText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    [ObservableProperty]
    private Category? selectedCategory;

    private List<Flashcard> _allFlashcards = new();

    public FlashcardListPageModel(FlashcardRepository repository, FlashcardCategoryRepository categoryRepository, UpdateService updateService)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _updateService = updateService;
    }

    public async Task InitializeAsync()
    {
        System.Diagnostics.Debug.WriteLine("🔧 FlashcardListPageModel.InitializeAsync() START");
        await LoadCategoriesAsync();
        System.Diagnostics.Debug.WriteLine("📂 Kategorie załadowane");
        await LoadFlashcardsAsync();
        System.Diagnostics.Debug.WriteLine("📚 Fiszki załadowane");
        await CheckForUpdatesAsync();
        System.Diagnostics.Debug.WriteLine("🔧 FlashcardListPageModel.InitializeAsync() KONIEC");
    }

    private async Task CheckForUpdatesAsync()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("🚀 FlashcardListPageModel: Rozpoczynam sprawdzanie aktualizacji...");

            var update = await _updateService.CheckForUpdatesAsync();

            System.Diagnostics.Debug.WriteLine($"📦 Wynik sprawdzenia: {(update != null ? "JEST AKTUALIZACJA" : "Brak aktualizacji")}");

            if (update != null)
            {
                System.Diagnostics.Debug.WriteLine($"💬 Pokazuję dialog dla wersji: {update.Version}");

                bool answer = await Shell.Current.DisplayAlertAsync(
                    "Dostępna aktualizacja!",
                    $"Wersja {update.Version} jest dostępna!\n\n{update.ReleaseNotes}\n\nCzy chcesz pobrać aktualizację?",
                    "Tak",
                    "Później");

                System.Diagnostics.Debug.WriteLine($"👆 Użytkownik wybrał: {(answer ? "TAK" : "PÓŹNIEJ")}");

                if (answer)
                {
                    await _updateService.DownloadAndInstallUpdateAsync(update.DownloadUrl);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("✅ Brak dostępnych aktualizacji - nie pokazuję dialogu");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ BŁĄD w CheckForUpdatesAsync: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"❌ Stack: {ex.StackTrace}");
        }
    }

    private async Task LoadCategoriesAsync()
    {
        var cats = await _categoryRepository.GetAllCategoriesAsync();
        Categories.Clear();
        Categories.Add(new Category { ID = 0, Title = "Wszystkie", Color = "#9E9E9E" });
        foreach (var cat in cats)
        {
            Categories.Add(cat);
        }
        SelectedCategory = Categories[0];
    }

    [RelayCommand]
    private async Task LoadFlashcardsAsync()
    {
        IsLoading = true;
        try
        {
            _allFlashcards = await _repository.GetAllFlashcardsAsync();
            TotalCount = _allFlashcards.Count;
            ApplyFilter();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AddFlashcardAsync()
    {
        await Shell.Current.GoToAsync("addflashcard");
    }

    [RelayCommand]
    private async Task ImportFlashcardsAsync()
    {
        await Shell.Current.GoToAsync("importflashcards");
    }

    [RelayCommand]
    private async Task EditFlashcardAsync(Flashcard flashcard)
    {
        var parameters = new Dictionary<string, object>
        {
            { "FlashcardId", flashcard.Id }
        };
        await Shell.Current.GoToAsync("addflashcard", parameters);
    }

    [RelayCommand]
    private async Task DeleteFlashcardAsync(Flashcard flashcard)
    {
        bool answer = await Shell.Current.DisplayAlertAsync(
            "Usun fiszke", 
            $"Czy na pewno chcesz usunac fiszke '{flashcard.EnglishWord}'?", 
            "Tak", 
            "Nie");

        if (answer)
        {
            await _repository.DeleteFlashcardAsync(flashcard.Id);
            await LoadFlashcardsAsync();
        }
    }

    [RelayCommand]
    private async Task StartLearningAsync()
    {
        if (TotalCount == 0)
        {
            await Shell.Current.DisplayAlertAsync("Brak fiszek", "Dodaj najpierw kilka fiszek do nauki!", "OK");
            return;
        }

        await Shell.Current.GoToAsync("learningconfig");
    }

    [RelayCommand]
    private async Task ViewStatisticsAsync()
    {
        await Shell.Current.GoToAsync("statistics");
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    partial void OnSelectedCategoryChanged(Category? value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Flashcards.Clear();

        var filtered = _allFlashcards.AsEnumerable();

        if (SelectedCategory != null && SelectedCategory.ID > 0)
        {
            filtered = filtered.Where(f => f.CategoryId == SelectedCategory.ID);
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            filtered = filtered.Where(f => 
                f.EnglishWord.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                f.PolishTranslation.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var flashcard in filtered)
        {
            Flashcards.Add(flashcard);
        }
    }
}

