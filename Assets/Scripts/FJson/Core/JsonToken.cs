using System;

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

        public object GetTokenObject()
        {
            switch (mType)
            {
                case TokenType.STRING:
                    return this.mValue;
                case TokenType.NUMBER:
                    return float.Parse(this.mValue);
                case TokenType.NULL:
                    return null;
                case TokenType.START_ARR:
                    return this.mValue;
                case TokenType.END_ARR:
                    return this.mValue;
                case TokenType.START_OBJ:
                    return this.mValue;
                case TokenType.END_OBJ:
                    return this.mValue;
                case TokenType.COMMA:
                    return this.mValue;
                case TokenType.COLON:
                    return this.mValue;
                case TokenType.BOOL:
                    return bool.Parse(this.mValue);
                case TokenType.END_DOC:
                    return this.mValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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