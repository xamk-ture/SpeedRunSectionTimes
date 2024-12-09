using System.Diagnostics;

namespace SpeedRunSectionTimes
{
    internal class Program
    {

        // Määritellään pelit ja niiden sektionimet sekä tavoiteajat tupleina.
        // Avain: pelin nimi, Arvo: taulukko tupleja (sektion nimi, sektion tavoiteaika)
        static Dictionary<string, (string nimi, double aika)[]> pelit = new Dictionary<string, (string, double)[]>
        {
            { "SuperGame", new (string, double)[]
                {
                    ("Prologi", 30.0),
                    ("Keskivaihe", 45.0),
                    ("Finaali", 60.0)
                }
            },
            { "MegaRun", new (string, double)[]
                {
                    ("Aloitus", 25.0),
                    ("Metsä", 40.0),
                    ("Loppukoitos", 55.0)
                }
            }
        };


        static void Main(string[] args)
        {
            Console.WriteLine("Valitse peli seuraavista:");
            foreach (var peliNimi in pelit.Keys)
            {
                Console.WriteLine(peliNimi);
            }

            Console.Write("Syötä pelin nimi: ");
            string valittuPeli = Console.ReadLine() ?? "";

            // Tarkistetaan, onko peli olemassa
            if (!pelit.ContainsKey(valittuPeli))
            {
                Console.WriteLine("Peliä ei löydy valikoimasta.");
                return;
            }

            var sektionTiedot = pelit[valittuPeli];

            TulostaOhjeetJaTavoitteet(valittuPeli, sektionTiedot);

            // Odotetaan Enteriä aloitukseen
            Console.WriteLine("Paina Enter aloittaaksesi...");
            Console.ReadLine();

            // Käynnistetään kokonaisajastin
            Stopwatch kokonaistimer = new Stopwatch();
            kokonaistimer.Start();

            //tähän tallenetaan runin section ajat
            double[] tulosAjat = new double[sektionTiedot.Length];

            // Käydään läpi jokainen sektio
            for (int i = 0; i < sektionTiedot.Length; i++)
            {
                SuoritaJaMittaaSektio(i, kokonaistimer, sektionTiedot, tulosAjat);
            }

            kokonaistimer.Stop();

            // Tulostetaan yhteenveto
            TulostaYhteenveto(sektionTiedot, tulosAjat);

            Console.WriteLine("Paina Enter poistuaksesi.");
            Console.ReadLine();
        }

        static void TulostaOhjeetJaTavoitteet(string peli, (string nimi, double aika)[] tavoitteet)
        {
            Console.WriteLine($"Speedrun-ajanmittausohjelma: {peli}");
            Console.WriteLine("Painamalla Enter aloitat ensimmäisen sektion. Jatka painamalla Enter jokaisen sektion lopuksi.");
            Console.WriteLine("Tavoiteajat (s):");
            for (int i = 0; i < tavoitteet.Length; i++)
            {
                Console.WriteLine($"Sektio {i + 1} - {tavoitteet[i].nimi}: {tavoitteet[i].aika}s");
            }
            Console.WriteLine();
        }

        static void SuoritaJaMittaaSektio(int indeksi, Stopwatch timer, (string nimi, double aika)[] tavoitteet, double[] tulokset)
        {
            Console.WriteLine($"Suorita sektio {indeksi + 1} ({tavoitteet[indeksi].nimi}) ja paina Enter kun valmis.");
            Console.ReadLine();

            double kulunutAika = timer.Elapsed.TotalSeconds;
            tulokset[indeksi] = kulunutAika;

            double erotus = kulunutAika - tavoitteet[indeksi].aika;

            Console.WriteLine($"Sektio {indeksi + 1} ({tavoitteet[indeksi].nimi}) valmis.");
            Console.WriteLine($"Kulunut aika: {kulunutAika:0.00}s, Tavoite: {tavoitteet[indeksi].aika}s, Erotus: {(erotus >= 0 ? "+" : "")}{erotus:0.00}s");
            Console.WriteLine();
        }

        static void TulostaYhteenveto((string nimi, double aika)[] tavoitteet, double[] tulokset)
        {
            Console.WriteLine("Koko speedrun valmis!");
            Console.WriteLine("Yhteenveto ajoista:");

            double kokonaisaika = tulokset[tulokset.Length - 1];
            double kokonaisTavoite = 0;

            for (int i = 0; i < tavoitteet.Length; i++)
            {
                kokonaisTavoite += tavoitteet[i].aika;
                Console.WriteLine($"Sektio {i + 1} ({tavoitteet[i].nimi}): Kulunut {tulokset[i]:0.00}s / Tavoite {tavoitteet[i].aika:0.00}s");
            }

            double kokonaisErotus = kokonaisaika - kokonaisTavoite;
            Console.WriteLine();
            Console.WriteLine($"Kokonaisaika: {kokonaisaika:0.00}s, Kokonaistavoite: {kokonaisTavoite:0.00}s, Erotus: {(kokonaisErotus >= 0 ? "+" : "")}{kokonaisErotus:0.00}s");
        }
    }
}
