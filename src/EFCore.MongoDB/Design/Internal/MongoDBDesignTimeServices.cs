using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace DEF.EntityFrameworkCore.MongoDB.Design.Internal
{
    public class MongoDBDesignTimeServices : IDesignTimeServices
    {
        // TODO: .AddSingleton<ITypeMappingSource, MongoDBTypeMappingSource>()
        // TODO: .AddSingleton<IDatabaseModelFactory, MongoDBDatabaseModelFactory>()
        // TODO: .AddSingleton<IProviderConfigurationCodeGenerator, MongoDBCodeGenerator>()
        // TODO: .AddSingleton<IAnnotationCodeGenerator, AnnotationCodeGenerator>();
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection) 
            => serviceCollection;
    }
}
