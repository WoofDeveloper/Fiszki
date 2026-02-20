namespace Fiszki.Models;

public class AppVersion
{
    public string Version { get; set; } = string.Empty;
    public int VersionCode { get; set; }
    public string DownloadUrl { get; set; } = string.Empty;
    public string ReleaseNotes { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public bool IsRequired { get; set; }
}
