using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Entities;

namespace BookDemo.Infrastructure.Data
{
    public static class ApplicationContext
    {
        public static List<Book> Books { get; set; }
        static ApplicationContext()
        {
            Books = new List<Book>()
            {
                new Book(){Id=1, Name="Karagöz ve Hacivat", Price=75},
                new Book(){Id=2, Name="Dede Korkut", Price=100},
                new Book(){Id=3, Name="Pamuk Prenses", Price=110}
            };
        }
    }
}
