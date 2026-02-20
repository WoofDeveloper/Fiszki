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
        await _viewModel.InitializeAsync();
    }
}
