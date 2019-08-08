
    using System;
    using System.Collections.Generic;
    using FJson;

    public class Main
    {
        public static void main()
        {
            Main m = new Main();
            m.listTest();
        }

        void sturectTest()
        {
            List<object> listObj = new List<object>(new object[]{null , 1 , false , "test"});
            string json = FJsonUtility.ToJson(listObj);

            Dictionary<string, object> jsonObject = FJsonUtility.ToObject<Dictionary<string, object>>(json);
            
            Console.WriteLine(json);
        }

        void listTest()
        {
            Dictionary<string,object> dictObj = new Dictionary<string, object>();
            dictObj.Add("name" , "测试");
            dictObj.Add("ID" , 5321);
            dictObj.Add("info" , "测试info");
            dictObj.Add("infoList" , new object[]{1,-1,null});

            var person = FJsonUtility.Convert<Person>(dictObj);
            Console.WriteLine(person);

        }
    }

    public class Person
    {
        public string name;
        public double ID;
        private string info;
        public object[] infoList;

        public override string ToString()
        {
            return $"name {this.name} , id: {this.ID} , infoList: {this.infoList.Length}";
        }
    }
    
