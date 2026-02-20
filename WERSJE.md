# Historia wersji - Fiszki

## Wersja 1.1 - Flipcards (19.02.2026)

### ✨ Główna zmiana: Odwracanie kart!
- ✅ **Nowy tryb nauki z odwracaniem kart** (Flashcards)
  - Kliknij na kartę aby ją odwrócić
  - Najpierw angielskie słowo 🇬🇧, po odwróceniu polskie 🇵🇱
  - Samoocena: "Znałem" lub "Nie znałem"
  - Szybsza i bardziej naturalna nauka
  - Brak potrzeby wpisywania odpowiedzi

### 🎨 UI/UX
- Wizualna karta z flagami emoji
- Przyjemne kolory (niebieski dla angielskiego, zielony dla polskiego)
- Duże, czytelne czcionki
- Przyciski z emotkami (✅ ❌)
- Płynne przejścia między kartami

### 🔧 Techniczne
- Usunięte pola: `UserAnswer`, `ShowAnswer`, `FeedbackMessage`, `FeedbackColor`
- Dodane pole: `IsCardFlipped`
- Nowe komendy: `FlipCardCommand`, `MarkAsCorrectCommand`, `MarkAsIncorrectCommand`
- Usunięte komendy: `CheckAnswerCommand`, `NextCardCommand`

### 📦 Plik APK
- `Fiszki-v1.1-Flipcards-Release.apk` (69 MB)

---

## Wersja 1.0 - Release (19.02.2026)

### ✨ Funkcje
- ✅ Pełne CRUD dla fiszek (Create, Read, Update, Delete)
- ✅ 5 domyślnych kategorii: Ogólne, Czasowniki, Rzeczowniki, Przymiotniki, Zwroty
- ✅ Import/Export fiszek w formacie JSON
- ✅ Baza danych SQLite z automatyczną migracją
- ✅ Konfiguracja sesji nauki:
  - Wybór liczby fiszek (1-50)
  - Wybór kategorii
  - Tryb "tylko fiszki do powtórki"
  - Priorytet błędnych odpowiedzi
- ✅ System powtórek przestrzeniowych (Spaced Repetition):
  - 7 poziomów opanowania (0-6+)
  - Interwały: 1, 3, 7, 14, 30, 60, 120 dni
  - Błędne odpowiedzi → powtórka za 10 minut
- ✅ Statystyki nauki:
  - Całkowita liczba fiszek
  - Opanowane fiszki (≥80%)
  - Fiszki do powtórki
  - Studiowane dzisiaj
  - Łączna liczba powtórek
  - Średni wskaźnik sukcesu
- ✅ Wizualne wskaźniki fiszek do powtórki (pomarańczowa ramka)
- ✅ Filtrowanie i wyszukiwanie fiszek
- ✅ Swipe do usunięcia fiszek

### 🎨 UI/UX
- Material Design z nowoczesnym wyglądem
- Kolorowe ikony i wskaźniki
- Intuicyjna nawigacja
- Responsywny layout
- Polskie tłumaczenia

### 🔧 Techniczne
- .NET MAUI 10
- Android API 21+ (Android 5.0 Lollipop i nowsze)
- SQLite z Microsoft.Data.Sqlite
- MVVM pattern z CommunityToolkit.Mvvm
- Dependency Injection
- Repository pattern

### 📦 Pakiety
- Microsoft.Maui.Controls
- CommunityToolkit.Mvvm 8.3.2
- CommunityToolkit.Maui 12.3.0
- Microsoft.Data.Sqlite.Core 8.0.8
- SQLitePCLRaw.bundle_green 2.1.10
- Syncfusion.Maui.Toolkit 1.0.8

### 📝 Pliki w dystrybucji
- `Fiszki-v1.0-Release.apk` - Aplikacja Android (69 MB)
- `README_INSTALACJA.md` - Instrukcja instalacji
- `CHECKLIST_TESTOWY.md` - Checklist dla testerów
- `sample_flashcards.json` - 15 przykładowych fiszek
- `NOWE_FUNKCJE.md` - Dokumentacja funkcji
- `JAK_ZBUDOWAC_APK.md` - Instrukcja budowania

### ⚠️ Znane problemy
- Ostrzeżenia kompilacji o przestarzałym `Frame` (nie wpływa na działanie)
- Rozmiar APK ~69 MB (możliwa optymalizacja w przyszłości)

### 🔮 Planowane w następnych wersjach
- Możliwość wyboru konkretnych fiszek do nauki
- Eksport statystyk do pliku
- Tryb ciemny (Dark Mode)
- Dźwięki i animacje
- Wykresy postępów
- Synchronizacja w chmurze
- Udostępnianie zestawów fiszek
- Tłumaczenie na język angielski
- Obsługa obrazków w fiszkach
- Quiz wielokrotnego wyboru

---

## Wersja 0.9 - Beta (przed 19.02.2026)

### Podstawowe funkcje
- Dodawanie i zarządzanie fiszkami
- Kategorie
- Import z JSON
- Podstawowa nauka (10 losowych fiszek)

---

**Wersja budowy**: 1  
**Data wydania**: 19.02.2026  
**Rozmiar APK**: ~69 MB  
**Min. Android**: 5.0 (API 21)  
**Target Android**: Latest (API 35+)
