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
- W ustawieniach rozwiązania "Warehouse Manager" w sekcji "Projekt startowy" wybrać opcję "Wiele projektów startowych" i przestawić akcję obu projektów (WarehouseAPI i WarehouseApp) na "Uruchomienie"
- Nacisnąć klawisz F5 (oba projekty ustawione jako startowe, co powoduje start strony serwowanej przez Swagger-a z opisem API jak i aplikacji front-endowej)

***Uwaga:*** _Baza danych wykorzystywana przez serwer API jest kopiowana do folderu wyjściowego i to z niej korzysta uruchomiona aplikacja. Baza zawiera utworzoną strukturę tabel (encji) z zależnościami
oraz kilka przykładowych rekordów dokumentów i innych danychm które będą widoczne w aplikacji Fron-Endowej w liście kontrahentów, towarów (artykułów) jak i dokumentów_

## Użytkownicy

Aplikacja posiada dwie role dla użytkownika: User oraz Admin

W bazie znajdują się predefiniowania następujący użytkownicy:
- Email: hsu@hs.pl | Hasło: 123456 | Rola: User
- Email: hsa@hs.pl | Hasło: 123456 | Rola: Admin

## Schemat Bazy danych

Diagram bazy danych przedstawia poniższy rysunek.

![Diagram bazy danych Warehouse Man](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/doc/warehouse-man-db-diag.jpg)
