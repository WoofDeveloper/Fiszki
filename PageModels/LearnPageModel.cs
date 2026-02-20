using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fiszki.Data;
using Fiszki.Models;

namespace Fiszki.PageModels;

[QueryProperty(nameof(Config), "Config")]
public partial class LearnPageModel : ObservableObject
{
    private readonly FlashcardRepository _repository;
    private List<Flashcard> _flashcards = new();
    private int _currentIndex = 0;

    [ObservableProperty]
    private LearningSessionConfig? config;

    [ObservableProperty]
    private Flashcard? currentFlashcard;

    [ObservableProperty]
    private bool isCardFlipped;

    [ObservableProperty]
    private int currentCardNumber;

    [ObservableProperty]
    private int totalCards;

    [ObservableProperty]
    private int correctCount;

    [ObservableProperty]
    private int incorrectCount;

    [ObservableProperty]
    private bool isSessionCompleted;

    public LearnPageModel(FlashcardRepository repository)
    {
        _repository = repository;
    }

    public async Task InitializeAsync()
    {
        await LoadFlashcardsAsync();
    }

    partial void OnConfigChanged(LearningSessionConfig? value)
    {
        if (value != null)
        {
            _ = LoadFlashcardsAsync();
        }
    }

    private async Task LoadFlashcardsAsync()
    {
        if (Config != null)
        {
            _flashcards = await _repository.GetFlashcardsForLearningAsync(Config);
        }
        else
        {
            _flashcards = await _repository.GetRandomFlashcardsAsync(10);
        }

        TotalCards = _flashcards.Count;
        _currentIndex = 0;
        CorrectCount = 0;
        IncorrectCount = 0;
        IsSessionCompleted = false;
        ShowNextCard();
    }

    private void ShowNextCard()
    {
        if (_currentIndex < _flashcards.Count)
        {
            CurrentFlashcard = _flashcards[_currentIndex];
            CurrentCardNumber = _currentIndex + 1;
            IsCardFlipped = false;
        }
        else
        {
            IsSessionCompleted = true;
            CurrentFlashcard = null;
        }
    }

    [RelayCommand]
    private async Task FlipCardAsync()
    {
        if (CurrentFlashcard == null) return;

        IsCardFlipped = !IsCardFlipped;
    }

    [RelayCommand]
    private async Task MarkAsCorrectAsync()
    {
        if (CurrentFlashcard == null) return;

        CorrectCount++;
        CurrentFlashcard.CorrectAnswers++;
        CurrentFlashcard.RepetitionLevel++;
        CurrentFlashcard.TimesReviewed++;
        CurrentFlashcard.LastReviewed = DateTime.Now;
        CurrentFlashcard.NextReview = _repository.CalculateNextReview(true, CurrentFlashcard.RepetitionLevel);

        await _repository.UpdateFlashcardAsync(CurrentFlashcard);

        _currentIndex++;
        ShowNextCard();
    }

    [RelayCommand]
    private async Task MarkAsIncorrectAsync()
    {
        if (CurrentFlashcard == null) return;

        IncorrectCount++;
        CurrentFlashcard.IncorrectAnswers++;
        CurrentFlashcard.RepetitionLevel = Math.Max(0, CurrentFlashcard.RepetitionLevel - 1);
        CurrentFlashcard.TimesReviewed++;
        CurrentFlashcard.LastReviewed = DateTime.Now;
        CurrentFlashcard.NextReview = _repository.CalculateNextReview(false, CurrentFlashcard.RepetitionLevel);

        await _repository.UpdateFlashcardAsync(CurrentFlashcard);

        _currentIndex++;
        ShowNextCard();
    }

    [RelayCommand]
    private async Task FinishSessionAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task RestartSessionAsync()
    {
        await LoadFlashcardsAsync();
    }
}

