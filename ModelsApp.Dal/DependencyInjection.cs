using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal
{
    public static class DependencyInjection : object
    {
        public static readonly string DatabaseName = "modelsappdb";
        public async static Task<IServiceCollection> AddDatabase(this IServiceCollection collection, IConfiguration configuration)
        {
            await Console.Out.WriteLineAsync($"DB: {configuration.GetConnectionString(DatabaseName)}");
            collection.AddDbContextFactory<ModelsDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(DatabaseName));
            });
            var serviceProvider = collection.BuildServiceProvider();
            var dbcontextFactory = serviceProvider.GetService<IDbContextFactory<ModelsDbContext>>()!;

            using (var dbcontext = await dbcontextFactory.CreateDbContextAsync())
            {
                await dbcontext.Database.MigrateAsync();
            }
            return collection;
        }
    }
}
