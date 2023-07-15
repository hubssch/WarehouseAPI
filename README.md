# WarehouseMan

Projekt przygotowany na przedmiot: **Programowanie Aplikacji Back Endowych**.
Do jego realizacja wykorzystano pomysł, jak i część kodu z innego projektu [Magazynier](https://github.com/hubssch/Magazynier) (nie wykononano jednak forka wymienionego projektu).
Backend aplikacji stanowi interfejs API serwowany za pomocą mechanizmów OpenAPI, z wykorzystanie prostej bazy danych SQLite.
Aplikację frontendową stanowi zwykła aplikacja standalone, której GUI definiuje język XAML. Zrezygnowano z technologii webowych.
Obie aplikacje oparte na platformie .NET 6.0.

Autor zaznacza, że nie skupiał się na optymalizacji kodu, a implementacji REST API wybranego tematu oraz istocie komunikacji między dwoma aplikacjami (serwisami).
Aplikacja frontendowa wykonuje requesty do serwera API poprzez protokół HTTPS. W zależności od wysyłanych typów zapytań HTTP (GET, POST, PUT, DELETE) wykonywane są różne
działania na bazie danych w aplikacji (serwerze) backendowym. Aby możliwa była wymiana danych między aplikacjami musi nastąpić wcześniej autoryzacja i uzyskanie tokena,
którego realizacja nastąpiła poprzez wykorzystanie mechanizmów bibliotek JWT. Uzyskane w procesie logowania token aplikacja frontendowa wysyła z każdym kolejnym requestem
do backendu.

API dostępne w aplikacji backendowej jest dostępne w formie WEB serwowanej przez mechanizmy swagger pod powyższym https://localhost:7167/swagger/index.html (w trybie debug/development).

Zapytania GET (zwracanie danych) mogą być realizowane po zalogowaniu się i uzyskaniu tokena obu typów użytkowników (role), czyli zarówno Admin jak i User.
Metody POST (tworzenie rekordów), PUT (aktualizacja rekordów), DELETE (usuwanie rekodorów) mogą zostać wykonywane jedynie za pomocą użytkownika z rolą Admin.
Osoba nie zalogowana poprzez API, nie posiadająca tokenu nie jest w stanie powyższych operacji wykonać.
Rejestracji użytkowników również może dokonać tylko Admin.

## Wykorzystane technologie:
- .NET 6.0
- C#
- JWT
- Swagger
- SQLite
- XAML (WPF .NET)

## Uruchomienie projektu
- Pobrać repozytorium,
- Otworzyć solucję w Visual Studio
- W ustawieniach rozwiązania "Warehouse Manager" w sekcji "Projekt startowy" wybrać opcję "Wiele projektów startowych" i przestawić akcję obu projektów (WarehouseAPI i WarehouseApp) na "Uruchomienie",
jak na poniższym obrazku
![Ustawienie dwóch projektów startowych](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/warehouse-two-start-projs.jpg)
- Nacisnąć klawisz F5 (oba projekty ustawione jako startowe, co powoduje start strony serwowanej przez Swagger-a z opisem API jak i aplikacji front-endowej)

***Uwaga:*** _Baza danych wykorzystywana przez serwer API jest kopiowana do folderu wyjściowego i to z niej korzysta uruchomiona aplikacja. Baza zawiera utworzoną strukturę tabel (encji) z zależnościami
oraz kilka przykładowych rekordów dokumentów i innych danychm które będą widoczne w aplikacji Fron-Endowej w liście kontrahentów, towarów (artykułów) jak i dokumentów._

***Uwaga:*** _Nie ma potrzeby kopiowania bazy SQLite-owej w żadne miejsce - plik jest kopiowany po poprawnej kompilacji do folderu ze zbudowanym projektem aplikacji frontendowej._

## Użytkownicy

Aplikacja posiada dwie role dla użytkownika: _User_ oraz _Admin_

W bazie dodani są predefiniowani następujący użytkownicy:
| EMAIL | Hasło | Rola |
| --- | --- | --- |
| hsu@hs.pl | 123456 | User |
| hsa@hs.pl | 123456 | Admin |

## Schemat Bazy danych

Diagram bazy danych przedstawia poniższy rysunek.

![Diagram bazy danych Warehouse Man](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/warehouse-man-db-diag.jpg)

## Opis działania aplikacji Front-Endowej

![Ekran startwoy aplikacji](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/warehouse-gui-main.jpg)

Autor pominął możliwość rejestracji nowych użytkowników z poziomu GUI aplikacji. Do zalogowania można wykorzystać użytkowników wymienionych w liście powyżej.

Wykonanie wyszarzonych operacji po uruchomieniu aplikacji frontendowej możliwe jest jedynie po uprzednim zalogowaniu się. Okno logowania otwieramy za pomocą przycisku "Logowanie".

![Ekan logowania](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/warehouse-gui-login.jpg)

W zależności od powodzenia operacji logowania wyświetlany jest komunikat o sukcesie bądź nie. W przypadku poprawnego logowania komunikat zawiera również informację o roli zalogowanego
użytkownika.

![Sukces logowania](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/warehouse-gui-login-ok.jpg)

Aplikacji pozwala naciskać przyciski otwierające okna dodawania dokumentów, artykułów czy kontrahentów dla użytkowników obu ról, ale zgodnie z założeniami projektu (zadania z przedmiotu)
zapis rekordów dla użytkowników z rolą User jest blokowany na poziomie API i powoduje zwracania kodu 401. Aplikacji frontendowa wyświetla w takim momencie odpowiedni komunikat. Dla
przykładu próba dodania kontrahenta jako użytkownik o roli _User_ zakończy się komunikatem przedstawionym na kolejnym obrazku

![Zablokowania rola User](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/warehouse-man-role-blocked.jpg)

### Dodanie dokumentu

Aby dodać nowy dokument naciskamy przycisk "Nowy Dokument". Na ekranie formularza dokumentu wybieramy jego typ, dodajemy kontrahenta z listy otwieranej za pomocą przycisku
![1](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/(1).jpg) a następnie uzupełniamy listę
artykułów naciskając (przyciski ![2](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/(2).jpg)) "+" by dodać artykuł do listy, bądź "-",
aby usunąć wybrany artykuł z listy. Dwukrotne kliknięcie na dodanym artykule otwiera okno, w którym możemy uzupełnić ilość towaru na tworzonym dokumencie.
Podczas zapisywania dokumentów w zależności od typu dokumentu (WZ - wydanie, PZ - przyjęcie) ilości towarów podane na dokumencie są dodawane bądź ujmowane ze stanów
magazynowych.

![Formularz nowego dokumentu](https://raw.githubusercontent.com/hubssch/WarehouseMan/main/docs/warehouse-gui-new-doc.jpg)

### Listy dokumentów, kontrahentów i artykułów

Okna listy dokumentów (przycisk "Lista Dokumentów"), kontrahentów (przycisk "Lista Kontrahentów") oraz artykułów (przycisk "Stany magazynowe")
pozwalają na dodawanie wybranych danych za pomocą przycisków "+" lub ich usuwanie za pomocą "-". Użytkownicy z rolą _User_ otrzymają komunikat
o nieudanej próbie danych operacji. Dwuktrony kliknięcie na tych listach otwiera odpowiednie formularze do edycji danych poszczególnych encji:
dokumentów, kontrahentów czy artykułów. Istotne jest np. to, że z poziomu listy artykułów użytkownicy _Admin_ mogą dokonać zmiana stanów magazynowych towarów,
niezależnie od zmian wprowadzanych poprzez nowe dokumenty rejestrowane w systemie.
