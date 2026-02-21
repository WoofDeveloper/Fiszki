using Fiszki.PageModels;

namespace Fiszki.Pages;

public partial class FlashcardListPage : ContentPage
{
    private readonly FlashcardListPageModel _viewModel;

    public FlashcardListPage(FlashcardListPageModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine("🌟 FlashcardListPage.OnAppearing() - strona się pokazuje");
        await _viewModel.InitializeAsync();
        System.Diagnostics.Debug.WriteLine("✅ FlashcardListPage.OnAppearing() - InitializeAsync zakończone");
    }
}
