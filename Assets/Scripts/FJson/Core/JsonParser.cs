using System;
using System.Collections.Generic;
using System.Data;

namespace FJson.Core
{
    public class JsonParser
    {
        private JsonTokenizer _jsonTokenizer;
        private int _tokenIndex = 0;

        public JsonObject ParseJsonObject(string json )
        {
            _jsonTokenizer = new JsonTokenizer(json);
            _tokenIndex = 0;
            return this.parseJsonObject();
        }

        public JsonArray ParseJsonArray(string json)
        {
            _jsonTokenizer = new JsonTokenizer(json);
            _tokenIndex = 0;
            return this.parserJsonArray();
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
                            if (this.IsValue())
                            {
                                jsonObject.Put(this._jsonTokenizer.JsonTokens[this._tokenIndex - 2].GetTokenValue() , this._jsonTokenizer.JsonTokens[this._tokenIndex].GetTokenObject());
                            }
                            else if(this.IsObject())
                            {
                                jsonObject.Put(this._jsonTokenizer.JsonTokens[this._tokenIndex - 2].GetTokenValue() , this.parseJsonObject());
                            }else if (this.IsArray())
                            {
                                jsonObject.Put(this._jsonTokenizer.JsonTokens[this._tokenIndex - 2].GetTokenValue() , this.parserJsonArray());
                            }
                            else
                            {
                                throw new DataException("Invalid JSON input. is error Value " + this._jsonTokenizer.JsonTokens[this._tokenIndex]);  
                            }
                        }
                        else
                        {
                            throw new DataException("Invalid JSON input. is not COLON " + this._jsonTokenizer.JsonTokens[this._tokenIndex]); 
                        }
                    }
                    else
                    {
                        throw new DataException("Invalid JSON input. is not Key " + this._jsonTokenizer.JsonTokens[this._tokenIndex]);
                    }

                    //COMMA判断
                    this._tokenIndex++;
                    if (!this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.COMMA) && !this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.END_OBJ))
                    {
                        throw new DataException("Invalid JSON input. error end " + this._jsonTokenizer.JsonTokens[this._tokenIndex]);
                    }
                    
                }
            }
            return jsonObject;
        }

        private JsonArray parserJsonArray()
        {
            var jsonArray = new JsonArray();
            if (_jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.START_ARR))
            {
                while (!this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.END_ARR))
                {
                    this._tokenIndex++;
                    if (this.IsValue())
                    {
                        jsonArray.Add(this._jsonTokenizer.JsonTokens[this._tokenIndex].GetTokenValue());
                    }
                    else if(this.IsObject())
                    {
                        jsonArray.Add(this.parseJsonObject());
                    }else if (this.IsArray())
                    {
                        jsonArray.Add(this.parserJsonArray());
                    }
                    else
                    {
                        throw new DataException("Invalid JSON input. is error Value " + this._jsonTokenizer.JsonTokens[this._tokenIndex]);  
                    }

                    //COMMA判断
                    this._tokenIndex++;
                    if (!this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.COMMA) && !this._jsonTokenizer.JsonTokens[this._tokenIndex].IsType(TokenType.END_ARR))
                    {
                        throw new DataException("Invalid JSON input. error end " + this._jsonTokenizer.JsonTokens[this._tokenIndex]);
                    }
                    
                }
            }
            return jsonArray;
        }

        private bool IsValue()
        {
            var tokenType = this._jsonTokenizer.JsonTokens[this._tokenIndex].GetTokenType();
            if (tokenType == TokenType.BOOL | tokenType == TokenType.NULL | tokenType == TokenType.NUMBER | tokenType == TokenType.STRING)
            {
                return true;
            }
            return false;
        }

        private bool IsArray()
        {
            var tokenType = this._jsonTokenizer.JsonTokens[this._tokenIndex].GetTokenType();
            if (tokenType == TokenType.START_ARR)
            {
                return true;
            }
            return false;
        }
        
        private bool IsObject()
        {
            var tokenType = this._jsonTokenizer.JsonTokens[this._tokenIndex].GetTokenType();
            if (tokenType == TokenType.START_OBJ)
            {
                return true;
            }
            return false;
        }
        
    }
}