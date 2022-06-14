using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcDemo.Data;

public static class DataExtensions
{
    public static DbCommand CreateCommand(this IDbTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));


        var cmd = transaction.Connection!.CreateCommand();
        cmd.Transaction = transaction;
        return (DbCommand) cmd;
    }

    public static IDataParameter AddParameter(this IDbCommand command, string name, object value)
    {
        var p = command.CreateParameter();
        p.ParameterName = name;
        p.Value = value;
        command.Parameters.Add(p);
        return p;
    }

    public static void SetProperty<TSource, TProperty>(this TSource source,
        Expression<Func<TSource, TProperty>> propertyLambda,
        object? value)
    {
        var propInfo = source.GetPropertyInfo(propertyLambda);
        if (propInfo.CanWrite)
        {
            propInfo.SetValue(source, value);
            return;
        }

        var field = typeof(TSource)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(x =>
                x.Name.Contains($"{propInfo.Name}__BackingField", StringComparison.OrdinalIgnoreCase));
        if (field == null)
        {
            throw new InvalidOperationException("Failed to find field for " + propInfo.Name);
        }

        field.SetValue(source, value);
    }

    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(
        this TSource _,
        Expression<Func<TSource, TProperty>> propertyLambda)
    {
        var type = typeof(TSource);

        if (propertyLambda.Body is not MemberExpression member)
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
        }

        var propInfo = member.Member as PropertyInfo;
        if (propInfo == null)
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
        }

        if (type != propInfo.ReflectedType &&
            propInfo.ReflectedType != null &&
            !type.IsSubclassOf(propInfo.ReflectedType))
            throw new ArgumentException(
                $"Expression '{propertyLambda}' refers to a property that is not from type {type}.");

        return propInfo;
    }

    public static string GetString(this IDataReader reader, string name)
    {
        var value = reader[name];
        if (value is DBNull)
            throw new InvalidOperationException($"Field {name} was DbNull.");

        return (string) value;
    }

    public static string? GetStringNullable(this IDataReader reader, string name)
    {
        var value = reader[name];
        if (value is DBNull)
            return null;

        return (string)value;
    }

    public static int GetInt(this IDataReader reader, string name)
    {
        var value = reader[name];
        if (value is DBNull)
            throw new InvalidOperationException($"Field {name} was DbNull.");

        return (int)value;
    }
    public static DateTime? GetDateTimeNullable(this IDataReader reader, string name)
    {
        var value = reader[name];
        if (value is DBNull)
            return null;

        return (DateTime?)value;
    }

    public static DateTime GetDateTime(this IDataReader reader, string name)
    {
        var value = reader[name];
        if (value is DBNull)
            throw new InvalidOperationException($"Field {name} was DbNull.");

        return (DateTime)value;
    }
}