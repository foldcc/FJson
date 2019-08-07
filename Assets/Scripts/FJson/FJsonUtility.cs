using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FJson.Core;
using UnityEngine;

namespace FJson
{
    public class FJsonUtility
    {
        public static string ToJson(object value)
        {
            IJsonDemoder mJsonDemoder = null;
            if (IsArray(value))
                mJsonDemoder = CreateJsonArray(value);
            else if (IsObject(value))
                mJsonDemoder = CreateJsonObject(value);
            return mJsonDemoder != null ? mJsonDemoder.Serialization() : value.ToString();
        }

        public static T ToObject<T>(string json)
        {
            var jsonObject = new JsonParser().ParseJson(json);
            if (jsonObject != null)
            {
                return (T)jsonObject.Deserialization(typeof(T));
            }
            return default;
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
                    FieldInfo[] mFieldInfos = value.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
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