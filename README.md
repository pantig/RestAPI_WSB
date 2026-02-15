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
- SQLite

## Instalacja

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/pantig/RestAPI_WSB.git
   cd RestAPI_WSB
   ```

2. Wygeneruj bezpieczny klucz JWT (minimum 32 znaki) i zamień wartość `Jwt:Key` w `appsettings.json`:
   ```powershell
   # W PowerShell:
   -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 64 | ForEach-Object {[char]$_})
   ```

3. Przejdź do katalogu projektu:
   ```bash
   cd RestAPI_WSB
   ```

4. Przywróć pakiety NuGet:
   ```bash
   dotnet restore
   ```

5. Utwórz migracje bazy danych:
   ```bash
   dotnet ef migrations add InitialCreate
   ```

6. Zaktualizuj bazę danych (utworzy plik `RestAPI_WSB.db`):
   ```bash
   dotnet ef database update
   ```

7. Uruchom aplikację:
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

Dokumentacja API dostępna pod adresem: `http://localhost:5000/swagger` (lub inny port wskazany podczas uruchomienia)

## Baza Danych

Projekt używa SQLite jako bazy danych. Plik bazy danych (`RestAPI_WSB.db`) jest tworzony automatycznie po uruchomieniu migracji i znajduje się w katalogu projektu `RestAPI_WSB/`.