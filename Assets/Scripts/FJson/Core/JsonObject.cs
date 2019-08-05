using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
        
        public JsonArray GetJsonArray(string key)
        {
            return (JsonArray) ObjectDict[key];
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
                            dict[dictKey] = GetJsonArray(dictKey).ToArray(valueType);
                        }
                        else
                        {
                            dict[dictKey] = Convert.ChangeType(GetObject(dictKey), valueType);
                        }
                    }
                    obj = dict;
                }
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
                                fieldInfo.SetValue(obj, GetJsonObject(fieldInfo.Name).ToObject(fieldInfo.FieldType));
                            }
                            else if (IsJsonArray(fieldInfo.Name))
                            {
                                fieldInfo.SetValue(obj, GetJsonArray(fieldInfo.Name).ToArray(fieldInfo.FieldType));
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
        
        public string ToJson(){
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            int count = 0;
            foreach (var mDictKey in this.ObjectDict.Keys)
            {
                sb.Append('"');
                sb.Append(mDictKey);
                sb.Append('"');
                sb.Append(':');
                
                if (IsJsonObject(mDictKey))
                {
                    sb.Append(GetJsonObject(mDictKey).ToJson());
                }
                else if (IsJsonArray(mDictKey))
                {
                    sb.Append(GetJsonArray(mDictKey).ToJson());
                }
                else
                {
                    if (this.ObjectDict[mDictKey] == null)
                    {
                        sb.Append("null");
                    }
                    else if (this.ObjectDict[mDictKey].GetType().IsValueType)
                    {
                        sb.Append(this.ObjectDict[mDictKey]);
                    }
                    else
                    {
                        sb.Append('"');
                        sb.Append(this.ObjectDict[mDictKey]);
                        sb.Append('"');
                    }
                }

                if (count < Count - 1)
                    sb.Append(',');
                count++;
            }
            sb.Append('}');
            return sb.ToString();
        }
    }
}