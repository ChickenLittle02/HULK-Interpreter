namespace Parser
{
    partial class Parser
    {
        private object Numb()
        {
            if (actual_token.Type == TokenType.Number)
            {
                Eat(TokenType.Number);

                double Conversion;
                if (!double.TryParse(actual_token_value.ToString(), out Conversion)) Error("Se esperaba un tipo number");
                return Conversion;
            }
            else if (actual_token.Type == TokenType.SUM_Operator)
            {
                Eat(TokenType.SUM_Operator);

                double Conversion;
                if (!double.TryParse(Numb().ToString(), out Conversion)) Error("Se esperaba un tipo number");
                return Conversion;
            }
            else if (actual_token.Type == TokenType.SUBSTRACTION_Operator)
            {
                Eat(TokenType.SUBSTRACTION_Operator);

                double Conversion;
                if (!double.TryParse(Numb().ToString(), out Conversion)) Error("Se esperaba un tipo number");
                return 0 - Conversion;
            }
            else if (actual_token.Type == TokenType.Boolean)
            {//Puede ser true o false
                Eat(TokenType.Boolean);

                bool Conversion;
                if (!bool.TryParse(actual_token_value.ToString(), out Conversion)) Error("Se esperaba un tipo bool");
                return Conversion;
            }
            else if (actual_token.Type == TokenType.Quotes_Text)
            {
                Eat(TokenType.Quotes_Text);
                object result = actual_token_value;
                return result;
            }
            else if (actual_token.Value.ToString() == "if")
            {
                Token if_token = actual_token;
                Eat(TokenType.Keyword);
                Eat(TokenType.LEFT_PARENTHESIS);

                bool decision;
                if(!bool.TryParse(Expression().ToString(), out decision)) Error("Se esperaba un tipo bool");
                Eat(TokenType.RIGHT_PARENTHESIS);
                if (decision is true)
                {
                    
                    object result = Expression();
                    position = if_token.final_else - 1;
                    GetNextToken();
                    return result;

                }
                else
                {
                    position = if_token.inicio_else - 1;
                    //Esta es la posicion del else correspondiente a este if
                    GetNextToken();

                    object result2=Expression();
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
                Variables_Set.RemoveAt(variable_subset);
                variable_subset--;
                return result;

            }
            else if (actual_token.Type == TokenType.Identifier)
            {
                Eat(TokenType.Identifier);
                //Comprobar si es una funcion
                if (IsNext(TokenType.LEFT_PARENTHESIS))
                {
                    //Es porque es una funcion
                    string function_name = actual_token_value.ToString();
                    object result = Choosing_Function(function_name);

                    return result;
                }
                //si no es una funcion entonces es una variable
                (object, bool) Existence = Check_Var_Existence();

                return Existence.Item1;

            }
            else if (actual_token.Type == TokenType.Not_Operator)
            {
                Eat(TokenType.Not_Operator);
                
                    bool result;
                    if(!bool.TryParse(Expression().ToString(), out result)) Error("Se esperaba un tipo bool");
                return !result;
            }
            else if (actual_token.Type == TokenType.Keyword)
            {
                Eat(TokenType.Keyword);
                string function_name = actual_token_value.ToString();
                object result = Choosing_Function(function_name);

                return result;
            }
            else
            {
                Eat(TokenType.LEFT_PARENTHESIS);
                object result = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);
                return result;
            }

        }
    }
}