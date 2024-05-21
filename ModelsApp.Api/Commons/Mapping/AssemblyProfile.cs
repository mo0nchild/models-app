using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using AutoMapper;

namespace ModelsApp.Api.Commons.Mapping
{
    public partial class AssemblyProfile : Profile
    {
        public ILogger<AssemblyProfile> Logger { get; private set; } = default!;
        public AssemblyProfile(Assembly assembly) : base()
        {
            this.Logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<AssemblyProfile>();
            this.ConfigureProfile(assembly);
        }

        protected virtual void ConfigureProfile(Assembly assembly)
        {
            var assemblyTypes = assembly.GetExportedTypes();
            var mappingTypes = assemblyTypes.Where(item => item.GetInterfaces()
                .Any(item => item.IsGenericType && item.GetGenericTypeDefinition() == typeof(IMappingTarget<>)));

            foreach (var mapper in mappingTypes)
            {
                var typeInstance = Activator.CreateInstance(mapper);
                var @interface = mapper.GetInterface(typeof(IMappingTarget<>).Name);

                this.Logger.LogInformation($"Mapping type: {mapper.FullName}");
                @interface?.GetMethod("ConfigureMapping")?.Invoke(typeInstance, new[] { this });
            }
        }
    }
}
