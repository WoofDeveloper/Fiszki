# ✅ Checklist testowy - Fiszki v1.0

## Podstawowe funkcje

### Zarządzanie fiszkami
- [ ] Dodanie nowej fiszki (słowo, tłumaczenie, przykład, kategoria)
- [ ] Edycja istniejącej fiszki (kliknij na fiszkę)
- [ ] Usunięcie fiszki (przesuń palcem w lewo → Usuń)
- [ ] Filtrowanie po kategorii
- [ ] Wyszukiwanie fiszek (pole search)

### Import/Export
- [ ] Import fiszek z JSON (skopiuj zawartość z `sample_flashcards.json`)
- [ ] Sprawdź czy importuje wszystkie 15 fiszek poprawnie
- [ ] Sprawdź czy kategorie są przypisane automatycznie

### Konfiguracja sesji nauki
- [ ] Otwórz "Zacznij naukę"
- [ ] Zmień liczbę fiszek (Stepper 1-50)
- [ ] Wybierz konkretną kategorię
- [ ] Zaznacz "Tylko fiszki do powtórki"
- [ ] Zaznacz "Priorytet błędnych odpowiedzi"
- [ ] Sprawdź statystyki (Dostępne fiszki, Do powtórki)

### Sesja nauki (Odwracanie kart)
- [ ] Rozpocznij sesję z 5 fiszkami
- [ ] Przeczytaj angielskie słowo na karcie
- [ ] Kliknij na kartę - sprawdź czy pokazuje polskie tłumaczenie
- [ ] Kliknij ponownie - sprawdź czy wraca do angielskiego (odwrócenie)
- [ ] Kliknij "Znałem" - sprawdź czy przechodzi do następnej karty
- [ ] Na kolejnej fiszce kliknij "Nie znałem"
- [ ] Sprawdź czy liczniki (Poprawne/Błędne) się aktualizują
- [ ] Dokończ sesję
- [ ] Sprawdź wyniki końcowe
- [ ] Kliknij "Jeszcze raz" → sprawdź czy sesja się restartuje
- [ ] Kliknij "Zakończ" → sprawdź czy wraca do listy

### System powtórek
- [ ] Dodaj nową fiszkę
- [ ] Naucz się jej (odpowiedz poprawnie)
- [ ] Wróć do listy głównej
- [ ] Sprawdź czy fiszka NIE ma pomarańczowej ramki (bo następna powtórka za 1 dzień)
- [ ] Odpowiedz błędnie na jakąś fiszkę
- [ ] Sprawdź czy po 10 minutach ma pomarańczową ramkę (🔄 Do powtórki)

### Statystyki
- [ ] Otwórz "Statystyki"
- [ ] Sprawdź czy "Wszystkich fiszek" = liczba dodanych fiszek
- [ ] Sprawdź "Studiowano dzisiaj" (powinno być > 0 po nauce)
- [ ] Sprawdź "Łączna liczba powtórek"
- [ ] Sprawdź "Średni wskaźnik sukcesu" (procent)
- [ ] Kliknij "Odśwież" → sprawdź czy aktualizuje dane

## Testy wydajnościowe

- [ ] Dodaj 50+ fiszek → sprawdź czy lista przewija się płynnie
- [ ] Import 100 fiszek z JSON → sprawdź czy import trwa < 5 sekund
- [ ] Sesja z 50 fiszkami → sprawdź czy działa bez zawieszania
- [ ] Zamknij i otwórz aplikację → sprawdź czy dane są zachowane

## Testy UI/UX

- [ ] Wszystkie przyciski działają
- [ ] Tekst jest czytelny (nie za mały/duży)
- [ ] Kolory są dobrze widoczne
- [ ] Nie ma literówek w interfejsie
- [ ] Responsywność na różnych rozmiarach ekranu
- [ ] Orientacja pionowa działa dobrze
- [ ] Orientacja pozioma działa dobrze

## Testy edge case'ów

- [ ] Dodaj fiszkę bez przykładu (Example) → powinno działać
- [ ] Dodaj fiszkę bez kategorii → powinno działać
- [ ] Próba nauki gdy brak fiszek → powinien pokazać alert
- [ ] Import pustego JSON → powinien pokazać błąd
- [ ] Import JSON z błędnym formatem → powinien pokazać szczegółowy błąd
- [ ] Bardzo długie słowo (100+ znaków) → sprawdź czy się wyświetla
- [ ] Specjalne znaki w słowach (ąćęłńóśźż, @#$%) → sprawdź obsługę

## Problemy do zgłoszenia

### Format zgłoszenia:
**Co się stało**: (opis problemu)  
**Kroki do odtworzenia**: (jak wywołać błąd)  
**Oczekiwane**: (jak powinno działać)  
**Aktualne**: (jak działa teraz)  
**Screenshot**: (jeśli możliwe)  
**Urządzenie**: (model telefonu, wersja Androida)

---

## Sugestie i pomysły

Zapisz tutaj swoje pomysły na ulepszenia:
- 
- 
- 

---

**Data testów**: _______________  
**Tester**: _______________  
**Wersja aplikacji**: 1.0  
**Urządzenie**: _______________  
**Android**: _______________
