using System.Data;
using Fjson.Core;

namespace FJson.Core
{
    public class JsonObjectParser
    {
        private JsonTokenizer _jsonTokenizer;
        private int _tokenIndex = 0;

        public JsonObject GetJsonObject(string json )
        {
            _jsonTokenizer = new JsonTokenizer(json);
            _tokenIndex = 0;
            return this.parseJsonObject();
        }

        private JsonObject parseJsonObject()
        {
            var jsonObject = new JsonObject();
            
            if (_jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.START_OBJ))
            {
                while (!this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.END_OBJ))
                {
                    this._tokenIndex++;
                    if (this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.STRING))
                    {
                        this._tokenIndex++;
                        if (this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.COLON))
                        {
                            this._tokenIndex++;
                            
                        }
                        else
                        {
                            throw new DataException("Invalid JSON input. is not COLON " + this._jsonTokenizer.JsonTokens[this._tokenIndex+1]); 
                        }
                    }
                    else
                    {
                        throw new DataException("Invalid JSON input. is not Key " + this._jsonTokenizer.JsonTokens[this._tokenIndex+1]);
                    }

                    //COMMA判断
                    this._tokenIndex++;
                    if (!this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.COMMA))
                    {
                        throw new DataException("Invalid JSON input. error end " + this._jsonTokenizer.JsonTokens[this._tokenIndex]);
                    }
                    
                }
            }
            return jsonObject;
        }
        
        private bool IsKeyValue()
        {
            if (this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.STRING))
            {
                if (this._jsonTokenizer.JsonTokens[this._tokenIndex+1].IsType(TokenType.COLON))
                {
                    var tokenType = this._jsonTokenizer.JsonTokens[this._tokenIndex + 2].GetTokenType();
                    if (tokenType == TokenType.BOOL | tokenType == TokenType.NULL | tokenType == TokenType.NUMBER | tokenType == TokenType.STRING)
                    {
                        return true;
                    }

                    return false;
                }
                throw new DataException("Invalid JSON input. is not COLON " + this._jsonTokenizer.JsonTokens[this._tokenIndex+1]);
            }
            throw new DataException("Invalid JSON input. is not Key " + this._jsonTokenizer.JsonTokens[this._tokenIndex+1]);
        }

        private bool IsKeyArray()
        {
            return false;
        }
        
        private bool IsKeyObject()
        {
            if (this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.STRING))
            {
                if (this._jsonTokenizer.JsonTokens[this._tokenIndex+1].IsType(TokenType.COLON))
                {
                    var tokenType = this._jsonTokenizer.JsonTokens[this._tokenIndex + 2].GetTokenType();
                    if (tokenType == TokenType.START_OBJ)
                    {
                        return true;
                    }
                    return false;
                }
                throw new DataException("Invalid JSON input. is not COLON " + this._jsonTokenizer.JsonTokens[this._tokenIndex+1]);
            }
            throw new DataException("Invalid JSON input. is not Key " + this._jsonTokenizer.JsonTokens[this._tokenIndex+1]);
        }
    }
}