using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;

namespace Common
{
    public class JsonHelper
    {
        /// <summary>
        /// JSON序列化(UTF8)
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            return JsonSerializer<T>(t, Encoding.UTF8);
        }

        /// <summary>
        /// JSON反序列化(UTF8)
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            return JsonDeserialize<T>(jsonString, Encoding.UTF8);
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t, Encoding encoding)
        {
            string jsonString = string.Empty;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                jsonString = encoding.GetString(ms.ToArray());
            }

            return jsonString;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString, Encoding encoding)
        {
            T obj = default(T);

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(jsonString)))
            {
                obj = (T)ser.ReadObject(ms);
            }

            return obj;
        }
    }
}
