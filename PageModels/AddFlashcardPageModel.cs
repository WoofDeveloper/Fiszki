using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fiszki.Data;
using Fiszki.Models;
using System.Collections.ObjectModel;

namespace Fiszki.PageModels;

public partial class AddFlashcardPageModel : ObservableObject, IQueryAttributable
{
    private readonly FlashcardRepository _repository;
    private readonly FlashcardCategoryRepository _categoryRepository;

    [ObservableProperty]
    private string englishWord = string.Empty;

    [ObservableProperty]
    private string polishTranslation = string.Empty;

    [ObservableProperty]
    private string example = string.Empty;

    [ObservableProperty]
    private string pageTitle = "Dodaj fiszke";

    [ObservableProperty]
    private bool isEditMode;

    [ObservableProperty]
    private ObservableCollection<Category> categories = new();

    [ObservableProperty]
    private Category? selectedCategory;

    private int? _editingFlashcardId;

    public AddFlashcardPageModel(FlashcardRepository repository, FlashcardCategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        LoadCategoriesAsync();
    }

    private async void LoadCategoriesAsync()
    {
        var cats = await _categoryRepository.GetAllCategoriesAsync();
        Categories.Clear();
        Categories.Add(new Category { ID = 0, Title = "Brak kategorii", Color = "#9E9E9E" });
        foreach (var cat in cats)
        {
            Categories.Add(cat);
        }
        SelectedCategory = Categories[0];
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("FlashcardId"))
        {
            var flashcardId = (int)query["FlashcardId"];
            LoadFlashcardAsync(flashcardId);
        }
    }

    private async void LoadFlashcardAsync(int id)
    {
        var flashcard = await _repository.GetFlashcardByIdAsync(id);
        if (flashcard != null)
        {
            _editingFlashcardId = flashcard.Id;
            EnglishWord = flashcard.EnglishWord;
            PolishTranslation = flashcard.PolishTranslation;
            Example = flashcard.Example ?? string.Empty;

            if (flashcard.CategoryId.HasValue)
            {
                SelectedCategory = Categories.FirstOrDefault(c => c.ID == flashcard.CategoryId.Value) ?? Categories[0];
            }

            IsEditMode = true;
            PageTitle = "Edytuj fiszke";
        }
    }

    [RelayCommand]
    private async Task SaveFlashcardAsync()
    {
        if (string.IsNullOrWhiteSpace(EnglishWord) || string.IsNullOrWhiteSpace(PolishTranslation))
        {
            await Shell.Current.DisplayAlertAsync("Blad", "Wypelnij wymagane pola (slowo angielskie i tlumaczenie)", "OK");
            return;
        }

        int? categoryId = SelectedCategory?.ID > 0 ? SelectedCategory.ID : null;

        if (IsEditMode && _editingFlashcardId.HasValue)
        {
            var flashcard = await _repository.GetFlashcardByIdAsync(_editingFlashcardId.Value);
            if (flashcard != null)
            {
                flashcard.EnglishWord = EnglishWord.Trim();
                flashcard.PolishTranslation = PolishTranslation.Trim();
                flashcard.Example = string.IsNullOrWhiteSpace(Example) ? null : Example.Trim();
                flashcard.CategoryId = categoryId;
                await _repository.UpdateFlashcardAsync(flashcard);
            }
        }
        else
        {
            var flashcard = new Flashcard
            {
                EnglishWord = EnglishWord.Trim(),
                PolishTranslation = PolishTranslation.Trim(),
                Example = string.IsNullOrWhiteSpace(Example) ? null : Example.Trim(),
                CategoryId = categoryId,
                CreatedDate = DateTime.Now,
                LastReviewed = DateTime.Now,
                NextReview = DateTime.Now
            };

            await _repository.AddFlashcardAsync(flashcard);
        }

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public void Reset()
    {
        EnglishWord = string.Empty;
        PolishTranslation = string.Empty;
        Example = string.Empty;
        IsEditMode = false;
        PageTitle = "Dodaj fiszke";
        _editingFlashcardId = null;
        SelectedCategory = Categories.FirstOrDefault();
    }
}
