using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace LegacyLoom.Tests
{
    public class Sorting<User>
    {
        public IQueryable<User> ApplySort(IQueryable<User> entities, string? orderByQueryString)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            Console.WriteLine($"OderByQueryString: {orderByQueryString}");

            if (!entities.Any() || string.IsNullOrWhiteSpace(orderByQueryString))
                return entities;

            var orderParams = orderByQueryString.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries);
            var propertyInfos = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                var trimmedParam = param.Trim();
                if (string.IsNullOrWhiteSpace(trimmedParam))
                    continue;

                var parts = trimmedParam.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                    continue;

                var propertyName = parts[0];
                Console.WriteLine($"Param1: {propertyName}");
                Console.WriteLine($"Param2: {parts[1]}");
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

                if (objectProperty == null)
                {
                    // Optionally throw an exception for invalid properties
                    throw new ArgumentException($"Property '{propertyName}' does not exist on type '{typeof(User).Name}'.");
                }

                var sortingOrder = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? "descending"
                    : "ascending";
                Console.WriteLine($"Sorting Order: {sortingOrder}");

                orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrEmpty(orderQuery))
            {
                return entities; // No valid sort parameters
            }

            Console.WriteLine($"Order Query: {orderQuery}");
            return entities.OrderBy(orderQuery);
        }
    }
}
