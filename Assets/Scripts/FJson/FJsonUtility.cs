using System;
using System.Collections;
using System.Reflection;
using FJson.Core;

namespace FJson
{
    public class FJsonUtility
    {

        public static BindingFlags SerializationAttr = BindingFlags.Instance | BindingFlags.Public;
        public static BindingFlags DeserializationAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        
        public static string ToJson(object value)
        {
            IJsonDemoder mJsonDemoder = null;
            if (value is IJsonDemoder)
            {
                mJsonDemoder = (IJsonDemoder)value;
            }
            else if (IsArray(value))
                mJsonDemoder = CreateJsonArray(value);
            else if (IsObject(value))
                mJsonDemoder = CreateJsonObject(value);
            return mJsonDemoder != null ? mJsonDemoder.Serialization() : value.ToString();
        }

        public static T ToObject<T>(string json)
        {
            var jsonObject = new JsonParser().ParseJson(json);
            if (typeof(IJsonDemoder).IsAssignableFrom(typeof(T)))
            {
                return (T)jsonObject;
            }
            if (jsonObject != null)
            {
                return (T)jsonObject.Deserialization(typeof(T));
            }
            return default;
        }

        public static JsonObject ToObject(string json) { return (JsonObject)new JsonParser().ParseJson(json); }

        public static T Convert<T>(object value)
        {
            IJsonDemoder mJsonDemoder = null;
            if (IsArray(value))
                mJsonDemoder = CreateJsonArray(value);
            else if (IsObject(value))
                mJsonDemoder = CreateJsonObject(value);
            
            return (T)mJsonDemoder?.Deserialization(typeof(T));
        }

        /// <summary>
        /// 将json字符串转换为中间类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IJsonDemoder ToJsonDemoderJson(string json)
        {
            return new JsonParser().ParseJson(json);
        }
        
        /// <summary>
        /// 将任意object对象转换为中间类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IJsonDemoder ToJsonDemoder(object value)
        {
            IJsonDemoder mJsonDemoder = null;
            if (IsArray(value))
                mJsonDemoder = CreateJsonArray(value);
            else if (IsObject(value))
                mJsonDemoder = CreateJsonObject(value);
            return mJsonDemoder;
        }

        private static JsonObject CreateJsonObject(object value)
        {
            JsonObject mJsonObject = new JsonObject();
            {
                if (IsDict(value))
                {
                    IDictionary mDictionary = (IDictionary)value;
                    foreach (var Key in mDictionary.Keys)
                    {
                        if (IsArray(mDictionary[Key]))
                        {
                            mJsonObject.Put(Key.ToString(), CreateJsonArray(mDictionary[Key]));
                        }
                        else if(IsObject(mDictionary[Key]))
                        {
                            mJsonObject.Put(Key.ToString(), CreateJsonObject(mDictionary[Key]));
                        }
                        else
                        {
                            mJsonObject.Put(Key.ToString(), mDictionary[Key]);
                        }
                    }
                }
                else
                {
                    FieldInfo[] mFieldInfos = value.GetType().GetFields(FJsonUtility.SerializationAttr);
                    foreach (var mFieldInfo in mFieldInfos)
                    {
                        var fieldValue = mFieldInfo.GetValue(value);
                        if (IsArray(fieldValue))
                        {
                            mJsonObject.Put(mFieldInfo.Name , CreateJsonArray(fieldValue));
                        }
                        else if(IsObject(fieldValue))
                        {
                            mJsonObject.Put(mFieldInfo.Name , CreateJsonObject(fieldValue));
                        }
                        else
                        {
                            mJsonObject.Put(mFieldInfo.Name , fieldValue);
                        }
                    }
                }
            }
            return mJsonObject;
        }

        private static JsonArray CreateJsonArray(object value)
        {
            JsonArray mJsonArray = new JsonArray();
            {
                IList mList = (IList) value;
                foreach (var elementValue in mList)
                {
                    if (IsArray(elementValue))
                    {
                        mJsonArray.Add(CreateJsonArray(elementValue));
                    }
                    else if (IsObject(elementValue))
                    {
                        mJsonArray.Add(CreateJsonObject(elementValue));
                    }
                    else
                    {
                        mJsonArray.Add(elementValue);
                    }
                }
            }
            return mJsonArray;
        }

        private static bool IsArray(object value)
        {
            if (value == null)
            {
                return false;
            }
            Type t = value.GetType();
            if (t.IsArray)
                return true;
            if (typeof(IList).IsAssignableFrom(t))
                return true;
            return false;
        }

        private static bool IsDict(object value)
        {
            if (value == null)
            {
                return false;
            }
            Type t = value.GetType();
            if (typeof(IDictionary).IsAssignableFrom(t) && t.GenericTypeArguments.Length == 2)
                return true;
            return false;
        }

        private static bool IsObject(object value)
        {
            if (value == null)
            {
                return false;
            }
            Type t = value.GetType();
            if (t.IsPrimitive == false & typeof(string).IsAssignableFrom(t) == false)
                return true;
            
            return false;
        }
    }
}