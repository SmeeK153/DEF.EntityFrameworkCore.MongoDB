// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.MongoDB.Extensions
{
    /// <summary>
    ///     MongoDb-specific extension methods for <see cref="IServiceCollection" />.
    /// </summary>
    public static class MongoDbServiceCollectionExtensions
    {
        /// <summary>
        ///     <para>
        ///         Adds the services required by the MongoDb database provider for Entity Framework
        ///         to an <see cref="IServiceCollection" />.
        ///     </para>
        ///     <para>
        ///         Calling this method is no longer necessary when building most applications, including those that
        ///         use dependency injection in ASP.NET or elsewhere.
        ///         It is only needed when building the internal service provider for use with
        ///         the <see cref="DbContextOptionsBuilder.UseInternalServiceProvider"/> method.
        ///         This is not recommend other than for some advanced scenarios.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static IServiceCollection AddEntityFrameworkMongoDb([NotNull] this IServiceCollection serviceCollection)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            var builder = new EntityFrameworkServicesBuilder(serviceCollection)
                .TryAdd<LoggingDefinitions, MongoDbLoggingDefinitions>()
                .TryAdd<IDatabaseProvider, DatabaseProvider<MongoDbOptionsExtension>>()
                .TryAdd<IDatabase, MongoDbDatabaseWrapper>()
                .TryAdd<IExecutionStrategyFactory, MongoDbExecutionStrategyFactory>()
                .TryAdd<IDbContextTransactionManager, MongoDbTransactionManager>()
                .TryAdd<IModelValidator, MongoDbModelValidator>()
                .TryAdd<IProviderConventionSetBuilder, MongoDbConventionSetBuilder>()
                .TryAdd<IDatabaseCreator, MongoDbDatabaseCreator>()
                .TryAdd<IQueryContextFactory, MongoDbQueryContextFactory>()
                .TryAdd<ITypeMappingSource, MongoDbTypeMappingSource>()

                // New Query pipeline
                .TryAdd<IQueryableMethodTranslatingExpressionVisitorFactory, MongoDbQueryableMethodTranslatingExpressionVisitorFactory>()
                .TryAdd<IShapedQueryCompilingExpressionVisitorFactory, MongoDbShapedQueryCompilingExpressionVisitorFactory>()

                .TryAdd<ISingletonOptions, IMongoDbSingletonOptions>(p => p.GetService<IMongoDbSingletonOptions>())
                .TryAddProviderSpecificServices(
                    b => b
                        .TryAddSingleton<IMongoDbSingletonOptions, MongoDbSingletonOptions>()
                        .TryAddSingleton<SingletonMongoDbClientWrapper, SingletonMongoDbClientWrapper>()
                        //.TryAddSingleton<ISqlExpressionFactory, SqlExpressionFactory>()                   // Not sure yet why this is needed in a non-relational provider, leaving in just in-case
                        //.TryAddSingleton<IQuerySqlGeneratorFactory, QuerySqlGeneratorFactory>()
                        .TryAddSingleton<IMethodCallTranslatorProvider, MongoDbMethodCallTranslatorProvider>()
                        .TryAddSingleton<IMemberTranslatorProvider, MongoDbMemberTranslatorProvider>()
                        .TryAddScoped<MongoDbClientWrapper, MongoDbClientWrapper>()
                );

            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}
