# 🌐 Konfiguracja systemu automatycznych aktualizacji

## Potrzebujesz:
- Konto GitHub (darmowe)
- Git zainstalowany lokalnie
- 10 minut czasu

---

## 📋 Krok po kroku:

### 1. Utwórz repozytorium na GitHub dla pliku version.json

**Opcja A - Przez stronę GitHub:**
1. Wejdź na https://github.com
2. Kliknij "+" w prawym górnym rogu → "New repository"
3. Nazwa: `fiszki-updates`
4. Opis: "Update information for Fiszki app"
5. Zaznacz "Public"
6. Zaznacz "Add a README file"
7. Kliknij "Create repository"

**Opcja B - Przez terminal:**
```bash
gh repo create fiszki-updates --public --description "Update info for Fiszki"
```

---

### 2. Upload pliku version.json do repozytorium

**Metoda 1 - Przez stronę:**
1. Otwórz swoje repo na GitHubie
2. Kliknij "Add file" → "Upload files"
3. Przeciągnij plik `version.json` z projektu
4. Kliknij "Commit changes"

**Metoda 2 - Przez Git:**
```bash
# Sklonuj repo
git clone https://github.com/TWOJ-USERNAME/fiszki-updates.git
cd fiszki-updates

# Skopiuj plik
copy ..\Fiszki\version.json .

# Commit i push
git add version.json
git commit -m "Add version 1.2 info"
git push
```

---

### 3. Uzyskaj link do pliku version.json

Po uploadzie, link będzie:
```
https://raw.githubusercontent.com/TWOJ-USERNAME/fiszki-updates/main/version.json
```

**Sprawdź czy działa**:
- Otwórz link w przeglądarce
- Powinien pokazać zawartość JSON

---

### 4. Utwórz Release głównego projektu

**Na GitHub:**
1. Otwórz główne repo projektu (lub utwórz nowe)
2. Kliknij "Releases" → "Create a new release"
3. Tag: `v1.2`
4. Title: "Wersja 1.2 - Wszystkie fiszki + Auto-update"
5. Description:
```markdown
## Nowa wersja 1.2!

### ✨ Co nowego:
- Opcja "Wszystkie fiszki" w konfiguracji nauki
- Nowa ikona z flagami Polski i Wielkiej Brytanii
- System automatycznych aktualizacji przez internet
- Poprawki błędów

### 📥 Instalacja:
Pobierz plik APK poniżej i zainstaluj na swoim urządzeniu.

### 🔄 Aktualizacja:
Jeśli masz już wersję 1.0 lub 1.1, po prostu zainstaluj - dane zostaną zachowane!
```
6. Upload pliku: `Fiszki-v1.2-AllCards-Update-Release.apk`
7. Kliknij "Publish release"

---

### 5. Skopiuj link do APK

Po utworzeniu release:
1. Prawym na plik APK → "Copy link address"
2. Link będzie wyglądał:
```
https://github.com/TWOJ-USERNAME/fiszki/releases/download/v1.2/Fiszki-v1.2-AllCards-Update-Release.apk
```

---

### 6. Zaktualizuj version.json

Otwórz plik `version.json` w repozytorium updates i zmień:

```json
{
  "version": "1.2",
  "versionCode": 2,
  "downloadUrl": "https://github.com/TWOJ-USERNAME/fiszki/releases/download/v1.2/Fiszki-v1.2-AllCards-Update-Release.apk",
  "releaseNotes": "✨ Nowa wersja 1.2!\n\n• Opcja 'Wszystkie fiszki'\n• Nowa ikona z flagami 🇵🇱 🇬🇧\n• Auto-update\n• Poprawki błędów",
  "releaseDate": "2026-02-19T23:00:00Z",
  "isRequired": false
}
```

**Commit zmiany:**
```bash
git add version.json
git commit -m "Update download URL for v1.2"
git push
```

---

### 7. Zaktualizuj kod aplikacji

Otwórz `Services/UpdateService.cs` i zmień linię 10:

```csharp
private const string UPDATE_CHECK_URL = "https://raw.githubusercontent.com/TWOJ-USERNAME/fiszki-updates/main/version.json";
```

**Podmień `TWOJ-USERNAME` na swój username GitHub!**

---

### 8. Przebuduj aplikację z nowym URL

```powershell
dotnet clean
dotnet publish -f net10.0-android -c Release /p:AndroidPackageFormat=apk
Copy-Item "bin\Release\net10.0-android\publish\com.fiszki.english-Signed.apk" -Destination "Fiszki-v1.2-Final.apk"
```

---

### 9. Zaktualizuj Release na GitHubie

1. Wejdź w Release v1.2
2. Kliknij "Edit release"
3. Usuń stary APK
4. Upload nowy: `Fiszki-v1.2-Final.apk`
5. Zapisz

---

### 10. Testowanie!

**Na telefonie z v1.1:**
1. Usuń aplikację i zainstaluj `Fiszki-v1.2-Final.apk`
2. Otwórz aplikację
3. Powinieneś NIE zobaczyć dialogu aktualizacji (bo masz najnowszą)

**Symulacja aktualizacji:**
1. Zmień w `version.json`: `"versionCode": 3` i `"version": "1.3"`
2. Commit i push
3. Zamknij i otwórz aplikację
4. Powinieneś zobaczyć dialog z aktualizacją!

---

## 🔄 Jak wydawać nowe wersje:

### 1. Zwiększ wersję w projekcie

W `Fiszki.csproj`:
```xml
<ApplicationDisplayVersion>1.3</ApplicationDisplayVersion>
<ApplicationVersion>3</ApplicationVersion>
```

### 2. Zbuduj APK

```powershell
dotnet publish -f net10.0-android -c Release
```

### 3. Utwórz Release na GitHub

```bash
gh release create v1.3 Fiszki-v1.3.apk --title "Wersja 1.3" --notes "Lista zmian..."
```

### 4. Zaktualizuj version.json

```json
{
  "version": "1.3",
  "versionCode": 3,
  "downloadUrl": "https://github.com/.../v1.3/Fiszki-v1.3.apk",
  "releaseNotes": "Co nowego w 1.3..."
}
```

### 5. Push i gotowe!

Użytkownicy przy następnym starcie aplikacji zobaczą powiadomienie o aktualizacji!

---

## 🎯 Alternatywne opcje (bez GitHub):

### Opcja 1: Google Drive
1. Upload `version.json` i APK na Drive
2. Ustaw udostępnianie: "Anyone with link"
3. Użyj direct download links (potrzebny converter)
4. **Minus**: Google ogranicza pobieranie dużych plików

### Opcja 2: Dropbox
1. Upload plików na Dropbox
2. Zmień `www.dropbox.com` na `dl.dropboxusercontent.com` w linkach
3. Użyj w kodzie
4. **Minus**: Limity transferu w darmowej wersji

### Opcja 3: Firebase Storage
1. Utwórz projekt Firebase
2. Upload plików do Storage
3. Użyj public URLs
4. **Plus**: Szybkie CDN, darmowe do 5GB
5. **Minus**: Bardziej skomplikowana konfiguracja

### Opcja 4: Własny serwer
1. Kup hosting (np. nazwa.pl, 10 zł/miesiąc)
2. Upload plików przez FTP
3. Pełna kontrola
4. **Minus**: Koszt

---

## 💡 Wskazówki:

✅ **GitHub jest najlepszą opcją** - darmowe, szybkie, niezawodne  
✅ **Testuj przed wydaniem** - zawsze sprawdź czy link działa  
✅ **Backup** - zachowaj stare wersje APK  
✅ **Changelog** - zawsze opisuj zmiany w releaseNotes  

---

## ❓ FAQ:

**Q: Czy muszę mieć GitHub?**  
A: Nie, możesz użyć innych opcji, ale GitHub jest zalecane.

**Q: Czy to kosztuje?**  
A: Nie, GitHub jest darmowy dla publicznych projektów.

**Q: Jak często sprawdzać aktualizacje?**  
A: Aplikacja sprawdza przy każdym starcie (zajmuje <3 sekundy).

**Q: Czy mogę wymusić aktualizację?**  
A: Tak, ustaw `"isRequired": true` w version.json.

**Q: Co jeśli link do APK nie działa?**  
A: Użytkownicy zobaczą błąd, ale aplikacja będzie działać normalnie.

---

**Gotowe! Masz działający system automatycznych aktualizacji! 🚀**
