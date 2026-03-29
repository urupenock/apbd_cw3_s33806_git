using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci
            .Where(s => s.Miasto == "Warsaw")
            .Select(s => $"{s.NumerIndeksu}: {s.Imie} {s.Nazwisko} ({s.Miasto})");
    }

    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Email);
    }

    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s => $"{s.NumerIndeksu}: {s.Nazwisko} {s.Imie}");
    }

    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var item = DaneUczelni.Przedmioty
            .FirstOrDefault(p => p.Kategoria == "Analytics");

        return item != null 
            ? new[] { $"{item.Nazwa} - {item.DataStartu:d}" } 
            : new[] { "Nie znaleziono przedmiotu Analytics." };
    }

    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        bool exists = DaneUczelni.Zapisy.Any(z => !z.CzyAktywny);
        return new[] { exists ? "Tak" : "Nie" };
    }

    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        bool result = DaneUczelni.Prowadzacy.All(p => !string.IsNullOrEmpty(p.Katedra));
        return new[] { result ? "Tak" : "Nie" };
    }

    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        int count = DaneUczelni.Zapisy.Count(z => z.CzyAktywny);
        return new[] { $"Aktywne zapisy: {count}" };
    }

    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .OrderBy(m => m);
    }

    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu:d} | Student: {z.StudentId} | Przedmiot: {z.PrzedmiotId}");
    }

    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa} ({p.Kategoria})");
    }

    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy,
                s => s.Id, 
                z => z.StudentId, 
                (s, z) => $"{s.Imie} {s.Nazwisko} | Data zapisu: {z.DataZapisu:d}");
    }

    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Join(DaneUczelni.Studenci, z => z.StudentId, s => s.Id, (z, s) => new { z, s })
            .Join(DaneUczelni.Przedmioty, temp => temp.z.PrzedmiotId, p => p.Id, 
                (temp, p) => $"{temp.s.Imie} {temp.s.Nazwisko} -> {p.Nazwa}");
    }

    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(DaneUczelni.Przedmioty, z => z.PrzedmiotId, p => p.Id, (z, p) => p.Nazwa)
            .GroupBy(nazwa => nazwa)
            .Select(g => $"Przedmiot: {g.Key} | Liczba zapisów: {g.Count()}");
    }

    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(DaneUczelni.Przedmioty, z => z.PrzedmiotId, p => p.Id, (z, p) => new { p.Nazwa, z.OcenaKoncowa })
            .GroupBy(x => x.Nazwa)
            .Select(g => $"Przedmiot: {g.Key} | Średnia: {g.Average(x => x.OcenaKoncowa):F2}");
    }

    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(DaneUczelni.Przedmioty, 
                pr => pr.Id, 
                p => p.ProwadzacyId, 
                (pr, przedmioty) => $"{pr.Imie} {pr.Nazwisko} | Liczba przedmiotów: {przedmioty.Count()}");
    }

    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(DaneUczelni.Studenci, z => z.StudentId, s => s.Id, (z, s) => new { s, z.OcenaKoncowa })
            .GroupBy(x => $"{x.s.Imie} {x.s.Nazwisko}")
            .Select(g => $"Student: {g.Key} | Najwyższa ocena: {g.Max(x => x.OcenaKoncowa)}");
    }

    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .Join(DaneUczelni.Studenci, z => z.StudentId, s => s.Id, (z, s) => s)
            .GroupBy(s => $"{s.Imie} {s.Nazwisko}")
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key} - Aktywne przedmioty: {g.Count()}");
    }

    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .GroupJoin(DaneUczelni.Zapisy, 
                p => p.Id, 
                z => z.PrzedmiotId, 
                (p, zapisy) => new { p.Nazwa, zapisy })
            .Where(x => !x.zapisy.Any(z => z.OcenaKoncowa.HasValue))
            .Select(x => $"Przedmiot: {x.Nazwa} (Brak ocen)");
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        throw Niezaimplementowano(nameof(Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach));
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        throw Niezaimplementowano(nameof(Wyzwanie04_MiastaILiczbaAktywnychZapisow));
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
