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
    Identifier, // todos los nombres
    Keyword, //Las palabras reservadas, number, string, bool, let, in, 
    Bool_True,
    Bool_False,
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

    public string[] Keywords = { "let", "in", "number", "string", "bool", "if", "else" };
    public string actual_TokenValue { get; set; }
    public Token actual_Token { get; set; }
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
            System.Console.WriteLine("Isn't possible to find a token, the code is empty");
        }
    }
    public void Start()
    {
        while (position < text_size)
        {
            AddToken();
        }
    }

    public void AddToken()
    {
        if (Char.IsDigit(actual_char))
        {
            System.Console.WriteLine("Entro a que es un digito");
            actual_TokenValue = "";
            actual_Tokentype = TokenType.Number;
            while (position < text_size && Char.IsDigit(actual_char))
            {
                actual_TokenValue += actual_char;
                GetNextChar();
            }
            if (Char.IsLetter(actual_char))
            {
                System.Console.WriteLine("Error, there should be a number");
                //Como puedo hacer para que en este punto el interprete deje de correr
                //Porque hubo un error
                return;
            }
            actual_Token = new Token(actual_Tokentype, actual_TokenValue);

            TokenSet.Add(actual_Token);
        }
        else if (actual_char == ';')
        {
            System.Console.WriteLine("Entro a que es un punto y coma");
            actual_Token = new Token(TokenType.Semicolon, ';');
            TokenSet.Add(actual_Token);
            GetNextChar();
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
                System.Console.WriteLine("Error, falta " + '"');
                return;
            }
            else
            {
                actual_Token = new Token(actual_Tokentype, actual_TokenValue);
                TokenSet.Add(actual_Token);
                GetNextChar();
            }
        }
        else if (actual_char == ' ')
        {
            System.Console.WriteLine("Entro a que es un espacio");
            actual_Tokentype = TokenType.Blank_Space;
            GetNextChar();
        }
        else if (actual_char == '_')
        {
            System.Console.WriteLine("Entro a que es una letra, o un _");
            actual_TokenValue = "" + actual_char;
            actual_Tokentype = TokenType.Identifier;
            GetNextChar();
            while (position < text_size && (Char.IsLetterOrDigit(actual_char) || actual_char == '_'))
            {
                actual_TokenValue += actual_char;
                GetNextChar();
            }
            actual_Token = new Token(actual_Tokentype, actual_TokenValue);
            TokenSet.Add(actual_Token);
        }
        else if (Char.IsLetter(actual_char))
        {
            bool comprobando = true;
            System.Console.WriteLine("Entro a que es una letra");
            actual_TokenValue = "" + actual_char;
            GetNextChar();
            while (position < text_size && (Char.IsLetterOrDigit(actual_char) || actual_char == '_'))
            {
                if (actual_char == '_') comprobando = false;
                actual_TokenValue += actual_char;
                GetNextChar();
            }

            if (comprobando && ItsKeyword(actual_Tokentype))
            {System.Console.WriteLine("Entro a que es Keyword y el valor de ItsNotKeyword es "+ItsKeyword(actual_Tokentype));
                actual_Tokentype = TokenType.Keyword;
                actual_Token = new Token(actual_Tokentype, actual_TokenValue);
                TokenSet.Add(actual_Token);
            }
            else if (comprobando || Convert.ToString(actual_TokenValue) == "true")
            {
                actual_Tokentype = TokenType.Bool_True;
                actual_Token = new Token(actual_Tokentype, actual_TokenValue);
                TokenSet.Add(actual_Token);
            }
            else if (comprobando || Convert.ToString(actual_TokenValue) == "true")
            {
                actual_Tokentype = TokenType.Bool_False;
                actual_Token = new Token(actual_Tokentype, actual_TokenValue);
                TokenSet.Add(actual_Token);

            }
            else
            {
                actual_Tokentype = TokenType.Identifier;
                actual_Token = new Token(actual_Tokentype, actual_TokenValue);
                TokenSet.Add(actual_Token);
            }

        }
        else if (actual_char == '+')
        {
            System.Console.WriteLine("Entro a que es un operador");
            actual_Tokentype = TokenType.SUM_Operator;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();
        }
        else if (actual_char == '-')
        {
            System.Console.WriteLine("Entro a que es un operador");
            actual_Tokentype = TokenType.REST_Operator;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();
        }
        else if (actual_char == '/')
        {
            System.Console.WriteLine("Entro a que es un operador");
            actual_Tokentype = TokenType.DIV_Operator;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();
        }
        else if (actual_char == '*')
        {
            System.Console.WriteLine("Entro a que es un operador");
            actual_Tokentype = TokenType.MULT_Operator;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();
        }
        else if (actual_char == '(')
        {
            System.Console.WriteLine("Entro a que es un parentesis izquierdo");
            actual_Tokentype = TokenType.LEFT_PARENTHESIS;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();
        }
        else if (actual_char == ')')
        {
            System.Console.WriteLine("Entro a que es un parentesis derecho");
            actual_Tokentype = TokenType.RIGHT_PARENTHESIS;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();
        }
        else if (actual_char == '^')
        {
            System.Console.WriteLine("Entro a que es el simbolo de potencia");
            actual_Tokentype = TokenType.POW_Operator;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();

        }
        else if (actual_char == '@')
        {

            System.Console.WriteLine("Entro a que es el operador de concatenar texto");
            actual_Tokentype = TokenType.CONCAT_OPERATOR;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();

        }


    }
    public bool ItsKeyword(TokenType token)
    {//Es un metodo que devuelve verdadero si la palabra es una Keyword
        string word = token.ToString();
        foreach (var item in Keywords)
        {
            if (word == item)
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