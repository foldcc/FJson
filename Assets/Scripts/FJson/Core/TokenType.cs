namespace FJson.Core
{
    public enum TokenType
    {
        STRING,
        NUMBER,
        NULL,
        START_ARR,
        END_ARR,
        START_OBJ,
        END_OBJ,
        COMMA,
        COLON,
        BOOL,
        END_DOC
    }
}