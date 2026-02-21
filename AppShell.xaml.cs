using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace Fiszki
{
    /// <summary>
    /// Główny shell aplikacji Fiszki - zarządza nawigacją między stronami.
    /// Definiuje dostępne trasy (routes) i udostępnia metody do wyświetlania powiadomień.
    /// </summary>
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Konstruktor - rejestruje wszystkie trasy nawigacyjne aplikacji.
        /// Trasy umożliwiają nawigację do różnych stron za pomocą Shell.GoToAsync().
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            // Rejestracja tras nawigacyjnych dla wszystkich stron aplikacji
            Routing.RegisterRoute("addflashcard", typeof(Pages.AddFlashcardPage));      // Dodawanie nowej fiszki
            Routing.RegisterRoute("learn", typeof(Pages.LearnPage));                     // Sesja nauki z odwracaniem fiszek
            Routing.RegisterRoute("importflashcards", typeof(Pages.ImportFlashcardsPage)); // Import fiszek z JSON
            Routing.RegisterRoute("learningconfig", typeof(Pages.LearningConfigPage));   // Konfiguracja sesji nauki
            Routing.RegisterRoute("statistics", typeof(Pages.StatisticsPage));           // Statystyki i wykresy postępów
        }

        /// <summary>
        /// Wyświetla Snackbar - powiadomienie na dole ekranu z czerwonym tłem.
        /// Używane do wyświetlania ważnych informacji lub błędów.
        /// </summary>
        /// <param name="message">Treść komunikatu do wyświetlenia</param>
        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),     // Czerwony kolor tła
                TextColor = Colors.White,                        // Biały tekst
                ActionButtonTextColor = Colors.Yellow,           // Żółty przycisk akcji
                CornerRadius = new CornerRadius(0),             // Brak zaokrąglenia rogów
                Font = Font.SystemFontOfSize(18),               // Rozmiar czcionki
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Wyświetla Toast - krótkie powiadomienie wyświetlane na ekranie.
        /// Nie działa na Windows, tylko na platformach mobilnych.
        /// </summary>
        /// <param name="message">Treść komunikatu do wyświetlenia</param>
        public static async Task DisplayToastAsync(string message)
        {
            // Toast nie działa na Windows, pomijamy
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }
    }
}

