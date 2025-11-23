using System.Linq.Expressions;
using System.Reflection;

namespace FastTrack.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortColumn, string sortDirection = "ASC")
        {
            ArgumentNullException.ThrowIfNull(source);
            if (string.IsNullOrWhiteSpace(sortColumn)) return source;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression propertyExpression = parameter;
            Type propertyType = typeof(T);

            string[] parts = sortColumn.Split('.');
            foreach (var part in parts)
            {
                PropertyInfo? prop = propertyType.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                {
                    return source;
                }

                propertyExpression = Expression.Property(propertyExpression, prop);
                propertyType = prop.PropertyType;
            }

            bool alreadyOrdered = false;
            if (source.Expression is MethodCallExpression m)
            {
                string name = m.Method.Name;
                alreadyOrdered = name.StartsWith("OrderBy", StringComparison.Ordinal) || name.StartsWith("ThenBy", StringComparison.Ordinal);
            }

            bool desc = string.Equals(sortDirection, "DESC", StringComparison.OrdinalIgnoreCase);
            string methodName = alreadyOrdered
                ? (desc ? "ThenByDescending" : "ThenBy")
                : (desc ? "OrderByDescending" : "OrderBy");

            LambdaExpression lambda = Expression.Lambda(propertyExpression, parameter);

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), propertyType },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<T>(call);
        }

        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> source, IDictionary<string, string>? filters)
        {
            if (filters == null || filters.Count == 0) return source;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            Expression? finalExpr = null;

            foreach (var kvp in filters)
            {
                string key = kvp.Key?.Trim();
                string? value = kvp.Value;
                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                    continue;

                // detect operators for ranges: suffixes _from, _to, _gt, _lt
                string keyLower = key.ToLowerInvariant();
                string op = "eq"; // eq, gte, lte, gt, lt
                if (keyLower.EndsWith("_from"))
                {
                    op = "gte";
                    key = key.Substring(0, key.Length - 5);
                }
                else if (keyLower.EndsWith("_to"))
                {
                    op = "lte";
                    key = key.Substring(0, key.Length - 3);
                }
                else if (keyLower.EndsWith("_gt"))
                {
                    op = "gt";
                    key = key.Substring(0, key.Length - 3);
                }
                else if (keyLower.EndsWith("_lt"))
                {
                    op = "lt";
                    key = key.Substring(0, key.Length - 3);
                }

                // Navigate nested properties (dot notation)
                Expression propertyExpression = parameter;
                Type propertyType = typeof(T);
                PropertyInfo? propInfo = null;
                string[] parts = key.Split('.');
                bool missing = false;
                foreach (var part in parts)
                {
                    propInfo = propertyType.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo == null)
                    {
                        missing = true;
                        break;
                    }
                    propertyExpression = Expression.Property(propertyExpression, propInfo);
                    propertyType = propInfo.PropertyType;
                }

                if (missing || propInfo == null)
                    continue;

                Type targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                Expression? expr = null;

                if (targetType == typeof(string))
                {
                    // Case-insensitive Contains: x.Prop != null && x.Prop.ToLower().Contains(value.ToLower())
                    MethodInfo toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
                    MethodInfo contains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) })!;

                    Expression notNull = Expression.NotEqual(propertyExpression, Expression.Constant(null, propertyExpression.Type));
                    Expression leftToLower = Expression.Call(propertyExpression, toLower);
                    Expression rightConst = Expression.Constant(value.ToLower());
                    Expression containsCall = Expression.Call(leftToLower, contains, rightConst);
                    expr = Expression.AndAlso(notNull, containsCall);
                }
                else
                {
                    object? converted = null;
                    try
                    {
                        if (targetType.IsEnum)
                        {
                            converted = Enum.Parse(targetType, value, true);
                        }
                        else if (targetType == typeof(Guid))
                        {
                            converted = Guid.Parse(value);
                        }
                        else if (targetType == typeof(DateTime))
                        {
                            // try parse DateTime explicitly to support different formats
                            converted = DateTime.Parse(value);
                        }
                        else
                        {
                            converted = Convert.ChangeType(value, targetType);
                        }
                    }
                    catch
                    {
                        // Skip filter if cannot convert
                        continue;
                    }

                    Expression constant = Expression.Constant(converted, targetType);

                    // If property is nullable, convert constant to nullable type
                    if (propertyExpression.Type != targetType)
                    {
                        constant = Expression.Convert(constant, propertyExpression.Type);
                    }

                    // build comparison expression based on operator
                    switch (op)
                    {
                        case "gte":
                            expr = Expression.GreaterThanOrEqual(propertyExpression, constant);
                            break;
                        case "lte":
                            expr = Expression.LessThanOrEqual(propertyExpression, constant);
                            break;
                        case "gt":
                            expr = Expression.GreaterThan(propertyExpression, constant);
                            break;
                        case "lt":
                            expr = Expression.LessThan(propertyExpression, constant);
                            break;
                        default:
                            expr = Expression.Equal(propertyExpression, constant);
                            break;
                    }
                }

                if (expr == null) continue;

                finalExpr = finalExpr == null ? expr : Expression.AndAlso(finalExpr, expr);
            }

            if (finalExpr == null) return source;

            var lambda = Expression.Lambda(finalExpr, parameter);
            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { typeof(T) },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<T>(call);
        }
    }
}
