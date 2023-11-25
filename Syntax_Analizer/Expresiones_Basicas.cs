namespace Syntax_Analizer{
    partial class Syntax{
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
                System.Console.WriteLine("EN el Lexer actual_token donde empieza la expresion despues del else es " + position + "  " + Token_Set[position].Value + "  " + actual_token.Value);
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

    }
}