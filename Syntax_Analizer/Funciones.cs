namespace Syntax_Analizer
{
    class Function
    {
        public string Name { get; private set; }
        public List<string> Variables { get; private set; }
        public List<Token> Tokens_Body { get; private set; }
        public Function(string name, List<string> variables, List<Token> tokens_body)
        {
            Name = name;
            Variables = variables;
            List<Token> Tokens_Body = tokens_body;
        }
    }
    partial class Syntax
    {
                
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
                    if (numero1 != TokenType.Number) Error("La funcion seno recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero1;
                case "cos":

                    //Parsea la expresion coge el valor y pasaselo al metodo
                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero2 = Expression();
                    if (numero2 != TokenType.Number) Error("La funcion coseno recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero2;
                case "sqrt":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero3 = Expression();
                    if (numero3 != TokenType.Number) Error("La funcion sqrt recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero3;
                case "exp":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero4 = Expression();
                    if (numero4 != TokenType.Number) Error("La funcion exp recibe como parametros un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return numero4;

                case "log":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    TokenType numero5 = Expression();
                    if (numero5 != TokenType.Number) Error("La funcion log recibe como primer parametro un tipo number");
                    Eat(TokenType.Comma);
                    TokenType numero6 = Expression();
                    if (numero5 != TokenType.Number) Error("La funcion log recibe como segundo parametro un tipo number");
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


    }
}