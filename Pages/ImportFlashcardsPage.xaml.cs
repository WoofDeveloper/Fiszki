using Fiszki.PageModels;

namespace Fiszki.Pages;

public partial class ImportFlashcardsPage : ContentPage
{
    private readonly ImportFlashcardsPageModel _viewModel;

    public ImportFlashcardsPage(ImportFlashcardsPageModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}
