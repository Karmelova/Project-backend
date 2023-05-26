# Dokumentacja projektu - Programowanie aplikacji backendowych
## Instalacja
### Wymagania wstępne
- .NET 6 
- Visual Studio
- SQL Server Management Studio
### Przygotowanie 
Należy pobrać projekt. Po uruchomieniu rozwiązania `TeamMinder.sln` projektu w plikach sub projektu `WebApi` zmodyfikować i ustawić własne połączenie do bazy danych, w plikach: `appsettings.json` oraz `ApplicationDbContext.cs`. Za pomocą konsoli powershell należy użyć polecenia `cd webapi` w celu dostania się do odpowiednego projektu, a następnie polecenia `dotnet ef database update` w celu utworzenia tabel.
## Konta testowe
### Administrator
login: admin

hasło: !Administrator123
### Użytkownik
login: user

hasło: !User123
## Opis aplikacji
Aplikacja TeamMinder służy do zarządzania projektami i zadaniami do wykonania w ramach projektu i milestone. Uwierzytelnianie użytkownika odbywa się za pomocą tokenu JWT po wysłaniu żądania zawierającego prawidłowe dane do logowania. Dokumentacja endpointów jest dostępna po uruchomieniu projektu `WebApi` w swagger. Polecenia register oraz login nie posiadają zabezpieczenia autoryzującego. Wszystkie polecenia delete posiadają zabezpieczenie, aby tylko osoby z rolą administratora były uprawnione do usuwania.
Zadania mogą być przypisane tylko do jednego milestone, milestone może zostać przypisany tylko do jednego projektu. Istnieje możliwość stworzenia kilku projektów.
