using System;

namespace FJson.Core
{
    public interface IJsonDemoder
    {
        string Serialization();
        object Deserialization(Type objectType);
    }
}