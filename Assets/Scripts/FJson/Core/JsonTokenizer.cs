using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FJson.Core
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

            while (_index < _jsonData.Length && this.clearSpace())
            {
                this.JsonTokens.Add(this.parse());
            }
        }

        private bool clearSpace()
        {
            do
            {
                this.read();   
                if (this._index > this._jsonData.Length)
                    return false;
            } while (isSplace());
            return true;
        }  

        private JsonToken parse()
        {
            if (isNull())
            {
                return new JsonToken(TokenType.NULL , null);
            }
            else if (_char == ',') 
            {
                return new JsonToken(TokenType.COMMA, null);
            }
            else if (_char == ':') 
            {
                return new JsonToken(TokenType.COLON, null);
            }
            else if (_char == '{') 
            {
                return new JsonToken(TokenType.START_OBJ, null);
            } 
            else if (_char == '[') 
            {
                 return new JsonToken(TokenType.START_ARR, null);
            } 
            else if (_char == ']') 
            {
                return new JsonToken(TokenType.END_ARR, null);
            }
            else if (_char == '}')
            {
                return new JsonToken(TokenType.END_OBJ, null);
            }
            else if (isTrue()) 
            {
                 return new JsonToken(TokenType.BOOL, JsonToken.TRUE);
            }
            else if (isFalse()) 
            {
                 return new JsonToken(TokenType.BOOL, JsonToken.FALSE); 
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
                return new JsonToken(TokenType.END_DOC, null);
            }
            throw new DataException("Invalid JSON input : " + _char);
        }

        private JsonToken readString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                this.read();
                if (_char == '\\')
                {
                    stringBuilder.Append(_char);
                    this.read();
                }
                else if (this._char == '"')
                {
                    break;
                }
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
                this.read();
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
                throw new DataException("json异常结尾 Invalid JSON input. error end   " + _char);
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
            if (_char == 'n' || _char == 'N')
            {
                this.read();
                if (_char == 'u')
                {
                    this.read();
                    if (_char == 'l')
                    {
                        this.read();
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
            if (_char == 't' || _char == 'T')
            {
                this.read();
                if (_char == 'r')
                {
                    this.read();
                    if (_char == 'u')
                    {
                        this.read();
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
            if (_char == 'f' || _char == 'F')
            {
                this.read();
                if (_char == 'a')
                {
                    this.read();
                    if (_char == 'l')
                    {
                        this.read();
                        if (_char == 's')
                        {
                            this.read();
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