using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonyvtarAsztaliKonzolos
{
    internal class Statisztika
    {
        private List<Book> bookList = new List<Book>();
        MySqlConnection conn = null;
        MySqlCommand sql = null;


        //Database
        public Statisztika()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "books";
            builder.CharacterSet = "utf8";
            conn = new MySqlConnection(builder.ConnectionString);
            sql = conn.CreateCommand();
            try
            {
                ConnectionOpen();
            }
            catch (MySqlException ex)
            {

                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
            finally { ConnectionClose(); }
        }

        private void ConnectionClose()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
        }

        private void ConnectionOpen()
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

        }

        //Függvények

        internal List<Book> getAllBooks()
        {
            sql.CommandText = "SELECT * FROM `books` ORDER BY  `id`";
            List<Book> tempBookList = new List<Book>();
            try
            {
                ConnectionOpen();
                using (MySqlDataReader dr = sql.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int id = dr.GetInt32("id");
                        string title = dr.GetString("title");
                        string author = dr.GetString("author");
                        Int16 publish_year = dr.GetInt16("publish_year");
                        int page_count = dr.GetInt32("page_count");

                        tempBookList.Add(new Book(title, author, publish_year, page_count, id));
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ConnectionClose();
            }

            bookList = tempBookList;
            return tempBookList;
        }


        public int GetBooksPage500PlusCount() 
        {
            int count = 0;
            List<Book> bookList = this.bookList;

            foreach (Book book in bookList. Where(x => x.Page_count > 500))
            {
                count++;
            }
            return count;
        }

        public bool BookOlderThan(int publish_year)
        {
            bool result = false;
            foreach (Book book in bookList)
            { 
                if(book.Publish_year < 1950)
                {
                    result = true;
                }
            }

            return result;
        }

        public Book GetLongestBook()
        {
            Book longest = null;
            int max = 0;

            foreach (Book book in this.bookList)
            {
                if (book.Page_count > max)
                {
                    longest = book;
                    max = book.Page_count; // Frissítsük a leghosszabb oldalszámot
                }
            }
            return longest;
        }


        public string GetAuthorWithMostBooks()
        {
            Dictionary<string, int> authorBookCount = new Dictionary<string, int>();

            foreach (Book book in this.bookList)
            {
                if (authorBookCount.ContainsKey(book.Author))
                {
                    authorBookCount[book.Author]++;
                }
                else
                {
                    authorBookCount[book.Author] = 1;
                }
            }

            string authorWithMostBooks = string.Empty;
            int maxBooks = 0;

            foreach (var pair in authorBookCount)
            {
                if (pair.Value > maxBooks)
                {
                    maxBooks = pair.Value;
                    authorWithMostBooks = pair.Key;
                }
            }

            return authorWithMostBooks;
        }


        public bool TitleSearch(string title) 
        {
            bool available = false;

            foreach (Book book in bookList)
            {
                if(book.Title == title)
                {
                    available = true;
                }
            }

            return available;
        }

        public string GetAuthorByTitle(string title) 
        {

            string author = string.Empty;
            
            foreach (Book book in this.bookList.Where(x=> x.Title == title))
            {
                author = book.Author;
            }

            return author;
        }


    }
}
