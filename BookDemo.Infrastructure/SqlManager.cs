using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDemo.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BookDemo.Infrastructure
{
    public class SqlManager : ISqlManager
    {
        private readonly string _connectionString;

        public SqlManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public Book ExecuteScalar<Book>(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var result = command.ExecuteScalar();
                    return result != DBNull.Value ? (Book)Convert.ChangeType(result, typeof(Book)) : default;
                }
            }
        }

        public List<Book> ExecuteReader<Book>(string query, Func<SqlDataReader, Book> readFunc, SqlParameter[] parameters = null)
        {
            List<Book> results = new List<Book>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(readFunc(reader));
                        }
                    }
                }
            }

            return results;
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }


        public string GetAllBooksQuery() => "SELECT * FROM Books";
        public string GetBookByIdQuery() => "SELECT * FROM Books WHERE Id = @Id";
        public string InsertBookQuery() => "INSERT INTO Books (Name, Price, CategoryId) OUTPUT INSERTED.Id VALUES (@Name, @Price, @CategoryId)";
        public string UpdateBookQuery() => "UPDATE Books SET Name = @Name, Price = @Price, CategoryId = @CategoryId WHERE ID = @Id";
        public string DeleteBookQuery() => "DELETE FROM Books WHERE ID = @Id";
        public string DeleteAllBooksQuery() => "DELETE FROM Books";
        public string GetBooksByCategoryQuery() => "SELECT ID, Name, Price, CategoryId FROM Books WHERE CategoryId = @CategoryId";
        public string GetBooksWithCategoryQuery() => "SELECT B.ID, B.Name, B.Price, C.Name AS CstegoryName FROM Books AS B JOIN Categories AS C ON B.CategoryID = C.Id;";

        public string GetAllCategoriesQuery() => "SELECT * FROM Categories";
        public string GetCategoryByIdQuery() => "SELECT * FROM Categories WHERE Id = @Id";
        public string InsertCategoryQuery() => "INSERT INTO Categories (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
        public string UpdateCategoryQuery() => "UPDATE Categories SET Name = @Name WHERE Id = @Id";
        public string DeleteCategoryQuery() => "DELETE FROM Categories WHERE Id = @Id";


    }
}
