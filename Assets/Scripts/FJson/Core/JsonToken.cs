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

        public TokenType GetTokenType()
        {
            return mType;
        }

        public string GetTokenValue()
        {
            return mValue;
        }

        public bool IsType(TokenType tokenType)
        {
            return this.mType.Equals(tokenType);
        }

        public override string ToString()
        {
            return this.mType + " : " + this.mValue;
        }
    }
}