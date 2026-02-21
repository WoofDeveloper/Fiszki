using Microsoft.Extensions.DependencyInjection;
using Fiszki.Services;

namespace Fiszki
{
    /// <summary>
    /// Główna klasa aplikacji Fiszki - zarządza cyklem życia aplikacji.
    /// Odpowiada za inicjalizację i ładowanie domyślnych fiszek przy starcie.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Konstruktor aplikacji.
        /// Przyjmuje DefaultFlashcardService przez dependency injection i automatycznie
        /// ładuje domyślne fiszki przy pierwszym uruchomieniu lub aktualizacji.
        /// </summary>
        /// <param name="defaultFlashcardService">Serwis zarządzający domyślnymi fiszkami</param>
        public App(DefaultFlashcardService defaultFlashcardService)
        {
            InitializeComponent();

            // Załaduj domyślne fiszki przy pierwszym uruchomieniu lub aktualizacji
            Task.Run(async () => await defaultFlashcardService.LoadDefaultFlashcardsIfNeededAsync());
        }

        /// <summary>
        /// Tworzy główne okno aplikacji.
        /// Zwraca nową instancję okna z głównym shellem aplikacji.
        /// </summary>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}