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
        public object this[string name] => throw new NotImplementedException();

        public IEntityType BaseType => throw new NotImplementedException();

        public string DefiningNavigationName => throw new NotImplementedException();

        public IEntityType DefiningEntityType => throw new NotImplementedException();

        public IModel Model => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public Type ClrType => typeof(TEntity);

        public bool HasSharedClrType => throw new NotImplementedException();

        public bool IsPropertyBag => throw new NotImplementedException();

        public IAnnotation FindAnnotation(string name) => throw new NotImplementedException();
        public IForeignKey FindForeignKey(IReadOnlyList<IProperty> properties, IKey principalKey, IEntityType principalEntityType) => throw new NotImplementedException();
        public IIndex FindIndex(IReadOnlyList<IProperty> properties) => throw new NotImplementedException();
        public IIndex FindIndex(string name) => throw new NotImplementedException();
        public IKey FindKey(IReadOnlyList<IProperty> properties) => throw new NotImplementedException();
        public IKey FindPrimaryKey() => throw new NotImplementedException();
        public IProperty FindProperty(string name) => throw new NotImplementedException();
        public IServiceProperty FindServiceProperty(string name) => throw new NotImplementedException();
        public ISkipNavigation FindSkipNavigation(string name) => throw new NotImplementedException();
        public IEnumerable<IAnnotation> GetAnnotations() => throw new NotImplementedException();
        public IEnumerable<IForeignKey> GetForeignKeys() => throw new NotImplementedException();
        public IEnumerable<IIndex> GetIndexes() => throw new NotImplementedException();
        public IEnumerable<IKey> GetKeys() => throw new NotImplementedException();
        public IEnumerable<IProperty> GetProperties() => throw new NotImplementedException();
        public IEnumerable<IServiceProperty> GetServiceProperties() => throw new NotImplementedException();
        public IEnumerable<ISkipNavigation> GetSkipNavigations() => throw new NotImplementedException();
    }
}
