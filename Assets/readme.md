# Fjson

Fjson是一套高易用Json解析库，无需对待序列化/反序列化的对象做特殊书写规范和约束

缺点：效率不高，不适合大量密集型数据处理。

# 序列化

> FJsonUtility.ToJson(object obj)

Fjson支持序列化大部分常用类型，可以直接对List、数组、键值对、字典、自定义对象、结构体进行序列化

#### 序列化示例1
```c#
Vector3 pos = new Vector3(100,1.5f,-50);

string json = FJsonUtility.ToJson(pos);
```
#### 序列化示例2
```c#
List<object> listObj = new List<object>(new object[]{null , 1 , false , "test"});

string json = FJsonUtility.ToJson(listObj);
```

# 反序列化

> FJsonUtility.ToObject<T>(string json)

Fjson可以直接将json反序列化为List、数组、键值对、字典、自定义对象、结构体。

#### 反序列化示例1
```c#
Dictionary<string, object> jsonObject = FJsonUtility.ToObject<Dictionary<string, object>>(json);
```

* **注： 如果反序列化目标类型是Object，则序列化默认赋值类型是JsonObject\JsonArray**

# 对象转换
> FJsonUtility.Convert\<T>(object value)

Fjson支持结构体、List、数组、Dictionary等对象\结构体相互转换，如一个Dictionary<string,object>转换为一个自定义对象，或者Array转换为一个List等

此外如果一个对象中的x字段为float类型，而转换目标中x字段为string或者int类型则会自动转换类型。

以下示例代码：

1 定义自定义类型
```c#
    public class Person
    {
        public string name;
        public double ID;
        private string info;
        public object[] infoList;

        public override string ToString()
        {
            return $"name {this.name} , id: {this.ID}";
        }
    }
```

2 创建一个Dictionary并转换为Object

```c#
Dictionary<string,object> dictObj = new Dictionary<string, object>();
            dictObj.Add("name" , "测试");
            dictObj.Add("ID" , 5321);
            dictObj.Add("info" , "测试info");
            dictObj.Add("infoList" , new object[]{1,-1,null});
var person = FJsonUtility.Convert<Person>(dictObj);
Console.WriteLine(person);
```