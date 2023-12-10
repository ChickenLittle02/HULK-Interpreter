using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.XPath;
namespace Parser
{
    partial class Parser
    {
        List<Token> Token_Set { get; set; }
        Token actual_token { get; set; }
        object actual_token_value { get; set; }
        private int position { get; set; }
        int size { get; set; }
        List<Dictionary<string, object>> Variables_Set { get; set; }
        Dictionary<string, Function> New_Functions { get; set; }
        //Aqui se encuentran todas las funciones agregadas
        int variable_subset { get; set; }


        List<string> Function_State = new List<string>
    {"sqrt","cos","sin","exp","log","rand"};
        //FUnction_State me dice quienes son las funciones que tengo
        public Parser(List<Token> token_Set, Dictionary<string, object> Var_Subset, Dictionary<string, Function> new_functions)
        {
            Token_Set = token_Set;
            position = 0;
            size = Token_Set.Count();
            variable_subset = -1;

            Variables_Set = new List<Dictionary<string, object>>();
            AddSystemVariables();
            Variables_Set.Add(Var_Subset);
            variable_subset++;

            New_Functions = new_functions;
            Renovando_Funciones();
            if (position != size)
            {
                actual_token = Token_Set[position];
            }
            else
            {
                actual_token = null;
            }
        }

        public Parser(List<Token> token_Set, Dictionary<string, Function> new_functions)
        {//Revisar que creo que hay uno de los constructores que se pueden eliminar
            Token_Set = token_Set;
            position = 0;
            size = Token_Set.Count();
            Variables_Set = new List<Dictionary<string, object>>();
            variable_subset = -1;
            AddSystemVariables();
            New_Functions = new_functions;
            Renovando_Funciones();
            if (position != size)
            {
                actual_token = Token_Set[position];
            }
            else
            {
                actual_token = null;
            }
        }
        private void Renovando_Funciones()
        {

            if (New_Functions.Count != 0)
            {
                foreach (var item in New_Functions)
                {
                    Function_State.Add(item.Key);
                }
            }
        }
        public object Start()
        {
            object result = Expression();
            return result;
        }

        private void Error()
        {//Intentar quitar este error sin mensaje, y recordar probar que pasa si divido 1/0 y se lo sumo a otro numero
            throw new Exception("Invalid syntax");
        }

        private void Error(string message)
        {
            throw new Exception(message);
        }

        private void Eat(TokenType Type)
        {
            //    System.Console.WriteLine(actual_token.Type + "  " + actual_token.Value + " type debe ser " + Type);
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

        private bool IsNext(TokenType Expected_Type)
        {
            return actual_token.Type == Expected_Type;
        }

        private Dictionary<string, object> Variables_Subset()
        {
            Dictionary<string, object> Var_Set = new Dictionary<string, object>();

            (string, object) Var = Variable();
            Var_Set.Add(Var.Item1, Var.Item2);

            while (actual_token.Type == TokenType.Comma)
            {
                Eat(TokenType.Comma);
                Var = Variable();
                Var_Set.Add(Var.Item1, Var.Item2);
            }

            return Var_Set;
        }

        private (string, object) Variable()
        {
            Eat(TokenType.Identifier);

            string variable_name = actual_token_value.ToString();
            Eat(TokenType.Asignation_Operator);
            object value = null;

            if (actual_token.Value.ToString() == "let") Error("Si va a declarar una expresión let dentro de una variable debe hacerla entre parentesis");

            value = Expression();
            return (variable_name, value);

        }

        (object, bool) Check_Var_Existence()
        {

            for (int i = variable_subset; i >= 0; i--)
            {
                if (Variables_Set[i].ContainsKey(actual_token_value.ToString()))
                    return (Variables_Set[i][actual_token_value.ToString()], true);
            }
            return ("", false);
        }


        private void AddSystemVariables()
        {
            Dictionary<string,object> SystemVars = new Dictionary<string, object>();
            SystemVars.Add("PI",Math.PI);
            Variables_Set.Add(SystemVars);
            variable_subset++;

        }





    }
}