using System.Security.AccessControl;
using System.Timers;
using System.Xml.XPath;

namespace Syntax_Analizer
{
    class Funciones
    {
        string Name { get; set; }
        List<string> Variables { get; set; }
        List<Token> Tokens_Body { get; set; }
        public Funciones(string name, List<string> variables, List<Token> tokens_body)
        {
            Name = name;
            Variables = variables;
            List<Token> Tokens_Body = tokens_body;
        }
    }
    class Syntax
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
        private void Error()
        {
            throw new Exception("Invalid syntax");
        }
        private void Error(string message)
        {
            throw new Exception(message);
        }
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

        private void Add_Function()
        {//las estructuras de las funciones son
         //function nombre_funcion(variables) => cuerpo_funcion;
            Eat(TokenType.Function_Keyword);
            Eat(TokenType.Identifier);//Nombre de la funcion
            string name = actual_token_value.ToString();

            Eat(TokenType.LEFT_PARENTHESIS);
            List<string> Var = Function_Variables();
            Eat(TokenType.RIGHT_PARENTHESIS);
            Eat(TokenType.Arrow);
            List<Token> body = Make_Body();
            Funciones New = new Funciones(name, Var, body);
            Function_State.Add((name, 1));

        }
        private List<string> Function_Variables()
        {
            List<string> Variable_ALL = new List<string>();
            Eat(TokenType.Identifier);
            string variable = actual_token_value.ToString();
            Variable_ALL.Add(variable);

            while (actual_token.Type == TokenType.Comma)
            {
                Eat(actual_token.Type);
                Eat(TokenType.Identifier);
                variable = actual_token_value.ToString();
                Variable_ALL.Add(variable);
            }

            return Variable_ALL;

        }

        private List<Token> Make_Body()
        {//Esto me crea una lista de token con el cuerpo de la funcion

            List<Token> Token_Body = new List<Token>();

            while (actual_token.Type != TokenType.Semicolon && actual_token.Type != TokenType.EOT)
            {
                Token_Body.Add(actual_token);
                GetNextToken();
            }

            if (actual_token.Type == TokenType.EOT) Error();

            Token_Body.Add(actual_token);

            GetNextToken();

            return Token_Body;
        }

        private TokenType Expression()
        {//Para que procese en caso de ser las expresiones booleanas unidas con & y |
            TokenType result = Bool_Op();
            bool Es_Bool = false;
            if (result == TokenType.Boolean)
            {
                Es_Bool = true;
            }
            while (actual_token.Type == TokenType.And_Operator || actual_token.Type == TokenType.Or_Operator)
            {
                if (Es_Bool)
                {
                    if (actual_token.Type == TokenType.And_Operator)
                    {
                        Eat(TokenType.And_Operator);
                        TokenType result1 = Bool_Op();
                        if (result1 != TokenType.Boolean) Error("El operador & tiene que tener a continuacion un tipo bool");

                    }
                    else if (actual_token.Type == TokenType.Or_Operator)
                    {
                        Eat(TokenType.Or_Operator);
                        TokenType result1 = Bool_Op();
                        if (result1 != TokenType.Boolean) Error("El operador | tiene que tener a continuacion un tipo bool");
                    }
                }
                else
                {
                    Error("El operador " + actual_token.Value + " tiene que estar precedido por un tipo bool");
                }
            }
            return result;
        }

        TokenType[] Bool_Oper =
        {
    TokenType.Equal_Operator,// ==
    TokenType.Distinct,// /!=
    TokenType.More_Than, //>
    TokenType.More_Equal_Than, //>=
    TokenType.Min_Than,// <
    TokenType.Min_Equal_Than// <=
    };

        private TokenType Bool_Op()
        {
            TokenType result = Text();
            int Operador = ItsBoolOp(actual_token.Type);
            //ItsBoolOp comprueba que tipo de operador es en el que estoy parado, para saber si es uno bool o no

            // if (Operador < Bool_Oper.Length)
            // {
            //     System.Console.WriteLine("Dice que es el operador " + Bool_Oper[Operador]);
            // }
            // else { System.Console.WriteLine("No es ninguno de los operadores booleanos"); }

            //Comprueba que sea alguno de los operadores booleanos
            /*
                0   ==
                1  /!=
                2  >
                3  >=
                4  <
                5  <=*/
            while (Operador < Bool_Oper.Length)
            {
                if (Operador == 0)
                {

                    Eat(TokenType.Equal_Operator);
                    TokenType result2 = Text();
                    if (result != result2) Error("El operador == tiene que tener el mismo tipo en ambos miembros");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);

                }

                if (Operador == 1)
                {
                    Eat(TokenType.Distinct);
                    TokenType result2 = Text();
                    if (result != result2) Error("El operador != tiene que tener el mismo tipo en ambos miembros");

                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);

                }
                if (Operador == 2)
                {
                    if (result != TokenType.Number) Error("El operador > tiene que estar precedido por un tipo number");
                    Eat(TokenType.More_Than);
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number) Error("El operador > tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);
                }
                if (Operador == 3)
                {
                    if (result != TokenType.Number) Error("El operador >= tiene que estar precedido por un tipo number");
                    Eat(TokenType.More_Equal_Than);
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number) Error("El operador >= tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);

                }
                if (Operador == 4)
                {
                    if (result != TokenType.Number) Error("El operador < tiene que estar precedido por un tipo number");
                    Eat(TokenType.Min_Than);
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number) Error("El operador < tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);
                }
                if (Operador == 5)
                {
                    if (result != TokenType.Number) Error("El operador >= tiene que estar precedido por un tipo number");
                    Eat(TokenType.Min_Equal_Than);
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number) Error("El operador >= tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);
                }

            }
            return result;
        }

        int ItsBoolOp(TokenType Type)
        {
            for (int i = 0; i < Bool_Oper.Length; i++)
            {
                if (Type == Bool_Oper[i]) return i;
            }

            return Bool_Oper.Length;

        }
        private TokenType Text()
        {

            TokenType result = Form();
            while (actual_token.Type == TokenType.CONCAT_OPERATOR)
            {//NO hay que comprobar tipos, pues en la jerarquia se pueden concatenar valores de diferentes tipos

                Eat(TokenType.CONCAT_OPERATOR);
                TokenType result2 = Form();
            }

            return result;
        }

        private TokenType Form()
        {
            TokenType result = Exp();
            while (actual_token.Type == TokenType.SUM_Operator || actual_token.Type == TokenType.REST_Operator)
            {//COmprueba que este sumando y restando tipos numericos

                if (actual_token.Type == TokenType.SUM_Operator)
                {
                    if (!(result is TokenType.Number)) Error("Antes de un simbolo de + se espera un tipo number");
                    Eat(TokenType.SUM_Operator);
                    TokenType result2 = Exp();
                    if (!(result2 is TokenType.Number)) Error("Despues de un simbolo de + se espera un tipo number");
                }
                if (actual_token.Type == TokenType.REST_Operator)
                {

                    if (!(result is TokenType.Number)) Error("Antes de un simbolo de - se espera un tipo number");
                    Eat(TokenType.REST_Operator);
                    TokenType result2 = Exp();
                    if (!(result2 is TokenType.Number)) Error("Despues de un simbolo de - se espera un tipo number");
                }
            }
            return result;
        }

        private TokenType Exp()
        {
            TokenType result = Pow();

            while (actual_token.Type == TokenType.MULT_Operator || actual_token.Type == TokenType.DIV_Operator)
            {//Comprueba que este multiplicando y dividiendo tipos numericos

                if (actual_token.Type == TokenType.MULT_Operator)
                {
                    if (!(result is TokenType.Number)) Error("Antes de un simbolo de * se espera un tipo number");
                    Eat(TokenType.MULT_Operator);
                    TokenType result2 = Pow();
                    if (!(result2 is TokenType.Number)) Error("Despues de un simbolo de * se espera un tipo number");
                }
                if (actual_token.Type == TokenType.DIV_Operator)
                {
                    if (!(result is TokenType.Number)) Error("Antes de un simbolo de / se espera un tipo number");
                    Eat(TokenType.DIV_Operator);
                    TokenType result2 = Pow();
                    if (!(result2 is TokenType.Number)) Error("Despues de un simbolo de / se espera un tipo number");
                }
            }
            return result;
        }
        private TokenType Pow()
        {//COmprueba que en caso de que el operador sea el de potencia, el token con el que se esta evaluando sea numerico
            TokenType result = Numb();
            if (actual_token.Type == TokenType.POW_Operator)
            {
                if (!(result is TokenType.Number)) Error("Antes de un simbolo de ^ se espera un tipo number");
                Eat(TokenType.POW_Operator);

                TokenType result2 = Pow();
                if (!(result2 is TokenType.Number)) Error("Despues de un simbolo de ^ se espera un tipo number");
            }
            //SI entra al if y sale sin problemas es porque el tipo de result es Numb, y si no, el tipo es cualquier otro
            return result;
        }
        private TokenType Numb()
        {
            actual_token.Show();

            if (actual_token.Type == TokenType.Number)
            {
                Eat(TokenType.Number);
                return TokenType.Number;
            }
            else if (actual_token.Type == TokenType.SUM_Operator)
            {
                Eat(TokenType.SUM_Operator);
                TokenType result = Expression();
                if (result != TokenType.Number) Error("Despues del operador + se espera un tipo Number");
                Eat(TokenType.Number);
                return TokenType.Number;
            }
            else if (actual_token.Type == TokenType.REST_Operator)
            {
                Eat(TokenType.REST_Operator);
                TokenType result = Expression();
                if (result != TokenType.Number) Error("Despues del operador - se espera un tipo Number");
                Eat(TokenType.Number);
                return TokenType.Number;
            }
            else if (actual_token.Type == TokenType.Boolean)
            {
                Eat(TokenType.Boolean);
                return TokenType.Boolean;
            }
            else if (actual_token.Type == TokenType.Quotes_Text)
            {
                Eat(TokenType.Quotes_Text);
                return TokenType.Quotes_Text;
            }
            else if (actual_token.Value.ToString() == "if")
            {
                int if_token_pos = position;
                Eat(TokenType.Keyword);
                Eat(TokenType.LEFT_PARENTHESIS);
                TokenType decision = Expression();
                if (decision != TokenType.Boolean) Error("Debe ir una expresion booleana");
                Eat(TokenType.RIGHT_PARENTHESIS);
                TokenType result1 = Expression();
                if (!(actual_token.Value.ToString() == "else")) Error("Se esperaba un else");
                Eat(TokenType.Keyword);
                System.Console.WriteLine("EN el Lexer actual_token donde empieza la expresion despues del else es "+position+"  "+Token_Set[position].Value+"  "+actual_token.Value);
                int else_pos = position;//Posicion a partir de la cual tengo que parsear la expresion del else
                TokenType result2 = Expression();
                int final_if_else = position;
                //Posicion en la que me quedo despues de parsear todo el if-else
                Token_Set[if_token_pos].Set_Inicio_FInal(else_pos, final_if_else);
                return result1;

            }
            else if (actual_token.Type == TokenType.Let_Keyword)
            {
                Eat(TokenType.Let_Keyword);
                Dictionary<string, TokenType> Var_Subset = Variables_Subset();
                Variables_Set.Add(Var_Subset);
                variable_subset++;
                Eat(TokenType.In_Keyword);
                TokenType result = Expression();
                //System.Console.WriteLine("No he eliminado las variables");
                Variables_Set.RemoveAt(variable_subset);
                //System.Console.WriteLine("Ya eliminé las variables");
                variable_subset--;
                return result;

            }
            else if (actual_token.Type == TokenType.Not_Operator)
            {
                Eat(TokenType.Not_Operator);
                TokenType result = Expression();
                if (!(result == TokenType.Boolean)) Error("No se puede aplicar el operador ! a un tipo " + result.GetType());

                return result;

            }
            else if (actual_token.Type == TokenType.Identifier)
            {
                Eat(TokenType.Identifier);
                //Comprobar si es una variable
                (TokenType, bool) Existence = Check_Var_Existence();

                if (Existence.Item2)
                {
                    System.Console.WriteLine("la variable es " + Existence.Item1);
                    return Existence.Item1;
                }
                else
                {
                    //Si no es una variable es una funcion
                    //Por ahora vamos a pasar error porque no hemos implementado las funciones
                    Error();
                }

                return Variables_Set[variable_subset][actual_token_value.ToString()];

            }
            else
            {
                if (actual_token.Value.ToString() == "else") Error();
                if (actual_token.Value.ToString() == "in") Error();
                Eat(TokenType.LEFT_PARENTHESIS);
                TokenType result = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);
                return result;
            }

        }

        private bool IsTrue(object choise)
        {
            if (choise is true) return true;

            return false;
        }

        private Dictionary<string, TokenType> Variables_Subset()
        {
            Dictionary<string, TokenType> Var_Set = new Dictionary<string, TokenType>();

            (string, TokenType) Var = Variable();
            Var_Set.Add(Var.Item1, Var.Item2);

            while (actual_token.Type == TokenType.Comma)
            {
                Eat(TokenType.Comma);
                Var = Variable();
                Var_Set.Add(Var.Item1, Var.Item2);
            }

            return Var_Set;
        }

        private (string, TokenType) Variable()
        {
            Eat(TokenType.Identifier);
            string variable_name = actual_token_value.ToString();
            Eat(TokenType.Asignation_Operator);


            if (actual_token.Value == "let") Error();

            TokenType value = Expression();
            return (variable_name, value);

        }

        (TokenType, bool) Check_Var_Existence()
        {//Comprueba por cada posicion del diccionario de variables si la variable existe
            
            for (int i = variable_subset; i >= 0; i--)
            {

                if (Variables_Set[i].ContainsKey(actual_token_value.ToString()))
                {
                    System.Console.WriteLine(actual_token_value + "  existe en " + i);
                    return (Variables_Set[i][actual_token_value.ToString()], true);
                }

            }
            return (TokenType.nul, false);
        }

        TokenType Choosing_Function(string function_name)
        {
            //Metodo para procesar funciones, primero descarta que sea una de las globales y 
            //después va para las funciones temporales
            switch (function_name)
            {
                case "sin":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero1 = Expression();
                    if(numero1!= TokenType.Number) Error("La funcion seno recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero1;
                case "cos":

                    //Parsea la expresion coge el valor y pasaselo al metodo
                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero2 = Expression();
                    if(numero2!= TokenType.Number) Error("La funcion coseno recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero2;
                case "sqrt":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero3 = Expression();
                    if(numero3!= TokenType.Number) Error("La funcion sqrt recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero3;
                case "exp":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero4 = Expression();
                    if(numero4!= TokenType.Number) Error("La funcion exp recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero4;

                case "log":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero5 = Expression();
                    if(numero5!= TokenType.Number) Error("La funcion log recibe como primer parametro un tipo number");
                    Eat(TokenType.Comma);
                    TokenType numero6 = Expression();
                    if(numero5!= TokenType.Number) Error("La funcion log recibe como segundo parametro un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero5;
                case "print":
                //Se toma como la funcion identidad
                TokenType result = Expression();
                return result;

                case "rand":
                return TokenType.Number;

                default:
                
                Eat(TokenType.LEFT_PARENTHESIS);

                //Que me parsee las variables y por cada una que parsee guarde la cantidad, si son diferentes
                //del lenght de la lista de variables que lance un error porque solo pueden haber n variables

                Dictionary<string, TokenType> Function_Variables = Make_Function_Variables(function_name);
                Eat(TokenType.RIGHT_PARENTHESIS);

                System.Console.WriteLine("Llego a aaqui ");
                System.Console.WriteLine("Variables de la funcion");
                foreach (var item in Function_Variables)
                {
                    System.Console.WriteLine(item.Key + "  " + item.Value);
                }
                //Ahora con las variables procesa el cuerpo de la funcion, recibe los tokens, las variables y las funciones
                Syntax Parse_Function = new Syntax(New_Functions[function_name].Tokens_Body, Function_Variables, New_Functions);
                TokenType resultado = Parse_Function.Start();
                return resultado;

            }
        }

    private Dictionary<string, TokenType> Make_Function_Variables(string name)
    {//Va construyendo los valores de cada variable
        Dictionary<string, TokenType> Function_Variables = new Dictionary<string, TokenType>();
        List<TokenType> Values = new List<TokenType>();
        TokenType var_value = Expression();
        Values.Add(var_value);
        while (actual_token.Type == TokenType.Comma)
        {
            Eat(TokenType.Comma);
            var_value = Expression();
            Values.Add(var_value);
        }
        int total_var = New_Functions[name].Variables.Count;

        if (total_var != Values.Count) Error("La funcion " + name + " tiene que tener " + total_var + " variables en su declaracion");

        for (int i = 0; i < total_var; i++)
        {
            Function_Variables.Add(New_Functions[name].Variables[i], Values[i]);
        }

        return Function_Variables;

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
