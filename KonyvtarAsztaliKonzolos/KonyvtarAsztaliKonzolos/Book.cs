using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonyvtarAsztaliKonzolos
{
    internal class Book
    {
        private string author;
        private int id;
        private int page_count;
        private int publish_year;
        private string title;

        public Book(string title, string author, int publish_year, int page_count, int id = 0)
        {
            this.author = author;
            this.id = id;
            this.page_count = page_count;
            this.publish_year = publish_year;
            this.title = title;
        }

        public string Author { get => author; set => author = value; }
        public int Id { get => id; set => id = value; }
        public int Page_count { get => page_count; set => page_count = value; }
        public int Publish_year { get => publish_year; set => publish_year = value; }
        public string Title { get => title; set => title = value; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Szerző: {this.author}");
            sb.AppendLine($"Cím: {this.title}");
            sb.AppendLine($"Kiadás éve: {this.publish_year}");
            sb.AppendLine($"Oldalszám: {this.page_count}");
            return sb.ToString();
        }
    }
}
