namespace Parser
{
    partial class Parser
    {
        bool Check_Function_Existence()
        {//Va iterando por el diccionario de funciones buscando si el nombre de la funcion existe

            for (int i = Function_State.Count - 1; i >= 0; i--)
            {
                if (Function_State[i] == actual_token_value.ToString()) return true;
                

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
                    double numero1;
                    if(!double.TryParse(Expression().ToString(),out numero1)) Error("Se esperaba un tipo number");
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    return Math.Sin(numero1*Math.PI/180);
                case "cos":
                    //Parsea la expresion coge el valor y pasaselo al metodo
                    Eat(TokenType.LEFT_PARENTHESIS);
                    object numero2 = Expression();
                    Eat(TokenType.RIGHT_PARENTHESIS);
                    double number = (double)numero2*Math.PI/180;
                    return Math.Cos(number);


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
                    Random random = new Random();
                    return random.NextDouble();

                default:
                    Eat(TokenType.LEFT_PARENTHESIS);

                    //Que me parsee las variables y por cada una que parsee guarde la cantidad, si son diferentes
                    //del lenght de la lista de variables que lance un error porque solo pueden haber n variables

                    Dictionary<string, object> Function_Variables = Make_Function_Variables(function_name);
                    Eat(TokenType.RIGHT_PARENTHESIS);

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