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

## Testowanie API z curl

### 1. Rejestracja nowego użytkownika
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!",
    "userName": "testuser"
  }'
```

### 2. Logowanie i otrzymanie tokenu JWT
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!"
  }'
```

Odpowiedź zawiera token JWT - skopiuj wartość pola `token`.

### 3. Utworzenie nowego projektu (wymaga tokenu)
```bash
curl -X POST http://localhost:5000/api/projekty \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT" \
  -d '{
    "nazwa": "Mój Pierwszy Projekt",
    "opis": "Opis projektu"
  }'
```

### 4. Pobranie wszystkich projektów użytkownika
```bash
curl -X GET http://localhost:5000/api/projekty \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT"
```

### 5. Utworzenie zadania w projekcie
```bash
curl -X POST http://localhost:5000/api/zadania \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT" \
  -d '{
    "tytul": "Moje pierwsze zadanie",
    "opis": "Opis zadania",
    "status": "Do zrobienia",
    "projektId": 1
  }'
```

### 6. Pobranie wszystkich zadań użytkownika
```bash
curl -X GET http://localhost:5000/api/zadania \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT"
```

### 7. Aktualizacja projektu
```bash
curl -X PUT http://localhost:5000/api/projekty/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT" \
  -d '{
    "nazwa": "Zaktualizowana nazwa",
    "opis": "Nowy opis projektu"
  }'
```

### 8. Usunięcie zadania
```bash
curl -X DELETE http://localhost:5000/api/zadania/1 \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT"
```

**Uwaga:** Zastąp `TWOJ_TOKEN_JWT` tokenem otrzymanym po zalogowaniu.

## Swagger

Dokumentacja API dostępna pod adresem: `http://localhost:5000/` lub `http://localhost:5000/swagger`

Swagger UI umożliwia interaktywne testowanie wszystkich endpointów bez używania curl.

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