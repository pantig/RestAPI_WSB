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

### Wymagania
- .NET 8 SDK
- Entity Framework Core Tools: `dotnet tool install --global dotnet-ef`

### Kroki instalacji

1. **Sklonuj repozytorium:**
   ```bash
   git clone https://github.com/pantig/RestAPI_WSB.git
   cd RestAPI_WSB/RestAPI_WSB
   ```

2. **Wygeneruj bezpieczny klucz JWT** (minimum 32 znaki) i zamień wartość `Jwt:Key` w `appsettings.json`:
   
   **Linux/macOS:**
   ```bash
   openssl rand -base64 48
   ```
   
   **Windows PowerShell:**
   ```powershell
   -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 64 | ForEach-Object {[char]$_})
   ```

3. **Utwórz inicjalną migrację** (tylko raz, przy pierwszym uruchomieniu):
   ```bash
   dotnet ef migrations add InitialCreate
   ```

4. **Uruchom aplikację:**
   ```bash
   dotnet run
   ```
   
   ⚠️ **Baza danych zostanie automatycznie utworzona przy pierwszym uruchomieniu!**

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

Projekt używa **SQLite** jako bazy danych. Plik bazy danych (`RestAPI_WSB.db`) jest tworzony **automatycznie przy pierwszym uruchomieniu aplikacji** i znajduje się w katalogu projektu `RestAPI_WSB/`.

### Automatyczne Migracje

Aplikacja automatycznie:
- Tworzy bazę danych przy pierwszym uruchomieniu
- Aplikuje wszystkie oczekujące migracje
- Loguje proces migracji w konsoli

Nie musisz ręcznie uruchamiać `dotnet ef database update` - wszystko dzieje się automatycznie!