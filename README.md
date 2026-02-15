# RestAPI_WSB

REST API oparte na .NET 8 z ASP.NET Core, wykorzystujace EntityFramework i Microsoft Identity.

## Funkcjonalnosci

- Uwierzytelnianie i autoryzacja uzytkownikow z wykorzystaniem tokenow JWT
- Modele danych: User (Microsoft Identity), Projekt, Zadanie
- Relacje:
  - Jeden uzytkownik moze posiadac wiele projektow
  - Projekt moze miec wiele zadan
  - Zadanie moze byc przypisane do jednego uzytkownika
- Endpointy CRUD dla Projektow i Zadan
- Zabezpieczenie dostepu tokenem JWT
- Uzytkownik ma dostep do:
  - Projektow, ktorych jest wlascicielem
  - Projektow, w ktorych ma przypisane zadania

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

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/pantig/RestAPI_WSB.git
   cd RestAPI_WSB/RestAPI_WSB
   ```

2. Wygeneruj bezpieczny klucz JWT (minimum 32 znaki) i zamien wartosc Jwt:Key w pliku appsettings.json:
   
   Linux/macOS:
   ```bash
   openssl rand -base64 48
   ```
   
   Windows PowerShell:
   ```powershell
   -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 64 | ForEach-Object {[char]$_})
   ```
   
   Skopiuj wygenerowany klucz i wklej do appsettings.json jako wartosc Jwt:Key.

3. Uruchom aplikacje:
   ```bash
   dotnet run
   ```
   
## Endpointy API

### Autoryzacja
- POST /api/auth/register - Rejestracja nowego uzytkownika
- POST /api/auth/login - Logowanie i otrzymanie tokenu JWT

### Projekty
- GET /api/projekty - Pobierz wszystkie projekty uzytkownika
- GET /api/projekty/{id} - Pobierz szczegoly projektu
- POST /api/projekty - Utworz nowy projekt
- PUT /api/projekty/{id} - Zaktualizuj projekt
- DELETE /api/projekty/{id} - Usun projekt

### Zadania
- GET /api/zadania - Pobierz wszystkie zadania uzytkownika
- GET /api/zadania/{id} - Pobierz szczegoly zadania
- POST /api/zadania - Utworz nowe zadanie
- PUT /api/zadania/{id} - Zaktualizuj zadanie
- DELETE /api/zadania/{id} - Usun zadanie

## Testowanie API z curl

### 1. Rejestracja nowego uzytkownika
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

Odpowiedz zawiera token JWT - skopiuj wartosc pola token.

### 3. Utworzenie nowego projektu (wymaga tokenu)
```bash
curl -X POST http://localhost:5000/api/projekty \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT" \
  -d '{
    "nazwa": "Moj Pierwszy Projekt",
    "opis": "Opis projektu"
  }'
```

### 4. Pobranie wszystkich projektow uzytkownika
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

### 6. Pobranie wszystkich zadan uzytkownika
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

### 8. Usuniecie zadania
```bash
curl -X DELETE http://localhost:5000/api/zadania/1 \
  -H "Authorization: Bearer TWOJ_TOKEN_JWT"
```

Uwaga: Zastap TWOJ_TOKEN_JWT tokenem otrzymanym po zalogowaniu.

## Swagger

Dokumentacja API dostepna pod adresem: http://localhost:5000/

Swagger UI umozliwia interaktywne testowanie wszystkich endpointow bez uzywania curl.

## Baza Danych

Projekt uzywa SQLite jako bazy danych. 
