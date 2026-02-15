# RestAPI_WSB

REST API oparte na .NET 8 z ASP.NET Core, wykorzystujące EntityFramework i Microsoft Identity.

## Funkcjonalności

- Uwierzytelnianie i autoryzacja użytkowników z wykorzystaniem tokenów JWT
- Modele danych: User (Microsoft Identity), Projekt, Zadanie
- Relacje:
  - Jeden użytkownik może posiadać wiele projektów
  - Projekt może mieć wiele zadań
  - Zadanie może być przypisane do jednego użytkownika
- Endpointy CRUD dla Projektów i Zadań
- Zabezpieczenie dostępu tokenem JWT
- Użytkownik ma dostęp do:
  - Projektów, których jest właścicielem
  - Projektów, w których ma przypisane zadania

## Technologie

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Microsoft Identity
- JWT Authentication
- SQL Server (LocalDB)

## Instalacja

1. Sklonuj repozytorium
2. Zaktualizuj connection string w `appsettings.json`
3. Wygeneruj bezpieczny klucz JWT i zamień wartość `Jwt:Key` w `appsettings.json`
4. Uruchom migracje:
   ```bash
   dotnet ef database update
   ```
5. Uruchom aplikację:
   ```bash
   dotnet run
   ```

## Endpointy API

### Autoryzacja
- `POST /api/auth/register` - Rejestracja nowego użytkownika
- `POST /api/auth/login` - Logowanie i otrzymanie tokenu JWT

### Projekty
- `GET /api/projekty` - Pobierz wszystkie projekty użytkownika
- `GET /api/projekty/{id}` - Pobierz szczegóły projektu
- `POST /api/projekty` - Utwórz nowy projekt
- `PUT /api/projekty/{id}` - Zaktualizuj projekt
- `DELETE /api/projekty/{id}` - Usuń projekt

### Zadania
- `GET /api/zadania` - Pobierz wszystkie zadania użytkownika
- `GET /api/zadania/{id}` - Pobierz szczegóły zadania
- `POST /api/zadania` - Utwórz nowe zadanie
- `PUT /api/zadania/{id}` - Zaktualizuj zadanie
- `DELETE /api/zadania/{id}` - Usuń zadanie

## Swagger

Dokumentacja API dostępna pod adresem: `http://localhost:5000/swagger`