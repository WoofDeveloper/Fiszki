using Fiszki.PageModels;

namespace Fiszki.Pages;

public partial class AddFlashcardPage : ContentPage
{
    private readonly AddFlashcardPageModel _viewModel;

    public AddFlashcardPage(AddFlashcardPageModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Reset();
    }
}
