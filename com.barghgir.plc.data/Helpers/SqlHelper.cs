using com.barghgir.plc.api.Data;
using com.barghgir.plc.data.Context;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.barghgir.plc.data.Helpers
{
    public class SqlHelper
    {
        public static readonly Dictionary<string, Type> tables = new Dictionary<string, Type>
        {
            { nameof(Course), typeof(Course) },
            { nameof(Content), typeof(Content) }
        };
        
        public static readonly string[] domainNames = new string[] { "Course" };

        public static void SeedTablesFromJsonFiles2(AppDbContext dbContext)
        {
            foreach (string domainName in domainNames)
            {
                MethodInfo? method = typeof(DataHelper).GetMethod(nameof(DataHelper.GetJsonFromFile));
                if (method != null)
                {
                    MethodInfo generic = method.MakeGenericMethod(Type.GetType(domainName) ?? typeof(object));
                    var data = generic.Invoke(null, new[] { $"data/{domainName}.json" });

                    var db = dbContext.Database;
                    //dbContext.Model.GetEntityTypes().Tables("");
                    var serviceCollection = new ServiceCollection()
                        //.AddEntityFrameworkDesignTimeServices()
                        //.AddDbContextDesignTimeServices(db)
                        //.AddEntityFrameworkSqlServer()
                        .AddLogging()
                        //.AddEntityFrameworkDesignTimeServices()
                        .AddSingleton<IDatabaseModelFactory, SqlServerDatabaseModelFactory>()
                        .BuildServiceProvider();

                    var model = serviceCollection.GetRequiredService<IDatabaseModelFactory>();
                    var dbOptions = new DatabaseModelFactoryOptions();
                    var databaseModelFactory = model.Create("", dbOptions);
                    var table = databaseModelFactory.Tables.FirstOrDefault(x => x.Name == domainName);

                    if (table != null)
                    {
                        //table.Add
                    }

                    dbContext.SaveChanges();
                }
            }
        }

        public static async Task SeedTablesFromJsonFiles()
        {
            var dbContext = new CcaDevContext();
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;

            await SeedFromJsonFiles_TableContent(dbContext, baseDir);
            //await SeedFromJsonFiles_TableCourse(dbContext, baseDir);
            //await SeedFromJsonFiles_TableCourseContent(dbContext, baseDir);
        }

        public static async Task SeedFromJsonFiles_TableContent(CcaDevContext dbContext, string baseDir)
        {
            var content = await DataHelper.GetJsonFromFile<Content>(Path.Combine(baseDir, @"seed\Content.json"));
            if (content != null) dbContext.Contents.AddRange(content);
            await SeedFromJsonFiles_SaveChanges(dbContext, "Contents");
        }

        public static async Task SeedFromJsonFiles_TableCourse(CcaDevContext dbContext, string baseDir)
        {
            var courses = await DataHelper.GetJsonFromFile<Course>(Path.Combine(baseDir, @"seed\Courses.json"));
            if (courses != null) dbContext.Courses.AddRange(courses);
            await SeedFromJsonFiles_SaveChanges(dbContext, "Course");
        }

        public static async Task SeedFromJsonFiles_TableCourseContent(CcaDevContext dbContext, string baseDir)
        {
            var courseContent = await DataHelper.GetJsonFromFile<CourseContent>(Path.Combine(baseDir, @"seed\CourseContent.json"));
            if (courseContent != null) dbContext.CourseContents.AddRange(courseContent);
            await SeedFromJsonFiles_SaveChanges(dbContext, "CourseContent");
        }

        public static async Task SeedFromJsonFiles_SaveChanges(CcaDevContext dbContext, string? identityInsertTableName = null)
        {
            if (!string.IsNullOrEmpty(identityInsertTableName))
            {
                dbContext.Database.OpenConnection();
                dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{identityInsertTableName} ON");
            }
            
            await dbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(identityInsertTableName))
            {
                dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{identityInsertTableName} OFF");
                dbContext.Database.CloseConnection();
            }
        }
    }
}
