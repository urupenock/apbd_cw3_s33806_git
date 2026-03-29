W trakcie pracy wykorzystałam różne operatory LINQ, żeby rozwiązać postawione problemy:
Filtrowanie i sortowanie: użyłam Where, OrderBy oraz ThenBy (np. przy liście studentów z Warszawy czy sortowaniu po nazwisku)
Pobieranie konkretnych danych: Select do wyciągania samych maili oraz Distinct do listy unikalnych miast
Łączenie tabel: zastosowałam Join, żeby powiązać studentów z ich zapisami na przedmioty
Grupowanie i statystyki: dzięki GroupBy, Average i Max przygotowałam raporty o średnich ocenach i liczbie osób na danym przedmiocie
Paginacja: użyłam Skip i Take, żeby podzielić listę przedmiotów na strony
