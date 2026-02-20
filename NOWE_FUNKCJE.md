# Nowe funkcje w aplikacji Fiszki

## 1. Tryb nauki z odwracaniem kart 🎴

### Jak działa:
- **Krok 1**: Pokazuje się karta z angielskim słowem i przykładem użycia
- **Krok 2**: Kliknij na kartę aby ją odwrócić i zobaczyć polskie tłumaczenie
- **Krok 3**: Oceń samodzielnie czy znałeś to słowo:
  - ✅ **Znałem** - fiszka przejdzie na wyższy poziom powtórek
  - ❌ **Nie znałem** - fiszka wróci do powtórki za 10 minut

### Zalety tego trybu:
- Szybsza nauka (nie trzeba wpisywać odpowiedzi)
- Koncentracja na zapamiętywaniu, nie pisowni
- Wizualne zapamiętywanie słów
- Naturalne tempo nauki (sam decydujesz czy znałeś słowo)

## 2. Konfiguracja sesji nauki (LearningConfigPage)

### Możliwości:
- **Wybór kategorii** - nauka z konkretnej kategorii lub wszystkich kategorii
- **Liczba fiszek** - możliwość wyboru od 1 do 50 fiszek (krok co 5)
- **Tylko fiszki do powtórki** - opcja nauki tylko tych fiszek, które są zaplanowane do powtórki
- **Priorytet błędnych odpowiedzi** - system priorytetyzuje fiszki z niskim wskaźnikiem skuteczności
- **Statystyki na żywo** - wyświetlanie liczby dostępnych fiszek i fiszek do powtórki

### Nawigacja:
- Z głównej listy -> przycisk "Zacznij naukę" -> Konfiguracja sesji -> Nauka z odwracaniem kart

## 3. System powtórek przestrzeniowych (Spaced Repetition)

### Algorytm:
- Poprawna odpowiedź: fiszka planowana do kolejnej powtórki według schematu:
  - Poziom 0: +1 dzień
  - Poziom 1: +3 dni
  - Poziom 2: +7 dni
  - Poziom 3: +14 dni
  - Poziom 4: +30 dni
  - Poziom 5: +60 dni
  - Poziom 6+: +120 dni

- Błędna odpowiedź: 
  - Fiszka wraca do powtórki za 10 minut
  - Poziom powtórek zmniejsza się o 1 (minimum 0)

### Pola w bazie danych:
- **NextReview** (DateTime) - data następnej zaplanowanej powtórki
- **RepetitionLevel** (int) - aktualny poziom opanowania fiszki (0-6+)

### Wizualne wskaźniki:
- Fiszki wymagające powtórki mają **pomarańczową ramkę** (#FF5722)
- Ikona 🔄 i tekst "Do powtorki" na liście fiszek

## 3. Statystyki nauki (StatisticsPage)

### Wyświetlane wskaźniki:
1. **Wszystkich fiszek** - całkowita liczba fiszek w bazie
2. **Opanowanych fiszek** - fiszki z ≥80% skutecznością
3. **Do powtórki** - liczba fiszek zaplanowanych do powtórki dzisiaj
4. **Studiowano dzisiaj** - ile fiszek było przeglądanych dzisiaj
5. **Łączna liczba powtórek** - suma wszystkich sesji nauki
6. **Średni wskaźnik sukcesu** - średnia skuteczność ze wszystkich fiszek

### Nawigacja:
- Z głównej listy -> przycisk "Statystyki"

## 4. Ulepszona strona nauki (LearnPage)

### Nowy interfejs z odwracaniem kart:
- Wizualna karta z flagami 🇬🇧 / 🇵🇱
- Angielskie słowo i przykład na przedniej stronie
- Polskie tłumaczenie na tylnej stronie
- Przyciski samooceny: "Znałem" / "Nie znałem"
- Natychmiastowa aktualizacja NextReview i RepetitionLevel
- Liczniki poprawnych i błędnych odpowiedzi

### Przebieg sesji:
1. Pokazuje się karta z angielskim słowem
2. Spróbuj sobie przypomnieć tłumaczenie
3. Kliknij na kartę aby sprawdzić odpowiedź
4. Oceń czy znałeś słowo (Znałem/Nie znałem)
5. Automatyczne przejście do następnej karty

### Po zakończeniu sesji:
- Przycisk "Jeszcze raz" - powtarza sesję z tymi samymi ustawieniami
- Przycisk "Zakończ" - powrót do głównej listy
- Podsumowanie: ile znałeś, ile nie znałeś

## 5. Ulepszona lista fiszek (FlashcardListPage)

### Wizualne zmiany:
- Kolorowa ramka dla fiszek do powtórki (pomarańczowa vs szara)
- Dodatkowy label "🔄 Do powtorki" dla fiszek wymagających powtórki
- Nowy przycisk "Statystyki" na dolnym pasku

### Nowa nawigacja:
- "Zacznij naukę" -> przechodzi do konfiguracji sesji zamiast bezpośrednio do nauki
- "Statystyki" -> otwiera stronę ze statystykami

## 6. Rozszerzone repozytorium (FlashcardRepository)

### Nowe metody:
- **GetFlashcardsForLearningAsync(config)** - pobiera fiszki według konfiguracji:
  - Filtrowanie po kategorii
  - Filtrowanie tylko fiszek do powtórki
  - Sortowanie z priorytetem błędnych odpowiedzi
  - Wybór konkretnych fiszek po ID
  
- **CalculateNextReview(wasCorrect, currentLevel)** - oblicza datę następnej powtórki

- **GetStatisticsAsync()** - zwraca kompletne statystyki nauki

### Migracja bazy danych:
- Automatyczne dodawanie kolumn NextReview i RepetitionLevel do istniejących baz
- Kompatybilność wsteczna z pomocą metody ReadFlashcard()

## 7. Import z JSON

### Ulepszenia:
- Inicjalizacja NextReview = DateTime.Now dla nowo importowanych fiszek
- Zapewnia, że wszystkie fiszki są od razu dostępne do nauki

## Przykładowe użycie:

1. **Szybka nauka 10 fiszek z odwracaniem kart:**
   - Główna lista -> "Zacznij naukę" -> domyślne ustawienia -> "Rozpocznij naukę"
   - Przeczytaj angielskie słowo
   - Kliknij kartę aby zobaczyć tłumaczenie
   - Wybierz czy znałeś słowo

2. **Powtórka tylko czasowników:**
   - Główna lista -> "Zacznij naukę" -> wybierz "Czasowniki" -> "Rozpocznij naukę"
   - Odwracaj karty i ucz się

3. **Tylko fiszki wymagające powtórki:**
   - Główna lista -> "Zacznij naukę" -> zaznacz "Tylko fiszki do powtórki" -> "Rozpocznij naukę"
   - Powtarzaj zapomniane słowa

4. **Sprawdzenie postępów:**
   - Główna lista -> "Statystyki"

## Plik testowy:
Użyj `sample_flashcards.json` do szybkiego importu 15 przykładowych fiszek w różnych kategoriach.
