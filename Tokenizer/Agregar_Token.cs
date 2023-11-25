namespace Lexer_Analizer{
    public partial class Tokenizer{
        public void Add_Simple_Token()
        {
            //Este va comprobando si es uno de los token que solo estan compuestos por un char,
            //Y si no es ninguno de ellos, entonces comprueba que sea uno de los tokens que tienen mas de un caracter

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
                case '|':

                    // System.Console.WriteLine("Entro a que es el operador de concatenar texto");
                    Add_To_TokenSet(TokenType.Or_Operator, actual_char);
                    GetNextChar();
                    break;
                case '&':
                    // System.Console.WriteLine("Entro a que es el operador de concatenar texto");
                    Add_To_TokenSet(TokenType.And_Operator, actual_char);
                    GetNextChar();
                    break;
                default:
                    Add_Compose_Token();
                    break;
            }

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
                    }

                    else Add_To_TokenSet(TokenType.Keyword, actual_TokenValue);

                }
                else if (comprobando && Convert.ToString(actual_TokenValue) == "true")
                    Add_To_TokenSet(TokenType.Boolean, actual_TokenValue);


                else if (comprobando && Convert.ToString(actual_TokenValue) == "false")
                    Add_To_TokenSet(TokenType.Boolean, actual_TokenValue);

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
                else
                    actual_TokenValue = "!";
                Add_To_TokenSet(TokenType.Not_Operator, actual_TokenValue);
                GetNextChar(); Error(actual_char + " No es un token valido");
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
            }
            else
            {
                Error(actual_char + " No es un token válido");
            }

        }
    }
    
}