using System;
using System.Reflection;
using FJson.Core;
using UnityEngine;

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
            if (jsonObject != null)
            {
                return (T)jsonObject.ToObject(typeof(T));
            }
            return default;
        }
        
        
    }
}