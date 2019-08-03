using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FJson.Core
{
    public class JsonObject
    {
        private Dictionary<string, object> ObjectDict = new Dictionary<string, object>();

        public int Count
        {
            get { return ObjectDict.Count; }
        }

        public void Put(string key , object value)
        {
            ObjectDict[key] = value;
        }

        public bool IsJsonObject(string key)
        {
            if (ObjectDict.ContainsKey(key) && ObjectDict[key] is JsonObject)
            {
                return true;
            }
            return false;
        }
        
        public bool IsJsonArray(string key)
        {
            if (ObjectDict.ContainsKey(key) && ObjectDict[key] is JsonArray)
            {
                return true;
            }
            return false;
        }

        public bool ContainsKey(string key)
        {
            return ObjectDict.ContainsKey(key);
        }

        public JsonObject GetJsonObject(string key)
        {
            return (JsonObject) ObjectDict[key];
        }

        public object GetObject(string key)
        {
            return ObjectDict[key];
        }
        
        public object ToObject(Type objectType)
        {
            if (Count > 0)
            {
                object obj = Activator.CreateInstance(objectType);
                
                //字典类型
                if (typeof(IDictionary).IsAssignableFrom(objectType) && objectType.GenericTypeArguments.Length == 2)
                {
                    
                    Type valueType = objectType.GenericTypeArguments[1];
                    var dict = (IDictionary)obj;
                    foreach (var dictKey in ObjectDict.Keys)
                    {
                        if (IsJsonObject(dictKey))
                        {
                            dict[dictKey] = GetJsonObject(dictKey).ToObject(valueType);
                        }
                        else if (IsJsonArray(dictKey))
                        {
                            
                        }
                        else
                        {
                            dict[dictKey] = Convert.ChangeType(GetObject(dictKey), valueType);
                        }
                    }
                    obj = dict;
                }
                
//                else if (objectType == typeof(object))
//                {
//                    
//                }
                //自定义类型
                else
                {
                    var mFields = objectType.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    foreach (var fieldInfo in mFields)
                    {
                        if (ContainsKey(fieldInfo.Name) )
                        {
                            if (IsJsonObject(fieldInfo.Name))
                            {
                                //自定义对象
                                object value = GetJsonObject(fieldInfo.Name).ToObject(fieldInfo.FieldType);
                                fieldInfo.SetValue(obj, value);
                            }
                            else if (IsJsonArray(fieldInfo.Name))
                            {
                                //TODO 反序列化为数组类型的值
                            }
                            else
                            {
                                //基础类型复制
                                object value = Convert.ChangeType(GetObject(fieldInfo.Name), fieldInfo.FieldType);
                                fieldInfo.SetValue(obj, value);
                            }
                        }
                    }
                }
                return obj;
            }
            return default;
        }
    }
}