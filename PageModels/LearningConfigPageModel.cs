using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fiszki.Data;
using Fiszki.Models;
using System.Collections.ObjectModel;

namespace Fiszki.PageModels;

public partial class LearningConfigPageModel : ObservableObject
{
    private readonly FlashcardRepository _flashcardRepository;
    private readonly FlashcardCategoryRepository _categoryRepository;

    [ObservableProperty]
    private ObservableCollection<Category> _categories = new();

    [ObservableProperty]
    private Category? _selectedCategory;

    [ObservableProperty]
    private int _flashcardCount = 10;

    [ObservableProperty]
    private bool _useAllFlashcards;

    [ObservableProperty]
    private bool _onlyDueForReview;

    [ObservableProperty]
    private bool _prioritizeWrongAnswers = true;

    [ObservableProperty]
    private int _totalAvailable;

    [ObservableProperty]
    private int _dueForReview;

    public string DisplayCount => UseAllFlashcards ? $"{TotalAvailable} (wszystkie)" : FlashcardCount.ToString();

    public LearningConfigPageModel(FlashcardRepository flashcardRepository, FlashcardCategoryRepository categoryRepository)
    {
        _flashcardRepository = flashcardRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task InitializeAsync()
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        Categories.Clear();
        Categories.Add(new Category { ID = 0, Title = "Wszystkie kategorie", Color = "#9E9E9E" });

        foreach (var category in categories)
        {
            Categories.Add(category);
        }

        SelectedCategory = Categories[0];
        await UpdateStatisticsAsync();
    }

    partial void OnSelectedCategoryChanged(Category? value)
    {
        _ = UpdateStatisticsAsync();
    }

    partial void OnOnlyDueForReviewChanged(bool value)
    {
        _ = UpdateStatisticsAsync();
    }

    partial void OnUseAllFlashcardsChanged(bool value)
    {
        OnPropertyChanged(nameof(DisplayCount));
    }

    partial void OnFlashcardCountChanged(int value)
    {
        OnPropertyChanged(nameof(DisplayCount));
    }

    partial void OnTotalAvailableChanged(int value)
    {
        OnPropertyChanged(nameof(DisplayCount));
    }

    private async Task UpdateStatisticsAsync()
    {
        var allFlashcards = await _flashcardRepository.GetAllFlashcardsAsync();

        if (SelectedCategory?.ID > 0)
        {
            allFlashcards = allFlashcards.Where(f => f.CategoryId == SelectedCategory.ID).ToList();
        }

        TotalAvailable = allFlashcards.Count;
        DueForReview = allFlashcards.Count(f => f.NeedsReview);
    }

    [RelayCommand]
    private async Task StartLearningAsync()
    {
        if (TotalAvailable == 0)
        {
            await Shell.Current.DisplayAlert("Brak fiszek", "Brak dostepnych fiszek do nauki w wybranej kategorii", "OK");
            return;
        }

        int countToUse = UseAllFlashcards ? TotalAvailable : Math.Min(FlashcardCount, TotalAvailable);

        var config = new LearningSessionConfig
        {
            CategoryId = SelectedCategory?.ID > 0 ? SelectedCategory.ID : null,
            Count = countToUse,
            OnlyDueForReview = OnlyDueForReview,
            PrioritizeWrongAnswers = PrioritizeWrongAnswers
        };

        var navigationParameter = new Dictionary<string, object>
        {
            { "Config", config }
        };

        await Shell.Current.GoToAsync("learn", navigationParameter);
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
