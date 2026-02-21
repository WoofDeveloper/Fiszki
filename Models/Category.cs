using System.Text.Json.Serialization;

namespace Fiszki.Models
{
    /// <summary>
    /// Model reprezentujący kategorię fiszek (np. "Człowiek", "Zwierzęta", "Przyroda").
    /// Kategorie pozwalają na organizację fiszek w tematyczne grupy.
    /// </summary>
    public class Category
    {
        /// <summary>Unikalny identyfikator kategorii w bazie danych</summary>
        public int ID { get; set; }

        /// <summary>Nazwa kategorii wyświetlana użytkownikowi (np. "Człowiek")</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Kolor kategorii w formacie hex (np. "#FF0000" dla czerwonego)</summary>
        public string Color { get; set; } = "#FF0000";

        /// <summary>
        /// Kolor kategorii jako obiekt Brush gotowy do użycia w XAML.
        /// Konwertuje string hex na SolidColorBrush.
        /// Ignorowany podczas serializacji JSON.
        /// </summary>
        [JsonIgnore]
        public Brush ColorBrush
        {
            get
            {
                return new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb(Color));
            }
        }

        /// <summary>
        /// Reprezentacja tekstowa kategorii.
        /// Zwraca nazwę kategorii (Title).
        /// </summary>
        public override string ToString() => $"{Title}";
    }
}