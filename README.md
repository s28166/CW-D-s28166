# CW-D-s28166
**Autor**: s28166
## Opis projektu
To repozytorium zawiera rozwiązanie zadania CW-D w ramach kursu APBD. Projekt obejmuje diagram ERD oraz implementację aplikacji w oparciu o podejście EF code first.​
## Przykładowe użycie endpointów:
- Utworzenie nowego wydarzenia
  - URL: `POST http://localhost:5046/event`
  ```json
  {
    "Title": "test2",
    "Description": "More of Something",
    "StartDate": "2025-07-10T11:00:00",
    "EndDate": "2025-07-10T15:30:00",
    "MaxParticipants": 11
  }
  ```
- Przypisanie prelegenta do wydarzenia:
  - URL: `POST http://localhost:5046/event/7/addSpeaker`
  ```json
  {
    "FirstName": "Jacek",
    "LastName": "Jackowki",
    "Email": "jacek.jackowski@example.pl"
  }
  ```
- Rejestracja uczestnika na wydarzenie:
  - URL: `POST http://localhost:5046/event/8/register`
  ```json
  {
    "FirstName": "Krystian",
    "LastName": "Jodła",
    "Email": "jacek.jackowski@example.pl"
  }
  ```
- Anulowanie rejestracji uczestnika:
  - URL: `PUT http://localhost:5046/event/8/cancel/4`
- Pobranie listy wydarzeń z informacją o liczbie wolnych miejsc:
  - URL: `GET http://localhost:5046/participant/3/report`
- Wygenerowanie raportu udziału uczestników:
  - URL: `GET http://localhost:5046/event/upcoming`
