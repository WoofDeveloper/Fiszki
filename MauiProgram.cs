using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using Fiszki.Data;
using Fiszki.PageModels;
using Fiszki.Pages;
using Fiszki.Services;

namespace Fiszki
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
#if WINDOWS
    				Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper.AppendToMapping("KeyboardAccessibleCollectionView", (handler, view) =>
    				{
    					handler.PlatformView.SingleSelectionFollowsFocus = false;
    				});

    				Microsoft.Maui.Handlers.ContentViewHandler.Mapper.AppendToMapping(nameof(Pages.Controls.CategoryChart), (handler, view) =>
    				{
    					if (view is Pages.Controls.CategoryChart && handler.PlatformView is Microsoft.Maui.Platform.ContentPanel contentPanel)
    					{
    						contentPanel.IsTabStop = true;
    					}
    				});
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            builder.Services.AddSingleton<FlashcardRepository>();
            builder.Services.AddSingleton<FlashcardCategoryRepository>();
            builder.Services.AddSingleton<FlashcardImportService>();
            builder.Services.AddSingleton<UpdateService>();
            builder.Services.AddSingleton<FlashcardListPageModel>();
            builder.Services.AddSingleton<FlashcardListPage>();
            builder.Services.AddTransient<AddFlashcardPageModel>();
            builder.Services.AddTransient<AddFlashcardPage>();
            builder.Services.AddTransient<LearnPageModel>();
            builder.Services.AddTransient<LearnPage>();
            builder.Services.AddTransient<ImportFlashcardsPageModel>();
            builder.Services.AddTransient<ImportFlashcardsPage>();
            builder.Services.AddTransient<LearningConfigPageModel>();
            builder.Services.AddTransient<LearningConfigPage>();
            builder.Services.AddTransient<StatisticsPageModel>();
            builder.Services.AddTransient<StatisticsPage>();

            return builder.Build();
        }
    }
}
