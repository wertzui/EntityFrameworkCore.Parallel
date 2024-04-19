using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EntityFrameworkCore.Parallel.Internal;

/// <summary>
/// Contains extension methods to find the generic type parameter of a collection type.
/// </summary>
internal static class SharedTypeExtensions
{
    public static Type GetSequenceType(this Type type)
    {
        var sequenceType = TryGetSequenceType(type) ?? throw new ArgumentException("The given type is not a collection type", nameof(type));

        return sequenceType;
    }

    public static Type? TryGetSequenceType(this Type type)
        => type.TryGetElementType(typeof(IEnumerable<>))
            ?? type.TryGetElementType(typeof(IAsyncEnumerable<>));

    public static Type? TryGetElementType(this Type type, Type interfaceOrBaseType)
    {
        if (type.IsGenericTypeDefinition)
        {
            return null;
        }

        var types = GetGenericTypeImplementations(type, interfaceOrBaseType);

        Type? singleImplementation = null;
        foreach (var implementation in types)
        {
            if (singleImplementation == null)
            {
                singleImplementation = implementation;
            }
            else
            {
                singleImplementation = null;
                break;
            }
        }

        return singleImplementation?.GenericTypeArguments.FirstOrDefault();
    }

    public static IEnumerable<Type> GetGenericTypeImplementations(this Type type, Type interfaceOrBaseType)
    {
        var typeInfo = type.GetTypeInfo();
        if (!typeInfo.IsGenericTypeDefinition)
        {
            var baseTypes = interfaceOrBaseType.GetTypeInfo().IsInterface
                ? typeInfo.ImplementedInterfaces
                : type.GetBaseTypes();
            foreach (var baseType in baseTypes)
            {
                if (baseType.IsGenericType
                    && baseType.GetGenericTypeDefinition() == interfaceOrBaseType)
                {
                    yield return baseType;
                }
            }

            if (type.IsGenericType
                && type.GetGenericTypeDefinition() == interfaceOrBaseType)
            {
                yield return type;
            }
        }
    }

    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
        var baseType = type.BaseType;

        while (baseType is not null)
        {
            yield return baseType;

            baseType = baseType.BaseType;
        }
    }
}
