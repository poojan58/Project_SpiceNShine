using Newtonsoft.Json;
using System.Data;
using System.Reflection;

namespace SpiceAndShine.Models
{
    public static class UserDefineExtensions
    {
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    ExcludedAttribute MyExcluded = (ExcludedAttribute)Attribute.GetCustomAttribute(prop, typeof(ExcludedAttribute));
                    if (MyExcluded == null && (!object.Equals(dr[prop.Name], DBNull.Value)))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        public static T DataReaderMapToEntity<T>(IDataReader dr)
        {
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    ExcludedAttribute MyExcluded = (ExcludedAttribute)Attribute.GetCustomAttribute(prop, typeof(ExcludedAttribute));
                    if (MyExcluded == null && (!object.Equals(dr[prop.Name], DBNull.Value)))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
            }

            return obj;
        }

        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
