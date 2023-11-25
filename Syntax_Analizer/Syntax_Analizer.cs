using System.Security.AccessControl;
using System.Timers;
using System.Xml.XPath;

namespace Syntax_Analizer
{
    partial class Syntax
    {
        List<Token> Token_Set { get; set; }
        Token actual_token { get; set; }
        object actual_token_value { get; set; }
        private int position { get; set; }
        int size { get; set; }
        List<Dictionary<string, TokenType>> Variables_Set { get; set; }
        Dictionary<string, Function> New_Functions { get; set; }
        //Aqui se encuentran todas las funciones agregadas

        //FUnction_State me dice el estado de la funcion, si es 0 es xq es del sistema y si es 1 es porque la agregué
        List<(string, int)> Function_State = new List<(string, int)>
    {("sqrt",0),("cos",0),("sin",0),("exp",0),("log",0),("rand",0)};
        int variable_subset { get; set; }
        public Syntax(List<Token> token_Set)
        {
            Token_Set = token_Set;
            position = 0;
            size = Token_Set.Count();
            Variables_Set = new List<Dictionary<string, TokenType>>();
            variable_subset = -1;
            if (position != size)
            {
                actual_token = Token_Set[position];
            }
            else
            {
                actual_token = null;
            }
        }

        public Syntax(List<Token> token_Set, Dictionary<string, TokenType> Variables, Dictionary<string, Function> Functions)
        {//Para los casos en los que voy a procesar la funcion, que necesito que reciba las variables,
         // las funciones que existen, y 
            Token_Set = token_Set;
            position = 0;
            size = Token_Set.Count();
            Variables_Set = new List<Dictionary<string, TokenType>>();
            Variables_Set.Add(Variables);
            variable_subset = 0;
            New_Functions = Functions;
            if (position != size)
            {
                actual_token = Token_Set[position];
            }
            else
            {
                actual_token = null;
            }
        }


        private void Error()
        {
            throw new Exception("Invalid syntax");
        }
        private void Error(string message)
        {
            throw new Exception(message);
        }


        

        public TokenType Start()
        {
            TokenType result = Expression();
            System.Console.WriteLine("El resultado es ");
            System.Console.WriteLine(result);

            if (position == size)
            {
                System.Console.WriteLine("Parser exitoso");
            }
            else
            {
                System.Console.WriteLine("Salio mal algo");
            }

            return result;
        }


        private void Eat(TokenType Type)
        {
            System.Console.WriteLine(actual_token.Type + "  " + actual_token.Value + " type debe ser " + Type);
            if (Type == actual_token.Type)
            {
                actual_token_value = actual_token.Value;
                GetNextToken();
            }
            else
            {
                Error();
            }
        }

        private void GetNextToken()
        {
            if (position == size - 1)
            {
                position++;
                actual_token = new Token(TokenType.EOT, "EOT");
            }
            else if (position == size)
            {

            }
            else
            {
                position++;
                actual_token = Token_Set[position];
            }

        }
    }
}
