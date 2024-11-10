using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.Set(key, System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
        }

        public static T Get<T>(this ISession session, string key)
        {
            byte[] value;
            if (session.TryGetValue(key, out value))
            {
                string json = System.Text.Encoding.UTF8.GetString(value);
                return JsonSerializer.Deserialize<T>(json);
            }
            return default;
        }

    }
}
