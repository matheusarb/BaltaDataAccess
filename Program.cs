﻿using BaltaDataAccess.Models;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";
        
        using(var connection = new SqlConnection(connectionString))
        {
            // UpdateCategoryTitle(connection);
            // DeleteCategory(connection, "9b06ecf8-a8f9-4196-9e3b-fb8fc872b06f");
            // CreateManyCategories(connection);
            // DeleteManyCategories(connection);
            

            // ListStudents(connection);
            // ListCategories(connection);
            // ExecuteProcedure(connection);
            // ExecuteReadProcedureGetCoursesByCategory(connection, "09ce0b7b-cfca-497b-92c0-3290ad9d5142");
            ExecuteScalar()
        }
    }

    static void ListCategories(SqlConnection connection)
    {
        var categories = connection.Query<Category>(
            "SELECT [Id], [Title], [Order] FROM [Category] ORDER BY [Order] ASC"
        );

        foreach(var item in categories)
        {
            System.Console.WriteLine($"{item.Order} | {item.Id} - {item.Title}");
        }
    }

    static void ListStudents(SqlConnection connection)
    {
        var sqlQuery = connection.Query<Student>(
            "SELECT [Id], [Name] FROM [Student]"
        );

        if(sqlQuery.Count() == 0)
            System.Console.WriteLine("Não há estudante cadastrado");

        foreach(var student in sqlQuery)
        {
            System.Console.WriteLine($"{student.Id} - {student.Name}");
        }
    }

    static void CreateCategory(SqlConnection connection, Category category)
    {
        var insertSqlCategory = @"INSERT INTO
                [Category]
            VALUES (
                @Id,
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured
                )
            ";

        var rows = connection.Execute(insertSqlCategory, new Category
        {
            Id = category.Id,
            Title = category.Title,
            Url = category.Url,
            Summary = category.Summary,
            Order = category.Order,
            Description = category.Description,
            Featured = category.Featured
        });

        System.Console.WriteLine($"{rows} linha(s) inserida(s)");
    }

    static void CreateManyCategories(SqlConnection connection)
    {
        var category1 = new Category();
        category1.Id = Guid.NewGuid();
        category1.Title = "novacategoria1";
        category1.Url = "novacategoria1";
        category1.Summary = "novacategoria1";
        category1.Order = 9;
        category1.Description = "novacategoria1";
        category1.Featured = false;
        
        var category2 = new Category();
        category2.Id = Guid.NewGuid();
        category2.Title = "novacategoria2";
        category2.Url = "novacategoria2";
        category2.Summary = "novacategoria2";
        category2.Order = 10;
        category2.Description = "novacategoria2";
        category2.Featured = false;
        
        var insertCategory = @"INSERT INTO
            [Category]
            VALUES (
                @Id,
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured
            )
        ";

        var rows = connection.Execute(insertCategory, new[] {
            new {
                category1.Id,
                category1.Title,
                category1.Url,
                category1.Summary,
                category1.Order,
                category1.Description,
                category1.Featured,
            },
            new {
                category2.Id,
                category2.Title,
                category2.Url,
                category2.Summary,
                category2.Order,
                category2.Description,
                category2.Featured,
            }
        });

        System.Console.WriteLine($"{rows} linha(s) inserida(s)");
    }

    static void UpdateCategoryTitle(SqlConnection connection)
    {
        var updateQuery = "UPDATE [Category] SET [Title] = @Title WHERE [Id] = @Id";

        var rows = connection.Execute(updateQuery, new {
            Id = "af3407aa-11ae-4621-a2ef-2028b85507c4",
            Title = "Frontend 2024"
        });

        System.Console.WriteLine($"{rows} registros atualizados");
    }

    static void DeleteCategory(SqlConnection connection, string Id)
    {
        var deleteQuery = "DELETE FROM [Category] WHERE [Id] = @Id";

        var rows = connection.Execute(deleteQuery, new {
            Id = Id
        });

        System.Console.WriteLine($"{rows} registros atualizados\n");
    }

    static void DeleteManyCategories(SqlConnection connection)
    {
        var deleteQuery = "DELETE FROM [Category] WHERE [Id] = @Id";

        var rows = connection.Execute(deleteQuery, new[] {
            new {
                Id = "ece5e8b7-9236-40f0-b92f-19aba5c70d67"
            },
            new {
                Id = "1ccbebb1-2cd3-41a2-9490-84517167b05f"
            }
        });
        
        System.Console.WriteLine($"{rows} registros atualizados\n");
    }

    static void ExecuteProcedureDeleteStudent(SqlConnection connection)
    {
        var procedure = "spDeleteStudent";
        var parameters = new { StudentId = "8dfb5815-5fe2-4859-8f77-42403eee4968" };

        var affectedRows = connection.Execute(
            procedure,
            parameters,
            commandType: CommandType.StoredProcedure
        );

        System.Console.WriteLine($"{affectedRows} linhas afetadas");
    }

    static void ExecuteReadProcedureGetCoursesByCategory(SqlConnection connection, string parameters)
    {
        var procedure = $"[spGetCoursesByCategory]";
        var pars = new { CategoryId = parameters };
        var courses = connection.Query(
            procedure, 
            pars,
            commandType: CommandType.StoredProcedure
            );

        foreach(var items in courses)
        {
            System.Console.WriteLine($"{items.Id} - {items.Title}");
        }
    }

    static void ExecuteScalar(SqlConnection connection)
    {
        var category = new Category();
        category.Title = "novaCategory";
        category.Title = "novaCategory";
        category.Title = "novaCategory";
        category.Order = 12;
        category.Description = "novaCat";
        category.Featured = false;

        var insertSqlCategory = @"
            INSERT INTO
                [Category]
            VALUES (
                NEWID(),
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)
            SELECT SCOPE_IDENTITY";

        var categoryId = connection.ExecuteScalar<Guid>(insertSqlCategory, new Category
        {
            Title = category.Title,
            Url = category.Url,
            Summary = category.Summary,
            Order = category.Order,
            Description = category.Description,
            Featured = category.Featured
        });

        System.Console.WriteLine($"A categoria inserida foi {categoryId}");
    }
}