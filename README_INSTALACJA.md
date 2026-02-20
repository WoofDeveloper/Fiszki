# Fiszki - Nauka Angielskiego v1.0 📚

Aplikacja mobilna Android do nauki języka angielskiego z wykorzystaniem fiszek i systemu powtórek przestrzeniowych.

## 📱 Instalacja na Androidzie

### Plik do pobrania
**Nazwa pliku**: `Fiszki-v1.0-Release.apk`  
**Rozmiar**: ~69 MB  
**Wymagany Android**: 5.0 (Lollipop) lub nowszy

### Kroki instalacji:

1. **Pobierz plik APK** na swoje urządzenie Android
   - Możesz pobrać przez email, Google Drive, lub bezpośrednio na telefon

2. **Zezwól na instalację z nieznanych źródeł** (jeśli system zapyta):
   - Otwórz: Ustawienia → Bezpieczeństwo → Nieznane źródła
   - Lub: Ustawienia → Aplikacje → Dostęp specjalny → Instaluj nieznane aplikacje
   - Włącz dla przeglądarki/menedżera plików, którego używasz

3. **Zainstaluj aplikację**:
   - Otwórz pobrany plik APK
   - Kliknij "Zainstaluj"
   - Poczekaj na zakończenie instalacji
   - Kliknij "Otwórz"

4. **Gotowe!** Aplikacja jest gotowa do użycia 🎉

---

## 🎯 Funkcje aplikacji

### ✅ Odwracanie kart (Flashcards)
- **Wizualne odwracanie kart** - kliknij na kartę aby zobaczyć tłumaczenie
- Najpierw angielskie słowo 🇬🇧, po odwróceniu polskie 🇵🇱
- Samoocena: "Znałem" lub "Nie znałem"
- Szybka i efektywna nauka bez potrzeby wpisywania
- Przykłady użycia słów na kartach

### ✅ Zarządzanie fiszkami
- Dodawanie własnych fiszek (słowo angielskie + polskie tłumaczenie + przykład użycia)
- Edycja i usuwanie fiszek
- Kategorie: Ogólne, Czasowniki, Rzeczowniki, Przymiotniki, Zwroty
- Import fiszek z pliku JSON (szybkie dodawanie wielu fiszek na raz)

### 📖 Nauka z konfiguracją
- Wybór liczby fiszek do nauki (1-50)
- Wybór konkretnej kategorii lub wszystkich kategorii
- Tryb "tylko fiszki do powtórki"
- Priorytet błędnych odpowiedzi (system uczy trudniejszych fiszek częściej)

### 🔄 System powtórek przestrzeniowych (Spaced Repetition)
- Inteligentny algorytm planuje powtórki
- Poprawne odpowiedzi → kolejna powtórka za: 1, 3, 7, 14, 30, 60, 120 dni
- Błędne odpowiedzi → powtórka za 10 minut
- Wizualne oznaczenie fiszek wymagających powtórki (pomarańczowa ramka 🔄)

### 📊 Statystyki nauki
- Całkowita liczba fiszek
- Opanowane fiszki (≥80% skuteczności)
- Liczba fiszek do powtórki dzisiaj
- Fiszki studiowane dzisiaj
- Łączna liczba wszystkich powtórek
- Średni wskaźnik sukcesu

### 💾 Baza danych SQLite
- Wszystkie dane przechowywane lokalnie na urządzeniu
- Brak potrzeby połączenia z internetem
- Automatyczna synchronizacja i backup

---

## 🚀 Jak korzystać z aplikacji

### Pierwsze uruchomienie:

1. **Dodaj kilka fiszek**:
   - Kliknij "Dodaj fiszkę"
   - Wpisz słowo angielskie, polskie tłumaczenie i przykład
   - Wybierz kategorię
   - Kliknij "Zapisz"

2. **Lub zaimportuj z JSON**:
   - Kliknij "Import z JSON"
   - Wklej JSON (przykład w pliku `sample_flashcards.json`)
   - Kliknij "Importuj"

### Nauka:

1. **Zacznij naukę**:
   - Kliknij "Zacznij naukę" na głównym ekranie
   - Skonfiguruj sesję (liczba fiszek, kategoria, opcje)
   - Kliknij "Rozpocznij naukę"

2. **Odwracaj karty**:
   - Przeczytaj angielskie słowo na karcie
   - Spróbuj sobie przypomnieć tłumaczenie
   - **Kliknij na kartę** aby ją odwrócić i zobaczyć odpowiedź
   - Wybierz: ✅ "Znałem" lub ❌ "Nie znałem"
   - Karta automatycznie przejdzie do następnej

3. **Po zakończeniu**:
   - Zobacz swoje wyniki (ile znałeś, ile nie)
   - Kliknij "Jeszcze raz" lub "Zakończ"

### Sprawdzanie postępów:

1. **Otwórz statystyki**:
   - Kliknij "Statystyki" na głównym ekranie
   - Zobacz wszystkie wskaźniki nauki
   - Kliknij "Odśwież" aby zaktualizować

---

## 📝 Przykładowy format JSON do importu

```json
{
  "flashcards": [
    {
      "englishWord": "hello",
      "polishTranslation": "cześć",
      "example": "Hello, how are you?",
      "category": "Ogólne"
    },
    {
      "englishWord": "run",
      "polishTranslation": "biegać",
      "example": "I run every morning.",
      "category": "Czasowniki"
    }
  ]
}
```

Pełny przykład znajduje się w pliku `sample_flashcards.json` (15 gotowych fiszek).

---

## 🐛 Zgłaszanie problemów

Jeśli napotkasz jakieś problemy lub błędy:

1. Zrób screenshot ekranu z błędem
2. Opisz co robiłeś przed wystąpieniem błędu
3. Podaj wersję Androida (Ustawienia → O telefonie)
4. Wyślij informacje do dewelopera

---

## 📌 Wskazówki dla testerów

### Co przetestować:
- [ ] Dodawanie nowych fiszek
- [ ] Edycja i usuwanie fiszek
- [ ] Import z JSON (użyj przykładowego pliku)
- [ ] Wszystkie kategorie
- [ ] Sesję nauki z różnymi konfiguracjami
- [ ] Czy system powtórek działa (fiszki wracają do powtórki)
- [ ] Statystyki po kilku sesjach nauki
- [ ] Czy dane są zapisywane po zamknięciu aplikacji
- [ ] Czy aplikacja działa płynnie i szybko

### Na co zwrócić uwagę:
- Błędy lub crashe aplikacji
- Problemy z interfejsem (nieczytelny tekst, źle wyświetlające się elementy)
- Problemy z wydajnością (długie ładowanie, zawieszanie się)
- Błędne działanie funkcji
- Sugestie i pomysły na ulepszenia

---

## 🔧 Informacje techniczne

- **Framework**: .NET MAUI 10
- **Platforma**: Android (API 21+)
- **Baza danych**: SQLite
- **Rozmiar APK**: ~69 MB
- **Język**: Polski
- **Wersja**: 1.0 (build 1)
- **Package ID**: com.fiszki.english

---

## 📞 Kontakt

W razie pytań lub problemów, skontaktuj się z deweloperem.

**Miłej nauki! 📚🎓**
