// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using EFCore.MongoDB.Infrastructure;
using EFCore.MongoDB.Infrastructure.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace EFCore.MongoDB.Extensions
{
    /// <summary>
    ///     MongoDb-specific extension methods for <see cref="DbContextOptionsBuilder" />.
    /// </summary>
    public static class MongoDbContextOptionsExtensions
    {
        /// <summary>
        ///     Configures the context to connect to a MongoDb database.
        /// </summary>
        /// <typeparam name="TContext"> The type of context to be configured. </typeparam>
        /// <param name="optionsBuilder"> The builder being used to configure the context. </param>
        /// <param name="accountEndpoint"> The account end-point to connect to. </param>
        /// <param name="accountKey"> The account key. </param>
        /// <param name="databaseName"> The database name. </param>
        /// <param name="MongoDbOptionsAction">An optional action to allow additional MongoDb-specific configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder<TContext> UseMongoDb<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [NotNull] string accountEndpoint,
            [NotNull] string accountKey,
            [NotNull] string databaseName,
            [CanBeNull] Action<MongoDbContextOptionsBuilder> MongoDbOptionsAction = null)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)UseMongoDb(
                (DbContextOptionsBuilder)optionsBuilder,
                accountEndpoint,
                accountKey,
                databaseName,
                MongoDbOptionsAction);

        /// <summary>
        ///     Configures the context to connect to a MongoDb database.
        /// </summary>
        /// <param name="optionsBuilder"> The builder being used to configure the context. </param>
        /// <param name="accountEndpoint"> The account end-point to connect to. </param>
        /// <param name="accountKey"> The account key. </param>
        /// <param name="databaseName"> The database name. </param>
        /// <param name="MongoDbOptionsAction">An optional action to allow additional MongoDb-specific configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseMongoDb(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [NotNull] string accountEndpoint,
            [NotNull] string accountKey,
            [NotNull] string databaseName,
            [CanBeNull] Action<MongoDbContextOptionsBuilder> MongoDbOptionsAction = null)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));
            Check.NotNull(accountEndpoint, nameof(accountEndpoint));
            Check.NotEmpty(accountKey, nameof(accountKey));
            Check.NotEmpty(databaseName, nameof(databaseName));

            var extension = optionsBuilder.Options.FindExtension<MongoDbOptionsExtension>()
                            ?? new MongoDbOptionsExtension();

            extension = extension
                .WithAccountEndpoint(accountEndpoint)
                .WithAccountKey(accountKey)
                .WithDatabaseName(databaseName);

            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            MongoDbOptionsAction?.Invoke(new MongoDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }
    }
}
