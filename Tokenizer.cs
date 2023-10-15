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
    Or_Operator,// ||
    Min_Equal_Than,// <=
    Arrow, //=>
    Identifier, // todos los nombres
    Keyword, //Las palabras reservadas, number, string, bool, let, in, 
    Function_Keyword, //function
    Let_Keyword,//let
    In_Keyword,//in
    Bool_True,
    Bool_False,
    Comma, //,
    Semicolon, //;
    Quotes_Text, // "mm"
    Blank_Space,// " "
    EOT, //End of Tokens
}

public class Token
{
    public TokenType Type { get; set; }
    public object Value { get; set; }

    public Token(TokenType type, object value)
    {
        Type = type;
        Value = value;
    }
    public void Show()
    {
        System.Console.WriteLine("(" + Type + "," + Value + ")");
    }
}



public class Tokenizer
{//Esta clase crea el consjunto de tokens, la clase parser es la que tiene que verificar si el conjunto de tokens
 // funciona correctamente, es decir si por cada token, el token siguiente es valido
    public List<Token> TokenSet { get; set; }
    public int position { get; set; }
    public char actual_char { get; set; }
    public string Text { get; set; }
    public int text_size;
    public TokenType actual_Tokentype { get; set; }

    public string[] Keywords = { "let", "in", "number", "string", "bool", "if", "else", "function",
    "sqrt","cos","sin","exp","log","rand", "print"};
    public string actual_TokenValue { get; set; }
    public Token actual_Token { get; set; }
    private void Error(string message)
    {
        throw new Exception(message);
    }

    public Tokenizer(string text)
    {
        text_size = text.Length;
        position = 0;
        if (text_size != position)
        {

            Text = text;
            TokenSet = new List<Token>();

            actual_char = text[position];
            actual_TokenValue = "";
            Start();
        }
        else
        {
          //  System.Console.WriteLine("Isn't possible to find a token, the code is empty");
        }
    }
    public void Start()
    {
        while (position < text_size)
        {
            Add_Simple_Token();
        }
    }

    public void Add_Simple_Token()
    {

        switch (actual_char)
        {
            case ';':

              //  System.Console.WriteLine("Entro a que es un punto y coma");
                Add_To_TokenSet(TokenType.Semicolon, ';');
                GetNextChar();

                break;

            case ',':

                // System.Console.WriteLine("Entro a que es una coma");
                Add_To_TokenSet(TokenType.Comma, ',');
                GetNextChar();

                break;
            case ' ':

                // System.Console.WriteLine("Entro a que es un espacio");
                actual_Tokentype = TokenType.Blank_Space;
                GetNextChar();
                break;

            case '+':

                // System.Console.WriteLine("Entro a que es un operador");
                Add_To_TokenSet(TokenType.SUM_Operator, actual_char);
                GetNextChar();
                break;
            case '-':

                // System.Console.WriteLine("Entro a que es un operador");
                Add_To_TokenSet(TokenType.REST_Operator, actual_char);
                GetNextChar();
                break;

            case '/':

                // System.Console.WriteLine("Entro a que es un operador");
                Add_To_TokenSet(TokenType.DIV_Operator, actual_char);
                GetNextChar();

                break;
            case '*':

                // System.Console.WriteLine("Entro a que es un operador");
                Add_To_TokenSet(TokenType.MULT_Operator, actual_char);
                GetNextChar();
                break;
            case '(':
                // System.Console.WriteLine("Entro a que es un parentesis izquierdo");
                Add_To_TokenSet(TokenType.LEFT_PARENTHESIS, actual_char);
                GetNextChar();

                break;
            case ')':

                // System.Console.WriteLine("Entro a que es un parentesis derecho");
                Add_To_TokenSet(TokenType.RIGHT_PARENTHESIS, actual_char);
                GetNextChar();

                break;
            case '^':

                // System.Console.WriteLine("Entro a que es el simbolo de potencia");
                Add_To_TokenSet(TokenType.POW_Operator, actual_char);
                GetNextChar();

                break;
            case '@':

                // System.Console.WriteLine("Entro a que es el operador de concatenar texto");
                Add_To_TokenSet(TokenType.CONCAT_OPERATOR, actual_char);
                GetNextChar();

                break;
            default:
                Add_Compose_Token();
                break;
        }

    }

    public void Add_To_TokenSet(TokenType Type, object Value)
    {
        Token Element = new Token(Type, Value);
        TokenSet.Add(Element);

    }

    public void Add_Compose_Token()
    {
        if (Char.IsDigit(actual_char))
        {
            // System.Console.WriteLine("Entro a que es un digito");
            actual_TokenValue = "";
            actual_Tokentype = TokenType.Number;

            while (position < text_size && Char.IsDigit(actual_char))
            {
                actual_TokenValue += actual_char;
                GetNextChar();
            }

            if (Char.IsLetter(actual_char))
            {
                // System.Console.WriteLine("Error, there should be a number");
                //Como puedo hacer para que en este punto el interprete deje de correr
                //Porque hubo un error
                return;
            }

            Add_To_TokenSet(actual_Tokentype, actual_TokenValue);
        }
        else if (actual_char == '"')
        {
            actual_TokenValue = "";
            actual_Tokentype = TokenType.Quotes_Text;
            GetNextChar();
            while (position < text_size && actual_char != '"')
            {
                actual_TokenValue += actual_char;
                GetNextChar();
            }

            if (actual_char != '"')
            {
                // System.Console.WriteLine("Error, falta " + '"');
                return;
            }

            Add_To_TokenSet(actual_Tokentype, actual_TokenValue);
            GetNextChar();

        }
        else if (actual_char == '_')
        {
            // System.Console.WriteLine("Entro a que es una letra, o un _");
            actual_TokenValue = "" + actual_char;
            actual_Tokentype = TokenType.Identifier;
            GetNextChar();

            while (position < text_size && (Char.IsLetterOrDigit(actual_char) || actual_char == '_'))
            {
                actual_TokenValue += actual_char;
                GetNextChar();
            }

            Add_To_TokenSet(actual_Tokentype, actual_TokenValue);
        }
        else if (Char.IsLetter(actual_char))
        {
            bool comprobando = true;
            // System.Console.WriteLine("Entro a que es una letra");
            actual_TokenValue = "" + actual_char;
            GetNextChar();

            while (position < text_size && (Char.IsLetterOrDigit(actual_char) || actual_char == '_'))
            {
                if (actual_char == '_') comprobando = false;
                actual_TokenValue += actual_char;
                GetNextChar();
            }
            // System.Console.WriteLine("Comprobando es " + comprobando + " y el valor de ItsKeyword es " + ItsKeyword(actual_TokenValue));

            if (comprobando && ItsKeyword(actual_TokenValue))
            {
                // System.Console.WriteLine("Entro a que es Keyword y el valor de ItsNotKeyword es " + ItsKeyword(actual_TokenValue));

                if (actual_TokenValue == "let") Add_To_TokenSet(TokenType.Let_Keyword, actual_TokenValue);

                else if (actual_TokenValue == "in") Add_To_TokenSet(TokenType.In_Keyword, actual_TokenValue);

                else if (actual_TokenValue == "function")
                {
                    Add_To_TokenSet(TokenType.Function_Keyword, actual_TokenValue);
                    Add_Function();
                }

                else Add_To_TokenSet(TokenType.Keyword, actual_TokenValue);

            }
            else if (comprobando && Convert.ToString(actual_TokenValue) == "true")
                Add_To_TokenSet(TokenType.Bool_True, actual_TokenValue);


            else if (comprobando && Convert.ToString(actual_TokenValue) == "false")
                Add_To_TokenSet(TokenType.Bool_False, actual_TokenValue);

            else
                Add_To_TokenSet(TokenType.Identifier, actual_TokenValue);


        }
        else if (actual_char == '=')
        {
            if (IsThat('='))
            {
                GetNextChar();
                actual_TokenValue = "==";
                Add_To_TokenSet(TokenType.Equal_Operator, actual_TokenValue);
                GetNextChar();

            }
            else if (IsThat('>'))
            {
                GetNextChar();
                actual_TokenValue = "=>";
                Add_To_TokenSet(TokenType.Arrow, actual_TokenValue);
                GetNextChar();

            }
            else
            {
                actual_TokenValue = "=";
                Add_To_TokenSet(TokenType.Asignation_Operator, actual_TokenValue);
                GetNextChar();

            }
        }
        else if (actual_char == '!')
        {
            if (IsThat('='))
            {

                GetNextChar();
                actual_TokenValue = "!=";
                Add_To_TokenSet(TokenType.Distinct, actual_TokenValue);
                GetNextChar();

            }
            else Error(actual_char+" No es un token valido");
        }
        else if (actual_char == '>')
        {
            if (IsThat('='))
            {
                GetNextChar();
                actual_TokenValue = ">=";
                Add_To_TokenSet(TokenType.More_Equal_Than, actual_TokenValue);
                GetNextChar();

            }
            else
            {
                actual_TokenValue = ">";
                Add_To_TokenSet(TokenType.More_Than, actual_TokenValue);
                GetNextChar();
            }
        }
        else if (actual_char == '<')
        {
            if (IsThat('='))
            {
                GetNextChar();
                actual_TokenValue = "<=";
                Add_To_TokenSet(TokenType.Min_Equal_Than, actual_TokenValue);
                GetNextChar();

            }
            else
            {
                actual_TokenValue = "" + actual_char;
                Add_To_TokenSet(TokenType.Min_Than, actual_TokenValue);
                GetNextChar();
            }
        }else{
            Error(actual_char+" No es un token válido");
        }          
            
    }

    public void Add_Function()
    {

    }
    public bool IsThat(char possible)
    {
        if (position + 1 != text_size && possible == Text[position + 1])
        {
            return true;
        }
        return false;
    }
    public bool ItsKeyword(string token)
    {//Es un metodo que devuelve verdadero si la palabra es una Keyword
        // System.Console.WriteLine("la palabra es " + token);
        foreach (var item in Keywords)
        {
            if (token == item)
            {
                return true;
            }

        }

        return false;
    }


    public void GetNextChar()
    {
        if (position == text_size - 1)
        {
            position++;
        }
        else
        {
            position++;
            actual_char = Text[position];
        }
    }
    public void Show_TokenSet()
    {
        foreach (var item in TokenSet)
        {
            item.Show();
        }
    }

}