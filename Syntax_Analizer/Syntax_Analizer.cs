using System.Linq.Expressions;
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
        bool EstoyAnalizando { get; set; }//Esto es para cuando vaya a analizar sintacticamente el cuerpo de una funcion
        List<Dictionary<string, TokenType>> Variables_Set { get; set; }
        Dictionary<string, Function> New_Functions { get; set; }
        //Aqui se encuentran todas las funciones agregadas
        List<string> System_Function = new List<string> {"print", "sqrt", "cos", "sin", "exp", "log", "rand" };
        //Las funciones del sistema
        int variable_subset { get; set; }

        public Syntax(List<Token> token_Set, Dictionary<string, Function> new_functions)
        {
            Token_Set = token_Set;
            position = 0;
            size = Token_Set.Count();
            Variables_Set = new List<Dictionary<string, TokenType>>();
            variable_subset=-1;
            AddSystemVariables();
            New_Functions = new_functions;
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
        {
            //Este constructor solo se utiliza en el casos en que voy a procesar la funcion, 
            //que necesito que reciba las variables,
            // las funciones que existen, y 
            Token_Set = token_Set;
            position = 0;
            size = Token_Set.Count();
            Variables_Set = new List<Dictionary<string, TokenType>>();
            EstoyAnalizando = AreAllNul();//Para saber que en este caso estoy analizando la funcion
            variable_subset = -1;
            AddSystemVariables();
            Variables_Set.Add(Variables);
            variable_subset++;
            New_Functions = Functions;
            if (position != size)
            {
                actual_token = Token_Set[position];
            }
            else
            {
                actual_token = null;
            }
            bool AreAllNul()
            {
                foreach (var item in Variables)
                {
                    if (item.Value == TokenType.nul) return true;
                }

                return false;
            }
        }



        
        private void Error(string message)
        {
            throw new Exception("Syntax error: "+message);
        }



        public TokenType Start()
        {
            TokenType result = TokenType.nul;
            if (actual_token.Type == TokenType.Function_Keyword) Add_Function();
            else
            {
                result = Expression();
                Eat(TokenType.Semicolon,"La expresión principal debe concluir con un punto y coma");
                if (actual_token.Type != TokenType.EOT) throw new Exception("Después de un punto y coma no puede haber ninguna otra expresion");
            }

            

            return result;

        }


        private void Eat(TokenType Type,string message)
        {
            if (Type == actual_token.Type)
            {
                actual_token_value = actual_token.Value;
                GetNextToken();
            }
            else
            {
                Error(message);
            }
        }

        private void GetNextToken()
        {

            if (position >= size - 1)
            {
                actual_token = new Token(TokenType.None, " ");
                position++;
            }
            else
            {
                position++;
                actual_token = Token_Set[position];
            }

        }

        private bool IsNext(TokenType Expected_Type)
        {
            return actual_token.Type == Expected_Type;
        }

        private void AddSystemVariables()
        {
            Dictionary<string,TokenType> SystemVars = new Dictionary<string, TokenType>();
            SystemVars.Add("PI",TokenType.Number);
            Variables_Set.Add(SystemVars);
            variable_subset++;

        }


    }
}
