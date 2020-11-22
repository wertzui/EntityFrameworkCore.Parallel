using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace EntitiyFrameworkCore.Parallel.Internal
{
    /// <summary>
    /// This is a minimal implementation of <see cref="IEntityType"/> to support the creation of queries without the need of a DbContext.
    /// Most of its methods will just throw exceptions.
    /// When the query is actually executed, the expression which is started with this class is replaced to contain an instance of the implementation from the real Entity Framework library.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities in the DbSet.</typeparam>
    public class EntityType<TEntity> : IEntityType
    {
        /// <inheritdoc/>
        public object this[string name] => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEntityType BaseType => throw new NotImplementedException();

        /// <inheritdoc/>
        public string DefiningNavigationName => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEntityType DefiningEntityType => throw new NotImplementedException();

        /// <inheritdoc/>
        public IModel Model => throw new NotImplementedException();

        /// <inheritdoc/>
        public string Name => throw new NotImplementedException();

        /// <inheritdoc/>
        public Type ClrType => typeof(TEntity);

        /// <inheritdoc/>
        public bool HasSharedClrType => throw new NotImplementedException();

        /// <inheritdoc/>
        public bool IsPropertyBag => throw new NotImplementedException();

        /// <inheritdoc/>
        public IAnnotation FindAnnotation(string name) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IForeignKey FindForeignKey(IReadOnlyList<IProperty> properties, IKey principalKey, IEntityType principalEntityType) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IIndex FindIndex(IReadOnlyList<IProperty> properties) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IIndex FindIndex(string name) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IKey FindKey(IReadOnlyList<IProperty> properties) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IKey FindPrimaryKey() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IProperty FindProperty(string name) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IServiceProperty FindServiceProperty(string name) => throw new NotImplementedException();

        /// <inheritdoc/>
        public ISkipNavigation FindSkipNavigation(string name) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<IAnnotation> GetAnnotations() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<IForeignKey> GetForeignKeys() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<IIndex> GetIndexes() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<IKey> GetKeys() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<IProperty> GetProperties() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<IServiceProperty> GetServiceProperties() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IEnumerable<ISkipNavigation> GetSkipNavigations() => throw new NotImplementedException();
    }
}