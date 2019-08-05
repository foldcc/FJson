using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FJson.Core
{
    public class JsonArray
    {
        private List<object> ArrayObject = new List<object>();
        
        public int Count
        {
            get { return ArrayObject.Count; }
        }

        public void Add(object value)
        {
            ArrayObject.Add(value);
        }

        public object ToArray(Type objectType) 
        {
            IList listObject = null;

            //数组类型
            if (typeof(IList).IsAssignableFrom(objectType))
            {
                Type elementType = null;
                bool isArray = false;
                //Array
                if (typeof(Array).IsAssignableFrom(objectType))
                {
                    elementType = objectType.GetElementType();
                    isArray = true;
                }
                //泛型类
                else
                    elementType = objectType.GenericTypeArguments[0];
                
                
                if (objectType.IsArray)
                {
                    listObject = Array.CreateInstance(elementType ?? throw new InvalidOperationException() , Count);
                }
                else
                {
                    listObject = (IList) Activator.CreateInstance(objectType);
                }
                
                if (Count > 0)
                {
                    int count = 0;
                    foreach (var element in ArrayObject)
                    {
                        object value = null;
                        if (element.GetType() == typeof(JsonArray))
                        {
                            value = ((JsonArray) element).ToArray(elementType);
                        }
                        else if (element.GetType() == typeof(JsonObject))
                        {
                            value = ((JsonObject) element).ToObject(elementType);
                        }
                        else
                        {
                            value = element;
                        }
                        
                        if (isArray)
                        {
                            listObject[count] = Convert.ChangeType(value , 
                                elementType ?? throw new InvalidOperationException());
                            
                        }
                        else
                        {
                            listObject.Add(Convert.ChangeType(value,
                                elementType ?? throw new InvalidOperationException()));
                        }
                        count++;
                    }
                }
            }
            return listObject;
        }
        
        public string ToJson(){
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            int count = 0;
            foreach (var mValue in this.ArrayObject)
            {
                if (mValue == null)
                {
                    sb.Append("null");
                }
                else if (mValue.GetType() == typeof(JsonObject))
                {
                    sb.Append(((JsonObject)mValue).ToJson());
                }
                else if (mValue.GetType() == typeof(JsonArray))
                {
                    sb.Append(((JsonArray)mValue).ToJson());
                }
                else
                {
                    
                    if (mValue.GetType().IsValueType)
                    {
                        sb.Append(mValue);
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
            sb.Append(']');
            return sb.ToString();
        }

    }
}