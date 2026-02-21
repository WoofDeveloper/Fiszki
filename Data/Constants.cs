namespace Fiszki.Data
{
    /// <summary>
    /// Stałe wykorzystywane w całej aplikacji, głównie ścieżki do bazy danych.
    /// </summary>
    public static class Constants
    {
        /// <summary>Nazwa pliku bazy danych SQLite</summary>
        public const string DatabaseFilename = "AppSQLite.db3";

        /// <summary>
        /// Pełna ścieżka do bazy danych SQLite w formacie connection string.
        /// Łączy katalog danych aplikacji (FileSystem.AppDataDirectory) z nazwą pliku bazy.
        /// Przykład: "Data Source=/data/user/0/com.fiszki.english/files/AppSQLite.db3"
        /// </summary>
        public static string DatabasePath =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";
    }
}