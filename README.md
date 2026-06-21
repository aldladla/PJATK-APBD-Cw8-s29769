# Hospital Bed Assignment API

Proste API w ASP.NET Core do zarządzania pacjentami i przypisywania im łóżek w szpitalu.

Projekt powstał w ramach zajęć z backendu i baz danych. Głównym celem było przećwiczenie REST API, Entity Framework Core oraz pracy na relacyjnej bazie danych.

## Technologie

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server / LocalDB
* LINQ
* REST API

## Funkcje

Aplikacja pozwala na:

* pobranie listy pacjentów,
* wyszukiwanie pacjentów po imieniu lub nazwisku,
* wyświetlenie przyjęć pacjenta i przypisanych łóżek,
* przypisanie pacjentowi wolnego łóżka,
* sprawdzenie dostępności łóżka na podstawie oddziału, typu łóżka i zakresu dat.

## Przykładowe endpointy

```http
GET /api/patients
```

```http
GET /api/patients?search=Jan
```

```http
POST /api/patients/{pesel}/bedassignments
```

Przykładowe body:

```json
{
  "from": "2026-05-20T14:00:00",
  "to": "2026-05-30T10:00:00",
  "bedType": "Standard",
  "ward": "Kardiologia"
}
```

## Uruchomienie lokalnie

1. Sklonuj repozytorium:

```bash
git clone https://github.com/aldladla/PJATK-APBD-Cw8-s29769.git
cd PJATK-APBD-Cw8-s29769
```

2. Przywróć zależności:

```bash
dotnet restore
```

3. Ustaw connection string do bazy w `appsettings.json`.

4. Uruchom projekt:

```bash
dotnet run
```

API będzie dostępne lokalnie pod endpointami `/api/patients`.

## Czego się nauczyłem

W projekcie przećwiczyłem:

* tworzenie endpointów w ASP.NET Core,
* pracę z Entity Framework Core,
* mapowanie danych do DTO,
* obsługę relacji w bazie danych,
* filtrowanie danych z użyciem LINQ,
* przenoszenie logiki z kontrolera do serwisu.

## Możliwe usprawnienia

W przyszłości można dodać:

* walidację danych wejściowych,
* lepszą obsługę błędów,
* migracje lub skrypt SQL do łatwiejszego uruchomienia bazy,
* testy jednostkowe,
* konfigurację Dockera,
* dokumentację API w Swaggerze.
