# Dokumentacja Projektu: System Hotelowy (SystemHotelowy)
## 1. Informacje Ogólne
- **Cel: Aplikacja webowa do kompleksowego zarządzania obiektem hotelowym, obejmująca obsługę pokoi, kategoryzację standardów oraz proces rezerwacji i obsługi gościa.**
- **Autor: Eliza Lenik**

## 2. Stos Technologiczny (Tech Stack)
- Backend: .NET 10 / ASP.NET Core MVC
- Baza danych: Microsoft SQL Server
- ORM: Entity Framework Core (pakiety: SqlServer, Design, Tools)
- Autoryzacja: ASP.NET Core Identity z rozszerzonym modelem użytkownika AppUser (FirstName, LastName).

## 3. Architektura i Modele Danych
- Rooms (Pokoje): Przechowuje numer pokoju, pojemność, cenę za noc oraz opis. Powiązana relacją z RoomType.
- RoomType (Typy Pokoi): Słownik standardów (np. Single, Double, Apartment).
- Booking (Rezerwacje): Kluczowy model łączący Room, Visitor (Gość) oraz Receptionist. Zawiera daty pobytu, datę rezerwacji oraz status.
- Statutes (Statusy): Słownik stanów rezerwacji.
|ID|Nazwa|Tłumaczenie|Przeznaczenie|
| --- | --- | --- | --- |
|1|Pending|Oczekujące|Rezerwacja dokonana przez Gościa.|
|2|Confirmed|Potwierdzona|Zatwierdzona przez recepcję lub utworzona przez nią.|
|3|Canceled|Odwołana|Rezerwacja anulowana (widok Canceled.cshtml).|
|4|Check-in|Zameldowany|Gość fizycznie znajduje się w hotelu.|
|5|Check-out|Wymeldowany|Pobyt zakończony, pokój zwolniony.|
|6|No-show|Nieobecny|Rezerwacja nie została zrealizowana (gość nie dotarł).|

- System Ról: Admin, Receptionist, Visitor.
- Reception:  Zarządzanie rezerwacjami, zmienianie statusów rezerwacji. 
 
## 4. Kluczowe Funkcjonalności i Logika Techniczna
**Tworzenie Rezerwacji (BookingsController)**
- Filtrowanie dostępności: System przy tworzeniu rezerwacji sprawdza, czy wybrany pokój nie jest zajęty w danym terminie przez inną, nieanulowaną rezerwację.
- Automatyzacja ról: Jeśli rezerwację tworzy Gość, system przypisuje mu status "Pending(Oczekująca)" (ID 1).
- Jeśli rezerwację tworzy Recepcjonista, otrzymuje ona od razu status "Confirmed(Potwierdzona)" (ID 2).
- Obliczanie ceny: System automatycznie wylicza TotalPrice na podstawie liczby dni pobytu i ceny pokoju (PricePerNight).
**Bezpieczeństwo Danych i Stabilność**
- W kontrolerach Rooms i Bookings zastosowano metodę .Include(). Gwarantuje to, że obiekty powiązane (jak RoomType czy Visitor) nie są wartościami null w widokach, co eliminuje błędy NullReferenceException.
- W widokach edycji zastosowano pola ukryte (<input type="hidden">) dla kluczy obcych (VisitorId, StatusId) oraz daty utworzenia rezerwacji. Zapobiega to nadpisywaniu istotnych danych wartościami domyślnymi podczas edycji innych parametrów.

## 5. Instalacja i Konfiguracja
1.	Baza danych: Skonfiguruj ConnectionString w pliku appsettings.json.
2.	Migracje: Uruchom w Package Manager Console:
- add-migration InitialMigration
- update-database
3. Uruchomić program w VS
  Po uruchomieniu programu utworzyły się 3 konta użytkowników:
  **Administrator** - Login: admin@hotel.pl Hasło: Administracja123!
  **Recepcjonista** - Login: recepcja@hotel.pl Hasło: Recepcja123!
  **Gość** - Login: gosc@hotel.pl Hasło: Visitor123!

## 6. Instrukcja Obsługi Modułów
- Receptionist Panel: Umożliwia pełne zarządzanie cyklem życia rezerwacji: potwierdzanie nowych zgłoszeń, check-in, check-out oraz anulowanie wizyt, zaznaczanie tych nieodbytych.
- Room Management: Administrator definiuje bazę noclegową, przypisując każdemu pokojowi jego numer, pojemność oraz odpowiedni standard z listy rozwijanej pobieranej z bazy danych, którą można edytować lub rozwijać za pomocą Add New Room Type.
- Room Reservation: Umożliwia rezerwację pokoju. W widoku gościa są dostępne tylko jego rezerwacje z możliwością odwołania nie odbytych rezerwacji. W widoku recepcjonisty możliwość utworzenia nowych i przejrzenia wszystkich zrobionych rezerwacji

## 7.Opis kontrolerów i plików
**ApplicationDBContext.cs** – połączenie z bazą danych oraz zdefiniowanie niezbędnych danych dla działania aplikacji, takich jak użytkownicy, 3 podstawowe typy pokoi, statusy rezerwacji.
**AppUser.cs** – zdefiniowanie użytkownika
**Folder Pages** – definiuje cały system logowania oraz rejestracji
**AdminController.cs** – tworzy Administration Panel dla użytkowników z uprawnieniami „Admin”
- Views/Admin/Index.cshtml – widok całego panelu, tworzy tabele użytkowników z ich rolami. Umożliwia zmianę lub usunięcie uprawnień za pomocą przycisków
**BookingsController.cs** – umożliwia rezerwacje pokoi - tworzy, edytuje, usuwa i odwołuje rezerwację oraz udostępnia widok detali rezerwacji
- Views/Bookings/Canceled.cshtml – widok odwoływania rezerwacji dostępny dla gości. Widok zapewnia ostrzeżenia o skutkach odwołania rezerwacji
- Views/Bookings/Create.cshtml – widok tworzenia rezerwacji, dla recepcjonistów z dodatkowym wyborem dla kogo jest rezerwacja. Dla użytkownika z uprawnieniami gościa widoczny jest wybór daty pobytu, liczby gości oraz dostępnych pokoi o podanych parametrach.
- Views/Bookings/Delete.cshtml – widok usuwania rezerwacji z wszystkimi detalami i ostrzeżeniem o usunięciu rezerwacji.
- Views/Bookings/Details.cshtml – widok szczegółów rezerwacji
- Views/Bookings/Edit.cshtml – widok edycji rezerwacji, dostępny tylko dla recepcjonisty. Umożliwia edycje rezerwacji – zmianę daty, pokoju lub ceny.
- Views/Bookings/Index.cshtml – widok tabeli wszystkich rezerwacji z przyciskami do edycji, detali oraz usuwania. Dla gości dostępna jest tylko opcja Cancel – odwołanie, jeśli rezerwacja nie została zrealizowana. 
**HomeController.cs** – tworzy widok startowy dla wszystkich użytkowników, „strona główna” hotelu.
**ReceptionController.cs** – tworzy Reception Panel dostępny tylko dla recepcjonistów, który dzieli rezerwacje ze względu na ich status, umożliwia potwierdzenie rezerwacji, check-in, check-out oraz zaznaczenie kto się nie pojawił.
- Views/Reception/Index.cshtml – widok całego panelu z 4 zakładkami
- Views/Reception/Details.cshtml – widok dla detali rezerwacji
**RoomsController** – tworzy pokoje do rezerwacji, dostępne tylko dla Administratora
- Views/Rooms/Create.cshtml – widok tworzenia nowego pokoju. 
- Views/Rooms/Edit.cshtml – widok edycji pokoju. 
- Views/Rooms/Details.cshtml – widok detali pokoju.
- Views/Rooms/Delete.cshtml – widok dla usunięcia pokoju.
- Views/Rooms/Index.cshtml – widok tabeli pokoi.
**RoomTypesController** – tworzy typy pokoi, dostępne tylko dla Administratora
- Views/RoomTypes/Create.cshtml – widok tworzenia typów pokoi
- Views/RoomTypes/Delete.cshtml – widok usuwania typów pokoi 
- Views/RoomTypes/Edit.cshtml – widok edycji typów pokoi
- View/RoomType/Index.cshtml – widok listy typów pokoi





