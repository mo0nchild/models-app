using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApp.Dal
{
    internal class ModelDbContextDesign : IDesignTimeDbContextFactory<ModelsDbContext>
    {
        public ModelDbContextDesign() : base() { }
        public ModelsDbContext CreateDbContext(string[] args)
        {
            var contextOptions = new DbContextOptionsBuilder<ModelsDbContext>();
            contextOptions.UseNpgsql("Server=localhost;Port=5432;Username=postgres;Password=prolodgy778;Database=modelsappdb");

            return new ModelsDbContext(contextOptions.Options);
        }
    }
}