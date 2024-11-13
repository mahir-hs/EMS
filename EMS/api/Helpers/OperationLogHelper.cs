using System.Text.Json;

namespace api.Helpers
{
    public static class OperationLogHelper
    {
        public static string CreateInsertDetails<T>(T entity)
        {
            return JsonSerializer.Serialize(new
            {
                Action = "Insert",
                Data = entity
            });
        }

        public static string CreateUpdateDetails<T>(T oldEntity, T newEntity)
        {
            var changes = new List<object>();

            foreach (var property in typeof(T).GetProperties())
            {
                var oldValue = property.GetValue(oldEntity)?.ToString();
                var newValue = property.GetValue(newEntity)?.ToString();

                if (oldValue != newValue)
                {
                    changes.Add(new
                    {
                        Property = property.Name,
                        OldValue = oldValue,
                        NewValue = newValue
                    });
                }
            }

            return JsonSerializer.Serialize(new
            {
                Action = "Update",
                Changes = changes
            });
        }

        public static string CreateDeleteDetails<T>(T entity)
        {
            return JsonSerializer.Serialize(new
            {
                Action = "Delete",
                Data = entity
            });
        }
    }
}
