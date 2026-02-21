using System.Text.Json;
using Fiszki.Models;

namespace Fiszki.Services;

/// <summary>
/// Serwis odpowiedzialny za sprawdzanie dostępności aktualizacji aplikacji.
/// Porównuje aktualną wersję z wersją na GitHubie i umożliwia pobranie nowej wersji.
/// </summary>
public class UpdateService
{
    /// <summary>
    /// URL do pliku version.json na GitHubie zawierającego informacje o najnowszej wersji.
    /// Format pliku: { "version": "1.4.1", "versionCode": 5, "downloadUrl": "...", ... }
    /// </summary>
    private const string UPDATE_CHECK_URL = "https://raw.githubusercontent.com/WoofDeveloper/fiszki-updates/main/version.json";

    private readonly HttpClient _httpClient;

    public UpdateService()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Sprawdza czy dostępna jest nowsza wersja aplikacji.
    /// Pobiera plik version.json z GitHuba i porównuje VersionCode z aktualną wersją.
    /// </summary>
    /// <returns>
    /// Obiekt AppVersion jeśli dostępna jest aktualizacja, null jeśli nie ma aktualizacji lub wystąpił błąd
    /// </returns>
    public async Task<AppVersion?> CheckForUpdatesAsync()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("🔍 Sprawdzam aktualizacje...");
            System.Diagnostics.Debug.WriteLine($"📡 URL: {UPDATE_CHECK_URL}");

            var currentVersion = GetCurrentVersionCode();
            System.Diagnostics.Debug.WriteLine($"📱 Aktualna wersja: {currentVersion}");

            // Pobierz plik version.json z GitHuba
            var response = await _httpClient.GetStringAsync(UPDATE_CHECK_URL);
            System.Diagnostics.Debug.WriteLine($"✅ Odpowiedź: {response}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true  // Ignoruj wielkość liter w nazwach właściwości
            };

            var latestVersion = JsonSerializer.Deserialize<AppVersion>(response, options);
            System.Diagnostics.Debug.WriteLine($"📝 Deserializacja: {(latestVersion != null ? "SUKCES" : "NULL")}");

            if (latestVersion != null)
            {
                System.Diagnostics.Debug.WriteLine($"🆕 Najnowsza wersja: {latestVersion.VersionCode}");

                // Porównaj kody wersji - wyższy = nowsza wersja
                if (latestVersion.VersionCode > currentVersion)
                {
                    System.Diagnostics.Debug.WriteLine("🎉 DOSTĘPNA AKTUALIZACJA!");
                    return latestVersion;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("✅ Masz najnowszą wersję");
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Błąd sprawdzania aktualizacji: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Pobiera kod wersji aktualnie zainstalowanej aplikacji.
    /// Kod wersji to liczba całkowita (np. 5) pochodząca z ApplicationVersion w Fiszki.csproj.
    /// </summary>
    /// <returns>Kod wersji jako int (np. 5 dla wersji 1.4.1)</returns>
    public int GetCurrentVersionCode()
    {
        // 🧪 TESTOWANIE: Odkomentuj linię poniżej żeby symulować starą wersję
        // return 1; // Symuluje wersję 1.1 - zobaczysz dialog aktualizacji!

#if ANDROID
        // Na Androidzie pobieramy kod wersji z PackageManager
        var context = Android.App.Application.Context;
        var packageInfo = context.PackageManager?.GetPackageInfo(context.PackageName!, 0);

        if (packageInfo == null)
            return 2;

        // Android 9+ (API 28+) używa LongVersionCode
        if (OperatingSystem.IsAndroidVersionAtLeast(28))
        {
            return (int)packageInfo.LongVersionCode;
        }
        else
        {
            // Starsze wersje Androida używają VersionCode
            return packageInfo.VersionCode;
        }
#else
        // Na innych platformach zwróć domyślną wartość
        return 2;
#endif
    }

    /// <summary>
    /// Pobiera tekstową wersję aplikacji (np. "1.4.1").
    /// Uwaga: Ta metoda zwraca stałą wartość i powinna być zaktualizowana lub usunięta.
    /// </summary>
    /// <returns>String z numerem wersji</returns>
    public string GetCurrentVersion()
    {
        return "1.2";  // TODO: Należy pobierać z ApplicationDisplayVersion
    }

    /// <summary>
    /// Otwiera przeglądarkę z linkiem do pobrania nowej wersji aplikacji.
    /// Na Androidzie uruchamia Intent z ACTION_VIEW wskazującym na URL do APK.
    /// </summary>
    /// <param name="url">URL do pliku APK do pobrania</param>
    public async Task DownloadAndInstallUpdateAsync(string url)
    {
#if ANDROID
        try
        {
            // Utwórz Intent z akcją VIEW aby otworzyć URL w przeglądarce
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Android.Content.Intent(Android.Content.Intent.ActionView, uri);
            intent.SetFlags(Android.Content.ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ Błąd otwierania linku do pobrania: {ex.Message}");
        }
#endif
    }
}
