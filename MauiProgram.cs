using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using Fiszki.Data;
using Fiszki.PageModels;
using Fiszki.Pages;
using Fiszki.Services;

namespace Fiszki
{
    /// <summary>
    /// Główna klasa konfiguracyjna aplikacji MAUI Fiszki.
    /// Odpowiada za konfigurację serwisów, dependency injection, obsługę platform i stylów.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Tworzy i konfiguruje aplikację MAUI.
        /// Rejestruje wszystkie serwisy, repozytoria, strony i PageModels.
        /// </summary>
        /// <returns>Skonfigurowana instancja MauiApp</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()              // Dodaje Community Toolkit (Toast, Snackbar, etc.)
                .ConfigureSyncfusionToolkit()           // Dodaje Syncfusion (wykresy, kontrolki)
                .ConfigureMauiHandlers(handlers =>
                {
#if WINDOWS
                    // Konfiguracja specyficzna dla Windows - obsługa klawiatury i dostępności
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
                    // Używamy domyślnych fontów systemu - brak custom fontów
                });

#if DEBUG
            // W trybie DEBUG włączamy dodatkowe logowanie
            builder.Logging.AddDebug();
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            // === REJESTRACJA REPOZYTORIÓW (Singleton - jedna instancja przez cały czas działania) ===
            builder.Services.AddSingleton<FlashcardRepository>();           // Zarządzanie fiszkami w bazie
            builder.Services.AddSingleton<CategoryRepository>();            // Zarządzanie kategoriami
            builder.Services.AddSingleton<FlashcardCategoryRepository>();   // Relacje fiszki-kategorie

            // === REJESTRACJA SERWISÓW ===
            builder.Services.AddSingleton<FlashcardImportService>();        // Import/Export fiszek do JSON
            builder.Services.AddSingleton<UpdateService>();                 // Sprawdzanie i instalacja aktualizacji
            builder.Services.AddSingleton<DefaultFlashcardService>();       // Ładowanie domyślnych fiszek

            // === REJESTRACJA STRON I PAGE MODELS ===
            // FlashcardListPage - główna strona z listą fiszek (Singleton)
            builder.Services.AddSingleton<FlashcardListPageModel>();
            builder.Services.AddSingleton<FlashcardListPage>();

            // Pozostałe strony (Transient - nowa instancja przy każdym nawigowaniu)
            builder.Services.AddTransient<AddFlashcardPageModel>();         // Dodawanie nowej fiszki
            builder.Services.AddTransient<AddFlashcardPage>();

            builder.Services.AddTransient<LearnPageModel>();                // Sesja nauki z odwracaniem kart
            builder.Services.AddTransient<LearnPage>();

            builder.Services.AddTransient<ImportFlashcardsPageModel>();     // Import z pliku JSON
            builder.Services.AddTransient<ImportFlashcardsPage>();

            builder.Services.AddTransient<LearningConfigPageModel>();       // Konfiguracja parametrów sesji nauki
            builder.Services.AddTransient<LearningConfigPage>();

            builder.Services.AddTransient<StatisticsPageModel>();           // Statystyki i wykresy postępów
            builder.Services.AddTransient<StatisticsPage>();

            return builder.Build();
        }
    }
}
