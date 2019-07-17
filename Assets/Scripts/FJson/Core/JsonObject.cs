using System.Collections.Generic;

namespace FJson.Core
{
    public class JsonObject
    {
        private Dictionary<string , object> jsonData = new Dictionary<string, object>();

        public void Put(string key , object value)
        {
            this.jsonData.Add(key , value);
        }

        public object Get(string key)
        {
            object obj = null;
            this.jsonData.TryGetValue(key, out obj);
            return obj;
        }

        public JsonObject GetJsonObject(string key)
        {
            return null;
        }

        public JsonArray GetJsonArray(string key)
        {
            return null;
        }

        public override string ToString()
        {
            string Str = "";
            foreach (KeyValuePair<string,object> mKeyValuePair in this.jsonData)
            {
                Str += mKeyValuePair.Key + " : " + mKeyValuePair.Value + "\n";
            }

            return Str;
        }
    }
}