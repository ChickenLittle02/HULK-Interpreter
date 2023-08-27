public enum TokenType
{
    Number, //todos los valores numericos
    Operator, //+, -, *, /
    Identifier, // todos los nombres
    Keyword, //Las palabras reservadas, number, string, bool, let, in, 
    Semicolon, //;
    Quotes_Text, // "mm"
    Punctuation, // Ver como asignamos estos valores
    Blank_Space,
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
    int i = 1;
    public List<Token> TokenSet { get; set; }
    public int position { get; set; }
    public char actual_char { get; set; }
    public string Text { get; set; }
    public int text_size;
    public TokenType actual_Tokentype { get; set; }

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
        int i = 0;
        while (i < 10 && position < text_size)
        {
            i++;
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
            actual_TokenValue += '"';
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
                actual_TokenValue += '"';
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
        else if (Char.IsLetter(actual_char) || actual_char == '_')
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
        else if (actual_char == '+' || actual_char == '-' || actual_char == '/' || actual_char == '*')
        {
            System.Console.WriteLine("Entro a que es un operador");
            actual_Tokentype = TokenType.Operator;

            actual_Token = new Token(actual_Tokentype, actual_char);
            TokenSet.Add(actual_Token);
            GetNextChar();
        }

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