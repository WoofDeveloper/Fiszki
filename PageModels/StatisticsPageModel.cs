using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fiszki.Data;
using Fiszki.Models;

namespace Fiszki.PageModels;

public partial class StatisticsPageModel : ObservableObject
{
    private readonly FlashcardRepository _flashcardRepository;

    [ObservableProperty]
    private LearningStatistics? _statistics;

    [ObservableProperty]
    private bool _isLoading = true;

    public StatisticsPageModel(FlashcardRepository flashcardRepository)
    {
        _flashcardRepository = flashcardRepository;
    }

    public async Task InitializeAsync()
    {
        IsLoading = true;
        Statistics = await _flashcardRepository.GetStatisticsAsync();
        IsLoading = false;
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await InitializeAsync();
    }

    [RelayCommand]
    private async Task CloseAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
