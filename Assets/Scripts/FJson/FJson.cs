using System;
using System.Reflection;
using FJson.Core;

namespace FJson
{
    public class FJsonUtility
    {
        public static string ToJson(object value)
        {
            return null;
        }

        public static T ToObject<T>(string json)
        {
            var jsonObject = new JsonParser().ParseJsonObject(json);
            if (jsonObject.Count > 0)
            {
                Type mType = typeof(T);
                object obj = mType.Assembly.CreateInstance(mType.Name);
                foreach (var mJsonObjectKey in jsonObject.Keys)
                {
                    var mProperty = mType.GetProperty(mJsonObjectKey);
                    object value = Convert.ChangeType(jsonObject[mJsonObjectKey], mType.GetProperty(mJsonObjectKey).PropertyType);
                    SetValue(mProperty , value , ref obj);
                }

                return (T)obj;
            }
            return default;
        }

        private static void SetValue(PropertyInfo rPropertyInfo , object value , ref object obj)
        {
            if (rPropertyInfo.GetType() == value.GetType())
            {
                rPropertyInfo.SetValue(obj , value);
            }
        }
    }
}