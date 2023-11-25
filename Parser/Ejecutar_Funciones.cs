namespace Parser
{
    partial class Parser
    {
        bool Check_Function_Existence()
        {//Va iterando por el diccionario de funciones buscando si el nombre de la funcion existe
         // System.Console.WriteLine("Function_State " + Function_State.Count);
            for (int i = Function_State.Count - 1; i >= 0; i--)
            {
                //System.Console.WriteLine(Function_State[i]);
                if (Function_State[i] == actual_token_value.ToString())
                {
                    //      System.Console.WriteLine(actual_token_value + "  existe en " + i);
                    return true;
                }

            }
            return false;
        }

        object Choosing_Function(string function_name)
        {
            //Metodo para procesar funciones, primero descarta que sea una de las globales y 
            //después va para las funciones temporales

            switch (function_name)
            {
                case "sin":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    object numero1 = Expression();
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return Math.Sin((double)numero1);
                case "cos":
                    //Parsea la expresion coge el valor y pasaselo al metodo
                    Eat(TokenType.LEFT_PARENTHESIS);
                    object numero2 = Expression();
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return Math.Cos((double)numero2);


                case "sqrt":
                    //Parsea la expresion coge el valor y pasaselo al metodo

                    Eat(TokenType.LEFT_PARENTHESIS);
                    object numero3 = Expression();
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return Math.Sqrt((double)numero3);

                case "exp":
                    //Parsea la expresion coge el valor y pasaselo al metodo


                    Eat(TokenType.LEFT_PARENTHESIS);
                    object numero4 = Expression();
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return Math.Pow(Math.E, (double)numero4);

                case "log":
                    //Parsea la expresion coge el valor y pasaselo al metodo


                    Eat(TokenType.LEFT_PARENTHESIS);
                    object basis = Expression();
                    Eat(TokenType.Comma);
                    object argumento = Expression();
                    Eat(TokenType.RIGHT_PARENTHESIS);

                    return Math.Log((double)basis, (double)argumento);

                case "print":
                    //Se toma como la funcion identidad

                    Eat(TokenType.LEFT_PARENTHESIS);
                    object ToPrint = Expression();
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return ToPrint;

                case "rand":

                    Eat(TokenType.LEFT_PARENTHESIS);
                    Eat(TokenType.RIGHT_PARENTHESIS);

                    return 0;

                default:
                    Eat(TokenType.LEFT_PARENTHESIS);

                    //Que me parsee las variables y por cada una que parsee guarde la cantidad, si son diferentes
                    //del lenght de la lista de variables que lance un error porque solo pueden haber n variables

                    Dictionary<string, object> Function_Variables = Make_Function_Variables(function_name);
                    Eat(TokenType.RIGHT_PARENTHESIS);

                    System.Console.WriteLine("Llego a aaqui ");
                    System.Console.WriteLine("Variables de la funcion");
                    foreach (var item in Function_Variables)
                    {
                        System.Console.WriteLine(item.Key + "  " + item.Value);
                    }
                    //Ahora con las variables procesa el cuerpo de la funcion
                    Parser Parse_Function = new Parser(New_Functions[function_name].Tokens_Body, Function_Variables, New_Functions);
                    object result = Parse_Function.Start();
                    return result;


            }
        }


        private Dictionary<string, object> Make_Function_Variables(string name)
        {//Va construyendo los valores de la variable
            Dictionary<string, object> Function_Variables = new Dictionary<string, object>();
            List<object> Values = new List<object>();
            object var_value = Expression();
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