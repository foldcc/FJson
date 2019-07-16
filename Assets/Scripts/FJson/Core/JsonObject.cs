using System.Collections.Generic;

namespace FJson.Core
{
    public class JsonObject
    {
        private Dictionary<string , object> jsonData = new Dictionary<string, object>();

        public void Put(string key , object value)
        {
            
        }

        public object Get(string key)
        {
            return null;
        }

        public JsonObject GetJsonObject(string key)
        {
            return null;
        }

        public JsonArray GetJsonArray(string key)
        {
            return null;
        }
        
    }
}