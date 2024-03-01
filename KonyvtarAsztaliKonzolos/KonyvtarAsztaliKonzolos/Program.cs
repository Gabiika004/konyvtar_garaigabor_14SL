using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonyvtarAsztaliKonzolos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Statisztika statisztika = new Statisztika();
            List<Book> books = new List<Book>();

            //1# Felvesszük az összes könyvet egy listába
            books = statisztika.getAllBooks();

            //2# 500 oldalnál hosszabb könyvek száma:
            Console.WriteLine($"500 oldalnál hosszabb könyvek száma: {statisztika.GetBooksPage500PlusCount()}");

            //3# 1950-nél régebbi könyv 
            int year = 1950;
            Console.WriteLine(
                statisztika.BookOlderThan(year) ? $"Van {year}-nél régebbi könyv" : $"Nincs {year}-nél régebbi könyv");

            //4# Leghosszabb könyv
            Book longestBook = statisztika.GetLongestBook();
            Console.WriteLine("A leghosszabb könyv: ");
            Console.WriteLine(longestBook);

            //5# Legtöbb könyvel rendelkező szerző
            Console.WriteLine($"A legtöbb könyvvel rendelkező szerző:{statisztika.GetAuthorWithMostBooks()}");

            //6# Könyv cím és a hozzá tartozó szerző
            Console.WriteLine("Adjon meg egy könyv címet:");
            string title = Console.ReadLine();
            string answer = statisztika.TitleSearch(title) ? $"A megadott könyv szerzője: {statisztika.GetAuthorByTitle(title)}" : "Nincs ilyen könyv";
            Console.WriteLine(answer);


            Console.ReadKey();
        }
    }
}
