namespace Parser{
    partial class Parser{
        private object Numb()
    {
        actual_token.Show();

        if (actual_token.Type == TokenType.Number)
        {
            Eat(TokenType.Number);
            object result = Convert.ToDouble(actual_token_value);
            return result;
        }
        else if (actual_token.Type == TokenType.SUM_Operator)
        {
            Eat(TokenType.SUM_Operator);
            object result = Expression();
            return result;
        }
        else if (actual_token.Type == TokenType.REST_Operator)
        {
            Eat(TokenType.REST_Operator);
            double result = 0 - Convert.ToDouble(Expression());
            return result;
        }
        else if (actual_token.Type == TokenType.Boolean)
        {//Puede ser true o false
            Eat(TokenType.Boolean);
            object result = Convert.ToBoolean(actual_token_value);
            return result;
        }
        else if (actual_token.Type == TokenType.Quotes_Text)
        {
            Eat(TokenType.Quotes_Text);
            object result = actual_token_value;
            return result;
        }
        else if (actual_token.Value.ToString() == "if")
        { //System.Console.WriteLine("Entro a que es un if");
            Token if_token = actual_token;
            Eat(TokenType.Keyword);
            object decision = Expression();
            if (decision is true)
            {
                object result = Expression();
                position = if_token.final_else - 1;
                GetNextToken();
                return result;

            }
            else
            {
                position = if_token.inicio_else-1;
                //Esta es la posicion del else correspondiente a este if
                GetNextToken();
                System.Console.WriteLine("Este token es " + actual_token.Value);
                object result2 = Expression();
                return result2;
            }



        }
        else if (actual_token.Type == TokenType.Let_Keyword)
        {
            Eat(TokenType.Let_Keyword);
            Dictionary<string, object> Var_Subset = Variables_Subset();
            Variables_Set.Add(Var_Subset);
            variable_subset++;
            Eat(TokenType.In_Keyword);
            object result = Expression();
            //          System.Console.WriteLine("No he eliminado las variables");
            Variables_Set.RemoveAt(variable_subset);
            //      System.Console.WriteLine("Ya eliminé las variables");
            variable_subset--;
            return result;

        }
        else if (actual_token.Type == TokenType.Identifier)
        {
            Eat(TokenType.Identifier);
            //Comprobar si es una funcion
            //      System.Console.WriteLine("Quejeto");
            System.Console.WriteLine("Antes de comprobar lo del parentesis para entrar en que sea o no una funcion");
            actual_token.Show();
            if (IsNext(TokenType.LEFT_PARENTHESIS))
            {
                //Es porque es una funcion
                string function_name = actual_token_value.ToString();
                bool Existence1 = Check_Function_Existence();
                if (!Existence1) Error("Esta funcion no existe");

                object result = Choosing_Function(function_name);

                return result;
            }
            //si no es una funcion entonces es una variable
            (object, bool) Existence = Check_Var_Existence();
            if (!Existence.Item2) Error("La variable no existe en este entorno");
            //     System.Console.WriteLine("la variable es " + Existence.Item1);

            return Existence.Item1;

        }
        else if (actual_token.Type == TokenType.Not_Operator)
        {
            Eat(TokenType.Not_Operator);
            object result = Expression();
            //            if(!(result is bool)) Error("Despues del operador ! se espera un tipo bool");
            return !Convert.ToBoolean(result);
        }
        else
        {
            if (actual_token.Value.ToString() == "else") Error("Antes de un else siempre debe existir un if");
            if (actual_token.Value.ToString() == "in") Error("Antes de un in siempre debe existir un let");
            Eat(TokenType.LEFT_PARENTHESIS);
            object result = Expression();
            Eat(TokenType.RIGHT_PARENTHESIS);
            return result;
        }

    }
    }
}