using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci.Where(s => s.Miasto == "Warsaw").Select(s =>
            $"indeks:{s.NumerIndeksu}; Imie i nazwisko:{s.Imie}, {s.Nazwisko}; Miasto: {s.Miasto}");
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Email);
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s => $"indeks: {s.NumerIndeksu}, Imie Nazwisko: {s.Imie} {s.Nazwisko}");
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var subject = DaneUczelni.Przedmioty
            .Where(p => p.Kategoria == "Analytics")
            .Select(p => p.Nazwa)
            .FirstOrDefault("Przedmiot z kategorii analytics nie istnieje");
        var res = new List<string>();
        res.Add(subject);
        return res;
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var isActive = DaneUczelni.Zapisy
            .Any(z => !z.CzyAktywny);
        string response = isActive ? "Tak" : "Nie";
        var resultList = new List<string>();
        resultList.Add(response);
        return resultList;
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var x = DaneUczelni.Prowadzacy
            .All(p => !string.IsNullOrEmpty(p.Katedra));
        var sentence =
            x ? "Kazdy prowadzacy ma uzupelniona nazwe katedry" : "Nie kazdy prowadzacy ma uzupelniona nazwe katedry";
        var res = new List<string>();
        res.Add(sentence);
        return res;
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var x = DaneUczelni.Zapisy
            .Where(z => z.CzyAktywny)
            .Count();
        var res = new List<string>();
        res.Add($"W systemie znajduje sie {x} aktywnych zapisow");
        return res;
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .OrderBy(s => s.Miasto)
            .Select(s => s.Miasto)
            .Distinct();
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu} ID studenta: {z.StudentId}, ID przedmiotu: {z.PrzedmiotId}");
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
       return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa} {p.Kategoria}");
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var x = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (student, zapis) => new
                {
                    Info = $"{student.Imie} {student.Nazwisko}: {zapis.DataZapisu}"
                }
            );
        return x.Select(inf => inf.ToString());
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        var query = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (student, zapis) => new
                {
                    Imie = student.Imie,
                    Nazwisko = student.Nazwisko,
                    IdPrzedmiot = zapis.PrzedmiotId
                }
            ).Join(
                DaneUczelni.Przedmioty,
                x => x.IdPrzedmiot,
                p => p.Id,
                (studentInfo, przedmiot) => new
                {
                    Imie = studentInfo.Imie,
                    Nazwisko = studentInfo.Nazwisko,
                    NazwaPrzedmiotu = przedmiot.Nazwa
                }
            );
        var result = new List<string>();
        foreach (var info in query)
        {
            result.Add($"{info.Imie} {info.Nazwisko} {info.NazwaPrzedmiotu}");
        }

        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var y = DaneUczelni.Przedmioty
            .GroupJoin(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (przedmiot, zapisy) => new
                {
                    NazwaPrzedmiotu = przedmiot.Nazwa,
                    LiczbaZapisow = zapisy.Count()
                }
            );
        var res = new List<string>();
        foreach (var info in y)
        {
            res.Add($"{info.NazwaPrzedmiotu}: {info.LiczbaZapisow}");
        }

        return res;
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var x = DaneUczelni.Przedmioty
            .GroupJoin(
                DaneUczelni.Zapisy,
                p => p.Id,
                z => z.PrzedmiotId,
                (przedmiot, zapisy) => new
                {
                    NazwaPrzedmiotu = przedmiot.Nazwa,
                    SredniaOcen = zapisy.Where(z => z.OcenaKoncowa != null).Select(z => z.OcenaKoncowa).Average()
                }
            );
        var res = new List<string>();
        foreach (var entry in x)
        {
            res.Add($"{entry.NazwaPrzedmiotu}: {entry.SredniaOcen}");
        }

        return res;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var x = DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                prow => prow.Id,
                p => p.ProwadzacyId,
                (prowadzacy, przedmioty) => new
                {
                    ImieProwadzacego = prowadzacy.Imie,
                    NazwiskoProwadzacego = prowadzacy.Nazwisko,
                    LiczbaPrzedmiotow = przedmioty.Count()
                }
            );
        var res = new List<string>();
        foreach (var info in x)
        {
            res.Add($"Prowadzacy {info.ImieProwadzacego} {info.NazwiskoProwadzacego}, liczba przedmiotow: {info.LiczbaPrzedmiotow}");
        }

        return res;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var x = DaneUczelni.Studenci
            .GroupJoin(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (student, zapisy) => new
                {
                    ImieStudenta = student.Imie,
                    NazwiskoStudenta = student.Nazwisko,
                    NajwyzszaOcena = zapisy.Select(z => z.OcenaKoncowa).Max()
                }
            );

        var result = new List<string>();
        foreach (var rec in x)
        {
            result.Add($"Student {rec.ImieStudenta} {rec.NazwiskoStudenta}: {rec.NajwyzszaOcena}");
        }

        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var x = DaneUczelni.Studenci
            .GroupJoin(
                DaneUczelni.Zapisy,
                s => s.Id,
                z => z.StudentId,
                (student, zapisy) => new
                {
                    Imie = student.Imie,
                    Nazwisko = student.Nazwisko,
                    LiczbaPrzedmiotow = zapisy.Where(z => z.CzyAktywny).Count()
                }
            ).Where(x => x.LiczbaPrzedmiotow > 1);
        
        var result = new List<string>();
        foreach (var rec in x)
        {
            result.Add($"{rec.Imie} {rec.Nazwisko}: {rec.LiczbaPrzedmiotow} aktywne przedmioty");
        }

        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var x = DaneUczelni.Przedmioty.Where(p => p.DataStartu.Month == 4 && p.DataStartu.Year == 2026)
            .GroupJoin(
                DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa is not null),
                p => p.Id,
                z => z.PrzedmiotId,
                (przedmiot, zapisy) => new
                {
                    NazwaPrzedmiot = przedmiot.Nazwa,
                    Ilosc = zapisy.Count()
                }
            ).Where(r => r.Ilosc == 0);

        var result = new List<string>();
        foreach (var rec in x)
        {
            result.Add($"{rec.NazwaPrzedmiot}: ilosc -> {rec.Ilosc}");
        }

        return result;
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
        var ZapisPrzedmiotJoined = DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa is not null)
            .Join(
                DaneUczelni.Przedmioty,
                z => z.PrzedmiotId,
                p => p.Id,
                (zapis, przedmiot) => new
                {
                    ProwadzacyId = przedmiot.ProwadzacyId,
                    OcenaKoncowa = zapis.OcenaKoncowa
                }
            );

        var ProwadzacyGrouped = DaneUczelni.Prowadzacy
            .GroupJoin(
                ZapisPrzedmiotJoined,
                pr => pr.Id,
                zp => zp.ProwadzacyId,
                (prowadzacy, oceny) => new
                {
                    Imie = prowadzacy.Imie,
                    Nazwisko = prowadzacy.Nazwisko,
                    SredniaOcen = oceny.Average(zp => zp.OcenaKoncowa)
                }
            );

        var result = new List<string>();
        foreach (var v in ProwadzacyGrouped)
        {
            result.Add($"{v.Imie} {v.Nazwisko}: srednia ocen {v.SredniaOcen}");
        }

        return result;
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
        var studentZapisJoined = DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                s => s.Id,
                z => z.StudentId,
                (student, zapis) => new
                {
                    Miasto = student.Miasto,
                    ZapisId = zapis.Id
                }
            );
        var groupedBy = studentZapisJoined.GroupBy(
            x => x.Miasto,
            (miasto, zapisy) => new
            {
                Miasto = miasto,
                LiczbaAktywnychZapisow = zapisy.Count()
            }
        );
        var result = new List<string>();
        foreach (var v in groupedBy)
        {
            result.Add($"{v.Miasto} liczba aktywnych zapisów: {v.LiczbaAktywnychZapisow}");
        }

        return result;
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}