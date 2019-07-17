using System.Collections.Generic;

namespace FJson.Core
{
    public class JsonArray
    {
        private List<object> jsonDatas = new List<object>();

        public void Add(object value)
        {
            this.jsonDatas.Add(value);
        }

        public object Get(int index)
        {
            return this.jsonDatas[index];
        }

        public override string ToString()
        {
            string Str = "";
            foreach (object mJsonData in this.jsonDatas)
            {
                Str += mJsonData + " ";
            }

            return Str;
        }
    }
}