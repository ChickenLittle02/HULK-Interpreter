namespace Syntax_Analizer
{
    partial class Syntax
    {
        private TokenType LowExpression()
        {//Estas son las expresiones más bajas en la jerarquía

            if (actual_token.Type == TokenType.Number)
            {
                Eat(TokenType.Number,"");
                return TokenType.Number;
            }
            else if (actual_token.Type == TokenType.SUM_Operator)
            {//SOn las expresiones del tipo +Numero
                Eat(TokenType.SUM_Operator,"");
                TokenType result = Expression();
                if (result != TokenType.Number && result != TokenType.nul) Error("Despues del operador + se espera un tipo Number");
                Eat(result,"");
                return TokenType.Number;
            }
            else if (actual_token.Type == TokenType.REST_Operator)
            {//Son las expresiones del tipo -Numero
                Eat(TokenType.REST_Operator,"");
                TokenType result = Expression();
                if (result != TokenType.Number && result != TokenType.nul) Error("Despues del operador - se espera un tipo Number");
                Eat(result,"");
                return TokenType.Number;
            }
            else if (actual_token.Type == TokenType.Boolean)
            {//Son las expresiones del tipo true, false
                Eat(TokenType.Boolean,"");
                return TokenType.Boolean;
            }
            else if (actual_token.Type == TokenType.Quotes_Text)
            {//SOn las expresiones del tipo "un texto"
                Eat(TokenType.Quotes_Text,"");
                return TokenType.Quotes_Text;
            }
            else if (actual_token.Value.ToString() == "if")
            {//Son las expresiones if-else
             //Que tienen la estructura if(expresion) expresion else expresion
             //Y de la que se guarda la posicion de la expresion después del else y después de analizada toda la expresion if-else  
                int if_token_pos = position;
                Eat(TokenType.Keyword,"");//if

                Eat(TokenType.LEFT_PARENTHESIS,"Despues de una expresion if se espera un parentesis izquierdo (");//Expresion condicional
                TokenType decision = Expression();
                if (decision != TokenType.Boolean && decision != TokenType.nul) Error("Debe ir una expresion booleana");
                Eat(TokenType.RIGHT_PARENTHESIS,"Se esperaba un parentesis derecho )");

                TokenType result1 = Expression();//Expresion despues de la condicion

                if (!(actual_token.Value.ToString() == "else")) Error("Se esperaba un else");
                Eat(TokenType.Keyword,"");//else

                int else_pos = position;//Posicion a partir de la cual tengo que parsear la expresion del else

                TokenType result2 = Expression();//Expresion despues del else

                int final_if_else = position;
                //Posicion en la que me quedo despues de parsear todo el if-else

                Token_Set[if_token_pos].Set_Inicio_FInal(else_pos, final_if_else);
                //Por defecto siempre se va a pasar el tipo del resultado1
                return result1;

            }
            else if (actual_token.Type == TokenType.Let_Keyword)
            {//Son las expresiones let-in
             //Que tienen la forma let variable1 = expresion, variable2 = expresion, ..., variableN = expresion in expresion
             //Donde cada variable solo existe dentro del let-in por tanto hay que agregarlas al diccionario de variables
             //Y al terminar de porcesarlas eliminarlas del diccionario con variables 
                Eat(TokenType.Let_Keyword,"");
                Dictionary<string, TokenType> Var_Subset = Variables_Subset();
                Variables_Set.Add(Var_Subset);
                variable_subset++;
                Eat(TokenType.In_Keyword,"Se esperaba un in");
                TokenType result = Expression();
                Variables_Set.RemoveAt(variable_subset);
                variable_subset--;
                return result;

            }
            else if (actual_token.Type == TokenType.Not_Operator)
            {//El operador bool de negacion, !
                Eat(TokenType.Not_Operator,"");
                TokenType result = Expression();
                if (!(result == TokenType.Boolean) && result != TokenType.nul) Error("No se puede aplicar el operador ! a un tipo " + result.GetType());
                return result;

            }
            else if (actual_token.Type == TokenType.Identifier)
            {
                Eat(TokenType.Identifier,"");
                //Comprobar si es una funcion
                if (IsNext(TokenType.LEFT_PARENTHESIS))
                {
                    //Es porque es una funcion
                    string function_name = actual_token_value.ToString();
                    bool Existence1 = Check_Function_Existence();
                    if (!Existence1) Error("Esta funcion no existe");

                    TokenType result = Choosing_Function(function_name);

                    return result;
                }
                //si no es una funcion entonces es una variable
                (TokenType, bool) Existence = Check_Var_Existence();
                if (!Existence.Item2) Error("La variable no existe en este entorno");
                return Existence.Item1;

            }
            else if(actual_token.Type == TokenType.Keyword)
            {//Aqui solo
                if (actual_token.Value.ToString() == "else") Error("Todo else debe estar precedido de un if");
                if (actual_token.Value.ToString() == "in") Error("Todo in debe estar precedido de un let");

                Eat(TokenType.Keyword,"");
                string function_name = actual_token_value.ToString();
                    bool Existence1 = Check_Function_Existence();
                    if (!Existence1) Error("Esta funcion no existe");

                    TokenType result = Choosing_Function(function_name);

                    return result;
                

            }
            else
            {
                Eat(TokenType.LEFT_PARENTHESIS,"Se esperaba un (");
                TokenType result = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS,"Se esperaba un )");
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
                Eat(TokenType.Comma,"");
                Var = Variable();
                Var_Set.Add(Var.Item1, Var.Item2);
            }

            return Var_Set;
        }

        private (string, TokenType) Variable()
        {
            Eat(TokenType.Identifier,"Se esperaba un nombre de variable");
            string variable_name = actual_token_value.ToString();
            Eat(TokenType.Asignation_Operator,"Se esperaba un =");

            TokenType value = Expression();
            return (variable_name, value);

        }

        (TokenType, bool) Check_Var_Existence()
        {//Comprueba por cada posicion del diccionario de variables si la variable existe

            for (int i = variable_subset; i >= 0; i--)
            {

                if (Variables_Set[i].ContainsKey(actual_token_value.ToString()))
                {
                    return (Variables_Set[i][actual_token_value.ToString()], true);
                }

            }
            return (TokenType.nul, false);
        }

    }
}