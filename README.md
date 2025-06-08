# Projekt - System zarządzania warsztatem samochodowym

## Autorzy:
- Bartłomiej Handziak
- Kacper Dziduch

# Wstęp

Projekt zostatał napisany w oparciu o: 
- ASP.NET Core (Backend) (branch api/development)
- React.js (Frontend) (branch feature/client)
- MS SQL (Baza danych)

I spełnia wszystkie wymagania User Stories.
Dodatkowo zaimplementowaliśmy:

- Indeksy – optymalizacja zapytań
- Sprawdziliśmy przykładowy endpoint (SQL Profiler)
- GitHub Actions – CI/CD (na branchu ci/github-actions) - składa się z 3 komend: dotnet restore (pobiera pakiety NuGet), dotnet build (kompiluje kod zródłowy), dotnet run (uruchamia testy jednostkowe)
- Logowanie błędów – NLog
- NBomber - testy wydajnościowe (branch api/nbomber)

## Frontend

Polecenie uruchamiające aplikację: ```npm run serve```.
Aplikacja React.js uruchamia się na porcie 3010. Jest napisana w oparciu o technologie:
- biblioteka React.js
- Webpack
- axios - obsługuje zapytania HTTP
- HTML
- SCSS
- Local Storage - do przechowania danych o zalogowanym użytkowniku
- Session Cookie - sesja użytkownika
- komponenty Auth - do zarządzania rolami

## Nawigacja po aplikacji

- US1 
    - panel logowania (strona publiczna)
    - panel rejestracji (strona publiczna) - dostępny pod przyciskiem ``` create an account ```
- US2 - admin
    - widok użytkowników w ```Set Role```
    - zmiana ról użytkowników
- US3 - receptionist, admin
    - dodawanie klienta ```Customer``` -> ```Add Customer```
    - CRUD klienta na podstronie ```Customer```
- US4 - receptionist, admin
    - dodanie pojazdu do klienta ```Customer``` -> ```Details``` -> ```Add vehicle```
- US5 - wszystkie role
    - dodanie zdjęcia do pojazdu ```Customer``` -> ```Details``` -> ```Przycisk do wysyłania zdjęcia```
- US6 - receptionist, admin
    - dodanie nowego zlecenia dla pojazdu ```Service Order``` -> ```Wybór potencjalnego zlecenia``` -> ```Add Order```
- US7 - mechanic
    - dodanie czynności serwisowych dla zlecenia ```Service Order```-> ```My Services``` -> ```Details``` -> ```Add Service Task```
- US8 - mechanic
    - dodanie części do czynności serwisowych ```Service Order```-> ```My Services``` -> ```Details``` ->  ```Wybór czyności``` -> ```Add Part```
- US9 - mechanic, admin
    - zmiana statusu zlecenia ```Service Order```-> ```My Services``` -> ```Details``` -> ```Complete/ Cancel Order```
- US10 - wszystkie role
    - komentowanie zlecenia ```Service Order```-> ```Comments``` -> ```Send```
- US11 - receptionist, admin
    - dodawanie nowej cześci ```Parts``` -> ```Add Part```
    - CRUD Part w ```Parts```
- US12 - receptionist, admin
    - raport kosztów napraw klienta ```Customer``` -> ```Details``` -> ```Raport```
- US13 - admin
    - raport PDF napraw wykonanych w miesiącu ```Raport```  -> ```See raport```