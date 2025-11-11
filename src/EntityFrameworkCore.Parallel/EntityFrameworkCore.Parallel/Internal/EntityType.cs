using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EntityFrameworkCore.Parallel.Internal;

/// <summary>
/// This is a minimal implementation of <see cref="IEntityType"/> to support the creation of
/// queries without the need of a DbContext. Most of its methods will just throw exceptions.
/// When the query is actually executed, the expression which is started with this class is
/// replaced to contain an instance of the implementation from the real Entity Framework library.
/// </summary>
/// <typeparam name="TEntity">The type of the entities in the DbSet.</typeparam>
public class EntityType<TEntity> : IEntityType
{
    /// <inheritdoc/>
    public IEntityType? BaseType => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyEntityType? IReadOnlyEntityType.BaseType => throw new NotImplementedException();

    /// <inheritdoc/>
    public Type ClrType => typeof(TEntity);

    /// <inheritdoc/>
    public InstantiationBinding? ConstructorBinding => throw new NotImplementedException();

    /// <inheritdoc/>
    public bool HasSharedClrType => throw new NotImplementedException();

    /// <inheritdoc/>
    public bool IsPropertyBag => throw new NotImplementedException();

    /// <inheritdoc/>
    public IModel Model => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyModel IReadOnlyTypeBase.Model => throw new NotImplementedException();

    /// <inheritdoc/>
    public string Name => throw new NotImplementedException();

    /// <inheritdoc/>
    public InstantiationBinding? ServiceOnlyConstructorBinding => throw new NotImplementedException();

    ITypeBase? ITypeBase.BaseType => throw new NotImplementedException();

    IReadOnlyTypeBase? IReadOnlyTypeBase.BaseType => throw new NotImplementedException();

    /// <inheritdoc/>
    public object? this[string name] => throw new NotImplementedException();

    /// <inheritdoc/>
    public IAnnotation AddRuntimeAnnotation(string name, object? value) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IAnnotation? FindAnnotation(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IForeignKey> FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    public INavigation? FindDeclaredNavigation(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyNavigation? IReadOnlyEntityType.FindDeclaredNavigation(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IProperty? FindDeclaredProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public ITrigger? FindDeclaredTrigger(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyTrigger? IReadOnlyEntityType.FindDeclaredTrigger(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IForeignKey? FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyForeignKey? IReadOnlyEntityType.FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IForeignKey> FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IIndex? FindIndex(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IIndex? FindIndex(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyIndex? IReadOnlyEntityType.FindIndex(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyIndex? IReadOnlyEntityType.FindIndex(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public PropertyInfo? FindIndexerPropertyInfo() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IKey? FindKey(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyKey? IReadOnlyEntityType.FindKey(IReadOnlyList<IReadOnlyProperty> properties) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IKey? FindPrimaryKey() => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyKey? IReadOnlyEntityType.FindPrimaryKey() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IReadOnlyList<IReadOnlyProperty>? FindProperties(IReadOnlyList<string> propertyNames) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IProperty? FindProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IAnnotation? FindRuntimeAnnotation(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IServiceProperty? FindServiceProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyServiceProperty? IReadOnlyEntityType.FindServiceProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public ISkipNavigation? FindSkipNavigation(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlySkipNavigation? IReadOnlyEntityType.FindSkipNavigation(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IAnnotation> GetAnnotations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public ChangeTrackingStrategy GetChangeTrackingStrategy() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IForeignKey> GetDeclaredForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IIndex> GetDeclaredIndexes() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDeclaredIndexes() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IKey> GetDeclaredKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetDeclaredKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<INavigation> GetDeclaredNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetDeclaredNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IProperty> GetDeclaredProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IForeignKey> GetDeclaredReferencingForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredReferencingForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IServiceProperty> GetDeclaredServiceProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetDeclaredServiceProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IReadOnlySkipNavigation> GetDeclaredSkipNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<ITrigger> GetDeclaredTriggers() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyTrigger> IReadOnlyEntityType.GetDeclaredTriggers() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IForeignKey> GetDerivedForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDerivedForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IIndex> GetDerivedIndexes() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDerivedIndexes() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IReadOnlyNavigation> GetDerivedNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IReadOnlyProperty> GetDerivedProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IReadOnlyServiceProperty> GetDerivedServiceProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IReadOnlySkipNavigation> GetDerivedSkipNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IReadOnlyEntityType> GetDerivedTypes() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IEntityType> GetDirectlyDerivedTypes() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyEntityType> IReadOnlyEntityType.GetDirectlyDerivedTypes() => throw new NotImplementedException();

    /// <inheritdoc/>
    public string? GetDiscriminatorPropertyName() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IProperty> GetForeignKeyProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IForeignKey> GetForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IIndex> GetIndexes() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetIndexes() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IKey> GetKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    public PropertyAccessMode GetNavigationAccessMode() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<INavigation> GetNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public TValue GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg?, TValue> valueFactory, TArg? factoryArgument) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IProperty> GetProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public PropertyAccessMode GetPropertyAccessMode() => throw new NotImplementedException();

    /// <inheritdoc/>
    public LambdaExpression? GetQueryFilter() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IForeignKey> GetReferencingForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetReferencingForeignKeys() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IAnnotation> GetRuntimeAnnotations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IDictionary<string, object?>> GetSeedData(bool providerValues = false) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IServiceProperty> GetServiceProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetServiceProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<ISkipNavigation> GetSkipNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetSkipNavigations() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IProperty> GetValueGeneratingProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IAnnotation? RemoveRuntimeAnnotation(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IAnnotation SetRuntimeAnnotation(string name, object? value) => throw new NotImplementedException();

    /// <inheritdoc/>
    public bool HasServiceProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IComplexProperty? FindComplexProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IComplexProperty> GetComplexProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IComplexProperty> GetDeclaredComplexProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IPropertyBase> GetMembers() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IPropertyBase> GetDeclaredMembers() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IPropertyBase? FindMember(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IPropertyBase> FindMembersInHierarchy(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IPropertyBase> GetSnapshottableMembers() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IProperty> GetFlattenedProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IComplexProperty> GetFlattenedComplexProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IProperty> GetFlattenedDeclaredProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyProperty? IReadOnlyTypeBase.FindProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyProperty? IReadOnlyTypeBase.FindDeclaredProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyProperty> IReadOnlyTypeBase.GetDeclaredProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyProperty> IReadOnlyTypeBase.GetProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyComplexProperty? IReadOnlyTypeBase.FindComplexProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IReadOnlyComplexProperty? FindDeclaredComplexProperty(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyComplexProperty> IReadOnlyTypeBase.GetComplexProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyComplexProperty> IReadOnlyTypeBase.GetDeclaredComplexProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IReadOnlyComplexProperty> GetDerivedComplexProperties() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyPropertyBase> IReadOnlyTypeBase.GetMembers() => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyPropertyBase> IReadOnlyTypeBase.GetDeclaredMembers() => throw new NotImplementedException();

    /// <inheritdoc/>
    IReadOnlyPropertyBase? IReadOnlyTypeBase.FindMember(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    IEnumerable<IReadOnlyPropertyBase> IReadOnlyTypeBase.FindMembersInHierarchy(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IReadOnlyCollection<IQueryFilter> GetDeclaredQueryFilters() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IQueryFilter? FindDeclaredQueryFilter(string? filterKey) => throw new NotImplementedException();

    IEnumerable<ITypeBase> ITypeBase.GetDirectlyDerivedTypes() => throw new NotImplementedException();

    /// <inheritdoc/>
    public IEnumerable<IProperty> GetFlattenedValueGeneratingProperties() => throw new NotImplementedException();

    IEnumerable<IReadOnlyTypeBase> IReadOnlyTypeBase.GetDerivedTypes() => throw new NotImplementedException();

    IEnumerable<IReadOnlyTypeBase> IReadOnlyTypeBase.GetDirectlyDerivedTypes() => throw new NotImplementedException();

    /// <inheritdoc/>
    public Func<MaterializationContext, object> GetOrCreateMaterializer(IStructuralTypeMaterializerSource source) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Func<MaterializationContext, object> GetOrCreateEmptyMaterializer(IStructuralTypeMaterializerSource source) => throw new NotImplementedException();
}