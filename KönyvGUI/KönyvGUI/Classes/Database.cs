using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KonyvGUI.Classes;
using MySql.Data.MySqlClient;

namespace KonyvGUI.Classes
{
    internal class Database
    {
        private List<Book> bookList = new List<Book>();
        MySqlConnection conn = null;
        MySqlCommand sql = null;


        //Database
        public Database()
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


        internal void AddBook(Book newBook)
        {
            try
            {
                ConnectionOpen();
                sql.CommandText = "INSERT INTO `books` (`title`, `author`, `publish_year`, `page_count`) VALUES (@title, @author, @publish_year, @page_count)";
                sql.Parameters.AddWithValue("@title", newBook.Title);
                sql.Parameters.AddWithValue("@author", newBook.Author);
                sql.Parameters.AddWithValue("@publish_year", newBook.Publish_year);
                sql.Parameters.AddWithValue("@page_count", newBook.Page_count);

                sql.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Adatbázis hiba: {ex.Message}");
            }
            finally
            {
                ConnectionClose();
                sql.Parameters.Clear();
            }
        }

        internal bool DeleteBook(int id)
        {
            try
            {
                ConnectionOpen();
                sql.CommandText = "DELETE FROM `books` WHERE `id` = @id";
                sql.Parameters.AddWithValue("@id", id);

                sql.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Adatbázis hiba: {ex.Message}");
                return false;
            }
            finally
            {
                ConnectionClose();
                sql.Parameters.Clear();
            }
        }

        internal void ChangeBook(int id, string newTitle, string newAuthor, int newPublishYear, int newPageCount)
        {


            try
            {
                ConnectionOpen();
                sql.CommandText = "UPDATE `books` SET `title` = @title, `author` = @author, `publish_year` = @publishYear, `page_count` = @pageCount WHERE `id` = @id";
                sql.Parameters.AddWithValue("@id", id);
                sql.Parameters.AddWithValue("@title", newTitle);
                sql.Parameters.AddWithValue("@author", newAuthor);
                sql.Parameters.AddWithValue("@publishYear", newPublishYear);
                sql.Parameters.AddWithValue("@pageCount", newPageCount);

                sql.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Adatbázis hiba: {ex.Message}");
            }
            finally
            {
                ConnectionClose();
                sql.Parameters.Clear();
            }
        }


        internal void UpdateDatabase(List<Book> booksToUpdate)
        {
            try
            {
                sql.CommandText = "DELETE FROM `books`";
                ConnectionOpen();
                sql.ExecuteNonQuery();
                ConnectionClose();

                foreach (Book book in booksToUpdate)
                {
                    sql.CommandText = "INSERT INTO `books` (`title`, `author`, `publish_year`, `page_count`) VALUES (@title, @author, @publish_year, @page_count)";
                    sql.Parameters.AddWithValue("@title", book.Title);
                    sql.Parameters.AddWithValue("@author", book.Author);
                    sql.Parameters.AddWithValue("@publish_year", book.Publish_year);
                    sql.Parameters.AddWithValue("@page_count", book.Page_count);
                    ConnectionOpen();
                    sql.ExecuteNonQuery();
                    ConnectionClose();

                    sql.Parameters.Clear();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal List<string> getAllAuthors()
        {
           List<string> result = new List<string>();

            foreach (var item in bookList)
            {
                if (!result.Contains(item.Author))
                {
                    result.Add(item.Author);
                }
            }

           return result;
        }
    }
}
