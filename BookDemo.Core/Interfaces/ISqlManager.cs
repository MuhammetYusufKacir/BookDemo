using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BookDemo.Core.Interfaces
{
    public interface ISqlManager
    {
        SqlConnection GetConnection();

        Book ExecuteScalar<Book>(string query, SqlParameter[] parameters = null);

        List<Book> ExecuteReader<Book>(string query, Func<SqlDataReader, Book> readFunc, SqlParameter[] parameters = null);

        int ExecuteNonQuery(string query, SqlParameter[] parameters = null);


        string GetAllBooksQuery();
        string GetBookByIdQuery();
        string InsertBookQuery();
        string UpdateBookQuery();
        string DeleteBookQuery();
        string DeleteAllBooksQuery();

        string GetAllCategoriesQuery();
        string GetCategoryByIdQuery();
        string InsertCategoryQuery();
        string UpdateCategoryQuery();
        string DeleteCategoryQuery();
        string GetBooksByCategoryQuery();
        string GetBooksWithCategoryQuery();
    }
}
