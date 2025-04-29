# Instrukcja uruchomienia

## 1. Otwórz rozwiązanie projektu BookApi.sln i uruchom projekt.

## 2. Serwer uruchomi się na porcie 7187.

## 3. Można uzyskać poniższe zapytania API: https://localhost:7187/{api}

| Funkcja                            | Metoda | Endpoint             | Opis |
|------------------------------------|--------|----------------------|------|
| Pobierz listę książek              | GET    | `/api/books`         | Zwraca wszystkie książki z bazy |
| Pobierz książkę po ID              | GET    | `/api/books/{id}`    | Zwraca szczegóły książki o podanym ID |
| Dodaj nową książkę                 | POST   | `/api/books`         | Dodaje książkę na podstawie danych z żądania |
| Zaktualizuj istniejącą książkę     | PUT    | `/api/books/{id}`    | Edytuje książkę na podstawie danych z żądania |
| Usuń książkę                       | DELETE | `/api/books/{id}`    | Usuwa książkę o podanym ID z bazy |


