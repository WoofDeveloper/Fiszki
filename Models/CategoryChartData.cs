namespace Fiszki.Models
{
    /// <summary>
    /// Model danych dla wykresu kołowego pokazującego liczbę fiszek w każdej kategorii.
    /// Używany na stronie statystyk (StatisticsPage) w komponencie CategoryChart.
    /// </summary>
    public class CategoryChartData
    {
        /// <summary>Nazwa kategorii (np. "Człowiek", "Zwierzęta")</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Liczba fiszek w danej kategorii</summary>
        public int Count { get; set; }

        /// <summary>
        /// Konstruktor tworzący dane dla jednej kategorii na wykresie.
        /// </summary>
        /// <param name="title">Nazwa kategorii</param>
        /// <param name="count">Liczba fiszek</param>
        public CategoryChartData(string title, int count)
        {
            Title = title;
            Count = count;
        }
    }
}