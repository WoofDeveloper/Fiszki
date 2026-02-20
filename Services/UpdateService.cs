using System.Text.Json;
using Fiszki.Models;

namespace Fiszki.Services;

public class UpdateService
{
    private const string UPDATE_CHECK_URL = "https://raw.githubusercontent.com/twoj-username/fiszki-updates/main/version.json";
    private readonly HttpClient _httpClient;

    public UpdateService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<AppVersion?> CheckForUpdatesAsync()
    {
        try
        {
            var currentVersion = GetCurrentVersionCode();
            var response = await _httpClient.GetStringAsync(UPDATE_CHECK_URL);
            var latestVersion = JsonSerializer.Deserialize<AppVersion>(response);

            if (latestVersion != null && latestVersion.VersionCode > currentVersion)
            {
                return latestVersion;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public int GetCurrentVersionCode()
    {
#if ANDROID
        var context = Android.App.Application.Context;
        var packageInfo = context.PackageManager?.GetPackageInfo(context.PackageName!, 0);
        return (int)(packageInfo?.LongVersionCode ?? 2);
#else
        return 2; // wersja 1.2
#endif
    }

    public string GetCurrentVersion()
    {
        return "1.2";
    }

    public async Task DownloadAndInstallUpdateAsync(string url)
    {
#if ANDROID
        try
        {
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Android.Content.Intent(Android.Content.Intent.ActionView, uri);
            intent.SetFlags(Android.Content.ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error opening download: {ex.Message}");
        }
#endif
    }
}
