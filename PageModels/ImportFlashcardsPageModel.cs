using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fiszki.Services;

namespace Fiszki.PageModels;

public partial class ImportFlashcardsPageModel : ObservableObject
{
    private readonly FlashcardImportService _importService;

    [ObservableProperty]
    private string jsonContent = string.Empty;

    [ObservableProperty]
    private string resultMessage = string.Empty;

    [ObservableProperty]
    private bool showResult;

    public ImportFlashcardsPageModel(FlashcardImportService importService)
    {
        _importService = importService;
        LoadSampleJson();
    }

    private void LoadSampleJson()
    {
        JsonContent = _importService.GetSampleJson();
    }

    [RelayCommand]
    private async Task ImportAsync()
    {
        if (string.IsNullOrWhiteSpace(JsonContent))
        {
            await Shell.Current.DisplayAlertAsync("Blad", "Wpisz dane JSON", "OK");
            return;
        }

        var (imported, failed, message) = await _importService.ImportFromJsonAsync(JsonContent);
        
        ResultMessage = message;
        ShowResult = true;

        if (imported > 0)
        {
            await Task.Delay(2000);
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    private async Task PickFileAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Wybierz plik JSON",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/json", "text/plain" } },
                    { DevicePlatform.iOS, new[] { "public.json", "public.plain-text" } },
                    { DevicePlatform.WinUI, new[] { ".json", ".txt" } },
                    { DevicePlatform.macOS, new[] { "json", "txt" } }
                })
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var reader = new StreamReader(stream);
                JsonContent = await reader.ReadToEndAsync();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Blad", $"Nie udalo sie wczytac pliku: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private void ClearJson()
    {
        JsonContent = string.Empty;
        ShowResult = false;
        ResultMessage = string.Empty;
    }

    [RelayCommand]
    private void LoadSample()
    {
        LoadSampleJson();
        ShowResult = false;
        ResultMessage = string.Empty;
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
