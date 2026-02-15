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

## Instalacja i Uruchomienie

### Wymagania
- .NET 8 SDK

### Kroki

1. **Sklonuj repozytorium:**
   ```bash
   git clone https://github.com/pantig/RestAPI_WSB.git
   cd RestAPI_WSB/RestAPI_WSB
   ```

2. **Wygeneruj bezpieczny klucz JWT** (minimum 32 znaki) i zamień wartość `Jwt:Key` w pliku `appsettings.json`:
   
   **Linux/macOS:**
   ```bash
   openssl rand -base64 48
   ```
   
   **Windows PowerShell:**
   ```powershell
   -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 64 | ForEach-Object {[char]$_})
   ```
   
   Skopiuj wygenerowany klucz i wklej do `appsettings.json` jako wartość `Jwt:Key`.

3. **Uruchom aplikację:**
   ```bash
   dotnet run
   ```
   
   ✅ **To wszystko!** Baza danych i wszystkie tabele zostaną automatycznie utworzone przy pierwszym uruchomieniu.

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

Dokumentacja API dostępna pod adresem: `http://localhost:5000/` lub `http://localhost:5000/swagger`

## Baza Danych

Projekt używa **SQLite** jako bazy danych. 

### Automatyczne Tworzenie Bazy

Przy pierwszym uruchomieniu aplikacji (`dotnet run`):
- Automatycznie tworzy plik bazy danych `RestAPI_WSB.db` w katalogu projektu
- Tworzy wszystkie niezbędne tabele (Users, Roles, Projekty, Zadania)
- Konfiguruje relacje między tabelami

**Nie musisz:**
- Instalować `dotnet-ef` tools
- Tworzyć migracji ręcznie
- Uruchamiać `dotnet ef database update`

Wszystko dzieje się automatycznie! Po prostu uruchom `dotnet run` i zacznij używać API.