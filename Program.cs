using BaltaDataAccess.Models;
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
            // DeleteCategory(connection, "aad38baa-9e37-4374-887d-7b8ff6f00e59");
            // CreateManyCategories(connection);
            // DeleteManyCategories(connection);
            

            // ListStudents(connection);
            // ListCategories(connection);
            // ExecuteProcedure(connection);
            // ExecuteReadProcedureGetCoursesByCategory(connection, "09ce0b7b-cfca-497b-92c0-3290ad9d5142");
            // ExecuteScalar(connection);
            // ReadViewCourses(connection);
            // OneToOne(connection);
            // OneToMany(connection);
            // SelectIn(connection);
            // Like(connection);
            // Transaction(connection);
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
        category.Url = "novaCategory";
        category.Summary = "novaCategory";
        category.Order = 12;
        category.Description = "novaCat";
        category.Featured = false;

        var insertSqlCategory = @"
            INSERT INTO
                [Category]
            OUTPUT inserted.[Id]
            VALUES (
                NEWID(),
                @Title,
                @Url,
                @Summary,
                @Order,
                @Description,
                @Featured)
            SELECT SCOPE_IDENTITY()";

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

    static void ReadViewCourses(SqlConnection connection)
    {
        var sqlQuery = "SELECT * FROM [vwCourses]";
        var courses = connection.Query(sqlQuery);

        foreach(var item in courses)
        {
            System.Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void OneToOne(SqlConnection connection)
    {
        //1. comando sql
        //2. variável pra armazenar retorno da consulta ao banco
        //3. Console.WriteLine para display
        var sql = @"
        SELECT
            *
        FROM
            [CareerItem]
        INNER JOIN
            [Course]
            ON
                [CareerItem].[CourseId] = [Course].[Id]";

        var items = connection.Query<CareerItem, Course, CareerItem>(
            sql,
            (careerItem, course) => {
                careerItem.Course = course;
                return careerItem;
            },
            splitOn: "Id");

        foreach(var item in items)
        {
            System.Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
        }
    }

    static void OneToMany(SqlConnection connection)
    {
        var sql = @"
            SELECT
                [Career].[Id],
                [Career].[Title],
                [CareerItem].[CareerId],
                [CareerItem].[Title]
            FROM
                [Career]
            INNER JOIN
                [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
            ORDER BY
                [Career].[Title]";

        var listCareers = new List<Career>();

        //Query<objetoPai, objetoFilho, resultadFinal=objetoPai>
        var items = connection.Query<Career, CareerItem, Career>(
            sql,
            (career, careerItem) => {
                var carreira = listCareers.Where(x=>x.Id == career.Id).FirstOrDefault();
                if(carreira == null)
                {
                    carreira = career;
                    carreira.Items.Add(careerItem);
                    listCareers.Add(carreira);
                }
                else
                {

                }
                return career;
            },
            splitOn: "CareerId");
        
        foreach(var career in items)
        {
            System.Console.WriteLine(career.Title);
            foreach(var item in career.Items)
            {
                System.Console.WriteLine($" - {item.Title}");
            }
        }
    }

    static void QueryMultiple(SqlConnection connection)
    {
        var query = @"SELECT * FROM [Category]; SELECT * FROM [Course]";

        using(var multi = connection.QueryMultiple(query))
        {
            var categories = multi.Read<Category>();
            var courses = multi.Read<Course>();

            foreach(var item in categories)
            {
                System.Console.WriteLine(item);
            }

            foreach(var item in courses)
            {
                System.Console.WriteLine(item);
            }
        }
    }

    static void SelectIn(SqlConnection connection)
    {
        var sql = @"SELECT * FROM [Career] WHERE [Id] IN (
            '01ae8a85-b4e8-4194-a0f1-1c6190af54cb',
            'e6730d1c-6870-4df3-ae68-438624e04c72'
        )";

        var queryResult = connection.Query<Career>(sql);
        foreach(var item in queryResult)
        {
            System.Console.WriteLine($"{item.Id} - {item.Title}");
        }
    }

    static void Like(SqlConnection connection)
    {
        var term = "backend";
        var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";

        var queryResult = connection.Query<Course>(query, new {
            exp = $"%{term}%"
        });

        foreach(var item in queryResult)
        {
            System.Console.WriteLine(item.Title);
        }
    }

    static void Transaction(SqlConnection connection)
    {
        var newCategory = new Category();
        newCategory.Id = Guid.NewGuid();
        newCategory.Title = "Cat nao quero inserir";
        newCategory.Url = "Url";
        newCategory.Summary = "Summary";
        newCategory.Order = 15;
        newCategory.Description = "Desc";
        newCategory.Featured = false;

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

        connection.Open();
        using(var transaction = connection.BeginTransaction())
        {
            var affectedRows = connection.Execute(
                insertSqlCategory,
                newCategory,
                transaction);


            transaction.Commit();
            // transaction.Rollback();
            System.Console.WriteLine($"{affectedRows} linhas afetadas");
        };
    }
}