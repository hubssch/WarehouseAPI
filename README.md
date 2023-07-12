# WarehouseMan

Projekt przygotowany na przedmiot: **Programowanie Aplikacji Back Endowych**.
Do jego realizacja wykorzystano pomysł, jak i część kodu z innego projektu [Magazynier](https://github.com/hubssch/Magazynier) (nie wykononano jednak forka wymienionego projektu).
Backend aplikacji stanowi interfejs API serwowany za pomocą mechanizmów OpenAPI, z wykorzystanie prostej bazy danych SQLite.
Aplikację frontendową stanowi zwykła aplikacja standalone, której GUI definiuje język XAML. Zrezygnowano z technologii webowych.
Obie aplikacje oparte na platformie .NET 6.0

## Wykorzystane technologie:
- .NET 6.0
- C#
- JWT
- Swagger
- SQLite
- XAML

## Uruchomienie projektu
- Pobrać repozytorium,
- Otworzyć solucję w Visual Studio
- Nacisnąć klawisz F5 (oba projekty ustawione jako startowe, co powoduje start strony serwowanej przez Swagger-a z opisem API jak i aplikacji front-endowej)

## Użytkownicy

Aplikacja posiada dwie role dla użytkownika: User oraz Admin

W bazie znajdują się predefiniowania następujący użytkownicy:
- Email: hsu@hs.pl | Hasło: 123456 | Rola: User
- Email: hsa@hs.pl | Hasło: 123456 | Rola: Admin