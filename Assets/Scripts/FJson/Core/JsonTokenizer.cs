using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;

namespace Fjson.Core
{
    //对json字符串分词解析
    public class JsonTokenizer
    {
        public List<JsonToken>                   JsonTokens;
        
        
        private char[]                           _jsonData;
        private char                             _char;
        private int                              _index;

        public  JsonTokenizer(string json)
        {
            JsonTokens = new List<JsonToken>();
            JsonTokens.Clear();
            _jsonData = json.ToCharArray();
            _char = '?';
            _index = 0;

            while (_index < _jsonData.Length)
            {
                JsonTokens.Add(parse());
            }
        }


        public int GetNextTokenIndex(TokenType tokenType , int startIndex)
        {
            if (startIndex < this.JsonTokens.Count)
            {
                for (int i = startIndex; i < JsonTokens.Count; i++)
                {
                    if (this.JsonTokens[i].IsType(tokenType))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private JsonToken parse()
        {
            do
            {
                read();
            } while (isSplace());

            if (isNull())
            {
                return new JsonToken(TokenType.NULL , "null");
            }
            else if (_char == ',') 
            {
                return new JsonToken(TokenType.COMMA, ",");
            }
            else if (_char == ':') 
            {
                return new JsonToken(TokenType.COLON, ":");
            }
            else if (_char == '{') 
            {
                return new JsonToken(TokenType.START_OBJ, "{");
            } 
            else if (_char == '[') 
            {
                 return new JsonToken(TokenType.START_ARR, "[");
            } 
            else if (_char == ']') 
            {
                return new JsonToken(TokenType.END_ARR, "]");
            }
            else if (_char == '}')
            {
                return new JsonToken(TokenType.END_OBJ, "}");
            }
            else if (isTrue()) 
            {
                 return new JsonToken(TokenType.BOOL, "true");
            }
            else if (isFalse()) 
            {
                 return new JsonToken(TokenType.BOOL, "false"); 
            }
            else if (_char == '"') 
            {
                 return readString();
            }
            else if (isNum()) 
            {
                 return readNum();
            }
            else if (_char == -1)
            {
                return new JsonToken(TokenType.END_DOC, "EOF");
            }
            throw new DataException("Invalid JSON input : " + _char);
        }

        private JsonToken readString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                read();
                if (_char == '"')
                    break;
                stringBuilder.Append(_char);
            } while (true);

            return new JsonToken(TokenType.STRING , stringBuilder.ToString());
        }

        private JsonToken readNum()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(_char);
            bool isPoint = false;
            int count = 0;
            if (this._char != '-')
                count++;
            do
            {
                read();
                if (this._char == '.' && count > 0 && isPoint == false)
                {
                    stringBuilder.Append(this._char);
                    isPoint = true;
                }
                else if (char.IsDigit(this._char))
                {
                    stringBuilder.Append(this._char);
                }
                else
                {
                    break; 
                }
                count++;
            } while (true);
            _index--;
            return new JsonToken(TokenType.NUMBER , stringBuilder.ToString());
        }


        private void read()
        {
            if (_index >= _jsonData.Length)
            {
                throw new DataException("Invalid JSON input. error end   " + _char);
            }
            _char = _jsonData[_index];
            _index++;
        }

        private bool isNum()
        {
            return char.IsDigit(_char) || _char == '-';
        }

        private bool isNull()
        {
            if (_char == 'n')
            {
                read();
                if (_char == 'u')
                {
                    read();
                    if (_char == 'l')
                    {
                        read();
                        if (_char == 'l')
                        {
                            return true;
                        }
                    }
                }
                throw new DataException("Invalid JSON input. 'null' is error  " + _index);
            }
            return false;
        }

        private bool isTrue()
        {
            if (_char == 't')
            {
                read();
                if (_char == 'r')
                {
                    read();
                    if (_char == 'u')
                    {
                        read();
                        if (_char == 'e')
                        {
                            return true;
                        }
                    }
                }
                throw new DataException("Invalid JSON input. 'true' is error  " + _index);
            }
            return false;
        }

        private bool isFalse()
        {
            if (_char == 'f')
            {
                read();
                if (_char == 'a')
                {
                    read();
                    if (_char == 'l')
                    {
                        read();
                        if (_char == 's')
                        {
                            read();
                            if (_char == 'e')
                            {
                                return true;
                            }
                        }
                    }
                }
                throw new DataException("Invalid JSON input. 'false' is error  " + _index);
            }
            return false;
        }

        private bool isSplace()
        {
            if (_char == ' ' || _char == '\n' || _char == '\t' || _char == '\r')
            {
                return true;
            }
            return false;
        }
        
    }
}