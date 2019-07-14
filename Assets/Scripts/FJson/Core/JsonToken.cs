namespace Fjson.Core
{
    public class JsonToken
    {
        private TokenType mType;
        private string mValue;

        public JsonToken(TokenType tokenType , string value)
        {
            this.mType = tokenType;
            this.mValue = value;
        }

        public TokenType GetType()
        {
            return mType;
        }

        public string GetValue()
        {
            return mValue;
        }
    }
}