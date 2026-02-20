# Jak zbudować APK Release dla aplikacji Fiszki

## Metoda 1: Przez Visual Studio (Najprostsza)

### Krok 1: Przygotowanie
1. Otwórz projekt w Visual Studio
2. Upewnij się, że wszystkie zmiany są zapisane
3. Wyczyść poprzednie buildy: **Build → Clean Solution**

### Krok 2: Zmiana konfiguracji
1. Na górnym pasku wybierz **Release** zamiast Debug
2. Wybierz **net10.0-android** jako target framework
3. Wybierz urządzenie docelowe (dowolny emulator Android lub "Generic Android Device")

### Krok 3: Build APK
1. Kliknij prawym na projekt **Fiszki** w Solution Explorer
2. Wybierz **Publish**
3. Wybierz **Ad Hoc** (dla testów) lub **Archive**
4. Kliknij **Create**

### Krok 4: Znajdź plik APK
Plik APK będzie w jednym z tych lokalizacji:
- `bin\Release\net10.0-android\`
- `bin\Release\net10.0-android\publish\`
- `bin\Release\net10.0-android\android-arm64\`

Szukaj pliku: `com.fiszki.english-Signed.apk` lub `Fiszki.apk`

---

## Metoda 2: Przez wiersz poleceń (.NET CLI)

### Dla nie-podpisanego APK (testowanie wewnętrzne):
```powershell
# Wyczyść poprzednie buildy
dotnet clean

# Zbuduj APK Release
dotnet publish -f net10.0-android -c Release
```

### Dla podpisanego APK (dystrybucja):
```powershell
# Utwórz keystore (tylko raz, zachowaj hasło!)
keytool -genkeypair -v -keystore fiszki.keystore -alias fiszki -keyalg RSA -keysize 2048 -validity 10000

# Zbuduj i podpisz APK
dotnet publish -f net10.0-android -c Release ^
  /p:AndroidKeyStore=true ^
  /p:AndroidSigningKeyStore=fiszki.keystore ^
  /p:AndroidSigningKeyAlias=fiszki ^
  /p:AndroidSigningKeyPass=TWOJE_HASLO ^
  /p:AndroidSigningStorePass=TWOJE_HASLO
```

### Lokalizacja pliku APK:
```
bin\Release\net10.0-android\publish\com.fiszki.english-Signed.apk
```

---

## Metoda 3: Przez MSBuild (dla zaawansowanych)

```powershell
# Przywróć pakiety
dotnet restore

# Zbuduj Release
msbuild Fiszki.csproj /t:SignAndroidPackage /p:Configuration=Release
```

---

## Instalacja APK na urządzeniu testowym

### Przez USB (z włączonym USB Debugging):
```powershell
# Zainstaluj APK
adb install bin\Release\net10.0-android\publish\com.fiszki.english-Signed.apk

# Jeśli aplikacja już istnieje, użyj -r (reinstall):
adb install -r bin\Release\net10.0-android\publish\com.fiszki.english-Signed.apk
```

### Przez udostępnienie pliku:
1. Skopiuj plik APK na telefon (np. przez Gmail, Google Drive, WeTransfer)
2. Na telefonie otwórz plik APK
3. Zezwól na instalację z nieznanych źródeł (jeśli system poprosi)
4. Zainstaluj aplikację

---

## Sprawdzanie rozmiaru i wersji APK

```powershell
# Informacje o APK
aapt dump badging bin\Release\net10.0-android\publish\com.fiszki.english-Signed.apk

# Sprawdź rozmiar
dir bin\Release\net10.0-android\publish\*.apk
```

---

## Rozwiązywanie problemów

### Problem: "No Android SDK found"
**Rozwiązanie**: Zainstaluj Android SDK przez Visual Studio Installer → Modify → Mobile development with .NET

### Problem: "Build failed - AndroidKeyStore"
**Rozwiązanie**: Ustaw `<AndroidKeyStore>false</AndroidKeyStore>` w pliku .csproj (już ustawione)

### Problem: APK jest za duży (>100MB)
**Rozwiązanie**: 
1. Zmień na AAB zamiast APK w .csproj: `<AndroidPackageFormat>aab</AndroidPackageFormat>`
2. Włącz linkowanie: `<AndroidLinkMode>Full</AndroidLinkMode>`

### Problem: Aplikacja crashuje na starcie
**Rozwiązanie**: Sprawdź logi ADB:
```powershell
adb logcat | findstr "fiszki"
```

---

## Szybka komenda (Kopiuj-Wklej):

```powershell
# Zbuduj Release APK i skopiuj do folderu głównego
dotnet clean
dotnet publish -f net10.0-android -c Release
copy bin\Release\net10.0-android\publish\*.apk Fiszki-Release.apk
```

Plik APK będzie w głównym folderze projektu jako `Fiszki-Release.apk`

---

## Informacje o aplikacji

- **Nazwa wyświetlana**: Fiszki - Nauka Angielskiego
- **Package name**: com.fiszki.english
- **Wersja**: 1.0 (build 1)
- **Min. Android**: 5.0 (API 21)
- **Target Android**: najnowszy (API 35+)

---

## Udostępnianie testerom

### Opcja 1: Email/Drive
1. Wyślij plik APK przez Gmail lub Google Drive
2. Napisz instrukcję: "Pobierz plik → Otwórz → Zainstaluj"

### Opcja 2: Firebase App Distribution
1. Zarejestruj się na Firebase
2. Dodaj projekt
3. Użyj Firebase CLI do uploadu APK
4. Wyślij link testerom

### Opcja 3: GitHub Releases
1. Utwórz Release na GitHubie
2. Dodaj APK jako asset
3. Wyślij link do release

---

**Gotowe!** APK jest gotowy do testowania! 🎉
