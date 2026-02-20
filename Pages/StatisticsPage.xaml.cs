using Fiszki.PageModels;

namespace Fiszki.Pages;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsPageModel _viewModel;

    public StatisticsPage(StatisticsPageModel viewModel)
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
