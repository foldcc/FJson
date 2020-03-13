using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FJson.Core
{
    public class JsonObject : IJsonDemoder
    {
        private Dictionary<string, object> ObjectDict = new Dictionary<string, object>();

        public void Clear()
        {
            this.ObjectDict.Clear();
        }

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
            if (ObjectDict[key] is JsonObject)
            {
                return true;
            }
            return false;
        }
        
        public bool IsJsonArray(string key)
        {
            if (ObjectDict[key] is JsonArray)
            {
                return true;
            }
            return false;
        }

        public bool ContainsKey(string key)
        {
            return this.ObjectDict.ContainsKey(key);
        }

        public JsonObject GetJsonObject(string key)
        {
            return (JsonObject) this.ObjectDict[key];
        }
        
        public T ToObject<T>(string key)
        {
            var td =  this.GetJsonObject(key);
            return (T)td.Deserialization(typeof(T));
        }
        
        public JsonArray GetJsonArray(string key)
        {
            return (JsonArray) ObjectDict[key];
        }

        public object GetObject(string key)
        {
            return ObjectDict[key];
        }
        
        public T GetObject<T>(string key)
        {
            return (T)ObjectDict[key];
        }
        
        public object Deserialization(Type objectType)
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
                            dict[dictKey] = GetJsonObject(dictKey).Deserialization(valueType);
                        }
                        else if (IsJsonArray(dictKey))
                        {
                            dict[dictKey] = GetJsonArray(dictKey).Deserialization(valueType);
                        }
                        else
                        {
                            dict[dictKey] = Convert.ChangeType(this.GetObject(dictKey), valueType);
                        }
                    }
                    obj = dict;
                }
                else if (objectType == typeof(object))
                {
                    obj = this;
                }
                //自定义类型
                else
                {
                    var mFields = objectType.GetFields(FJsonUtility.DeserializationAttr);
                    foreach (var fieldInfo in mFields)
                    {
                        if (ContainsKey(fieldInfo.Name) )
                        {
                            if (IsJsonObject(fieldInfo.Name))
                            {
                                //自定义对象
                                fieldInfo.SetValue(obj, GetJsonObject(fieldInfo.Name).Deserialization(fieldInfo.FieldType));
                            }
                            else if (IsJsonArray(fieldInfo.Name))
                            {
                                fieldInfo.SetValue(obj, GetJsonArray(fieldInfo.Name).Deserialization(fieldInfo.FieldType));
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
        
        public string Serialization(){
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            int count = 0;
            foreach (var mDictKey in this.ObjectDict.Keys)
            {
                var mValue = this.ObjectDict[mDictKey];
                sb.Append('"');
                sb.Append(mDictKey);
                sb.Append('"');
                sb.Append(':');
                
                if (IsJsonObject(mDictKey))
                {
                    sb.Append(GetJsonObject(mDictKey).Serialization());
                }
                else if (IsJsonArray(mDictKey))
                {
                    sb.Append(GetJsonArray(mDictKey).Serialization());
                }
                else
                {
                    if (mValue == null)
                    {
                        sb.Append("null");
                    }
                    else if (mValue.GetType().IsPrimitive && mValue.GetType() != typeof(char) && mValue.GetType() != typeof(Char))
                    {
                        if (mValue is bool)
                        {
                            sb.Append(mValue.ToString().ToLower());
                        }
                        else
                        {
                            sb.Append(mValue);
                        }
                    }
                    else
                    {
                        sb.Append('"');
                        sb.Append(mValue);
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