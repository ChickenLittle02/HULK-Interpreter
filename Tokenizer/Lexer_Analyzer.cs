
namespace Lexer_Analizer
{

    public partial class Tokenizer
    {//Esta clase crea el consjunto de tokens, la clase parser es la que tiene que verificar si el conjunto de tokens
     // funciona correctamente, es decir si por cada token, el token siguiente es valido
        public List<Token> TokenSet { get; set; }
        public int position { get; set; }
        public char actual_char { get; set; }
        public string Text { get; set; }
        public int text_size;
        public TokenType actual_Tokentype { get; set; }

        public string[] Keywords = { "let", "in", "if", "else", "function",
    "sqrt","cos","sin","exp","log","rand", "print"};
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
            else throw new Exception("Debe introducir algun código");
        }
        private void Error(string message)
        {
            throw new Exception(message);
        }
        public void Start()
        {
            while (position < text_size)
            {//Va por cada posicion del texto y dando una clasificacion a cada token
                Add_Simple_Token();
            }

            //El End of TOken se pone solamente si position se paso del text size
            Add_To_TokenSet(TokenType.EOT, "EOT");
        }

        public void Add_To_TokenSet(TokenType Type, object Value)
        {//Agrega al conjunto de tokens un nuevo token
            Token Element = new Token(Type, Value);
            TokenSet.Add(Element);

        }

        public bool IsThat(char possible)
        {//Comprueba si el char que esta en la posicion siguiente del string recibido es el esperado
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
        {//Actualiza el siguiente char en caso de ser una posicion valida
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
        {//Para mostrar el conjunto de tokens
            foreach (var item in TokenSet)
            {
                item.Show();
            }
        }

    }
}