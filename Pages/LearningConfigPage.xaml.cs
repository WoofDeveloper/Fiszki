using Fiszki.PageModels;

namespace Fiszki.Pages;

public partial class LearningConfigPage : ContentPage
{
    private readonly LearningConfigPageModel _viewModel;

    public LearningConfigPage(LearningConfigPageModel viewModel)
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
