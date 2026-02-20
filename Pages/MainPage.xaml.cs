using Fiszki.Models;
using Fiszki.PageModels;

namespace Fiszki.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}