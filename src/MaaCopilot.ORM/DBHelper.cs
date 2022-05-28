using MaaCopilot.Interfaces.ORM;
using System.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaaCopilot.ORM
{
    public class DBHelper : IDBHelper
    {
        protected const string prefix = "@";

        public string GenerateColumns<T>(string[] ignoreColumns, bool update = false)
        {
            var properties = GetProperties<T>();
            return string.Join(",", properties.Where(p => !ignoreColumns.Contains(p.Name)).Select(x => $"{(update ? (x.Name + "=" + prefix) : "")}{ x.Name}"));
        }

        public string GenerateParams<T>(string[] ignoreColumns, bool update = false)
        {
            if (update)
            {
                foreach (var property in typeof(T).GetProperties().AsEnumerable().Where(p => p.GetCustomAttribute<KeyAttribute>() != null || p.GetCustomAttribute<ForeignKeyAttribute>() != null))
                {
                    var value = typeof(T).GetProperty(property.Name)?.GetCustomAttribute<DatabaseGeneratedAttribute>();
                    if (value?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                    {
                        return $@"{property.Name}=@{property.Name}";
                    }
                }
                return "";
            }
            else
            {
                var properties = GetProperties<T>();
                return string.Join(",", properties.Where(p => !ignoreColumns.Contains(p.Name)).Select(x => $"{prefix + x.Name}"));
            }
        }

        public IEnumerable<PropertyInfo> GetProperties<T>()
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(p => p.MetadataToken);
            return properties.Where(p => p.PropertyType.GetTypeInfo().IsValueType || p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(string)).ToArray();
        }
    }
}