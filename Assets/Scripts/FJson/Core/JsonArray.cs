using System.Collections.Generic;

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
    }
}