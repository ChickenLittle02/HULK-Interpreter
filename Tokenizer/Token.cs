




public enum TokenType
{
    Number, //todos los valores numericos
    SUM_Operator,//+
    REST_Operator,//-
    MULT_Operator,//*
    DIV_Operator, // /
    POW_Operator,//^
    CONCAT_OPERATOR, // @
    RIGHT_PARENTHESIS,// (
    LEFT_PARENTHESIS,// )
    Asignation_Operator,// =
    Equal_Operator,// ==
    Distinct,// /!=
    More_Than, //>
    More_Equal_Than, //>=
    Min_Than,// <
    And_Operator,// &
    Or_Operator,// |
    Not_Operator,// /!
    Min_Equal_Than,// <=
    Arrow, //=>
    Identifier, // todos los nombres
    Keyword, //Las palabras reservadas, number, string, bool, let, in, 
    Function_Keyword, //function
    Let_Keyword,//let
    In_Keyword,//in
    Boolean,// true, false
    Comma, //,
    Semicolon, //;
    Quotes_Text, // "mm"
    Blank_Space,// " "
    EOT, //End of Tokens
    nul,
}



public class Token
{
    public int inicio_else { get; private set; }
    public int final_else { get; private set; }
    public TokenType Type { get; set; }
    public object Value { get; set; }

    public Token(TokenType type, object value)
    {
        Type = type;
        Value = value;
        inicio_else = 0;
        final_else = 0;
    }
    public void Show()
    {
        System.Console.WriteLine("(" + Type + "," + Value + ")");
    }
    public void Set_Inicio_FInal(int inicio, int final)
    {
        inicio_else = inicio;
        final_else = final;
    }
}