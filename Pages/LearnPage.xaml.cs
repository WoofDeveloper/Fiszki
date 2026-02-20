using Fiszki.PageModels;

namespace Fiszki.Pages;

public partial class LearnPage : ContentPage
{
    private readonly LearnPageModel _viewModel;

    public LearnPage(LearnPageModel viewModel)
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
