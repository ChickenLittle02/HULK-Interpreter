namespace Syntax_Analizer
{
    partial class Syntax
    {

        private TokenType Text()
        {

            TokenType result = Form();
            while (actual_token.Type == TokenType.CONCAT_OPERATOR)
            {//NO hay que comprobar tipos, pues en la jerarquia definida se pueden concatenar valores de diferentes tipos

                Eat(TokenType.CONCAT_OPERATOR,"");
                TokenType result2 = Form();
            }

            return result;
        }

        private TokenType Form()
        {
            TokenType result = Exp();
            while (actual_token.Type == TokenType.SUM_Operator || actual_token.Type == TokenType.SUBSTRACTION_Operator)
            {//COmprueba que este sumando y restando tipos numericos

                if (actual_token.Type == TokenType.SUM_Operator)
                {
                    if (result!= TokenType.Number && result != TokenType.nul) Error("Antes de un simbolo de + se espera un tipo number");
                    Eat(TokenType.SUM_Operator,"");
                    TokenType result2 = Exp();
                    if (result2!= TokenType.Number && result != TokenType.nul) Error("Despues de un simbolo de + se espera un tipo number");
                }
                if (actual_token.Type == TokenType.SUBSTRACTION_Operator)
                {

                    if (!(result is TokenType.Number) && result != TokenType.nul) Error("Antes de un simbolo de - se espera un tipo number");
                    Eat(TokenType.SUBSTRACTION_Operator,"");
                    TokenType result2 = Exp();
                    if (!(result2 is TokenType.Number) && result != TokenType.nul) Error("Despues de un simbolo de - se espera un tipo number");
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
                    if (!(result is TokenType.Number) && result != TokenType.nul) Error("Antes de un simbolo de * se espera un tipo number");
                    Eat(TokenType.MULT_Operator,"");
                    TokenType result2 = Pow();
                    if (!(result2 is TokenType.Number) && result != TokenType.nul) Error("Despues de un simbolo de * se espera un tipo number");
                }
                if (actual_token.Type == TokenType.DIV_Operator)
                {
                    if (!(result is TokenType.Number) && result != TokenType.nul) Error("Antes de un simbolo de / se espera un tipo number");
                    Eat(TokenType.DIV_Operator,"");
                    TokenType result2 = Pow();
                    if (!(result2 is TokenType.Number) && result != TokenType.nul) Error("Despues de un simbolo de / se espera un tipo number");
                }
            }
            return result;
        }
        private TokenType Pow()
        {//COmprueba que en caso de que el operador sea el de potencia, el token con el que se esta evaluando sea numerico
            TokenType result = Rest();
            if (actual_token.Type == TokenType.POW_Operator)
            {
                if (!(result is TokenType.Number) && result != TokenType.nul) Error("Antes de un simbolo de ^ se espera un tipo number");
                Eat(TokenType.POW_Operator,"");

                TokenType result2 = Pow();
                if (!(result2 is TokenType.Number) && result != TokenType.nul) Error("Despues de un simbolo de ^ se espera un tipo number");
            }
            //SI entra al if y sale sin problemas es porque el tipo de result es Numb, y si no, el tipo es cualquier otro
            return result;
        }
        private TokenType Rest()
        {//COmprueba que en caso de que el operador sea el de calcular resto, el token con el que se esta evaluando sea numerico
            TokenType result = LowExpression();
            if (actual_token.Type == TokenType.REST_Operator)
            {
                if (!(result is TokenType.Number) && result != TokenType.nul) Error("Antes de un simbolo de % se espera un tipo number");
                Eat(TokenType.REST_Operator,"");

                TokenType result2 = LowExpression();
                if (!(result2 is TokenType.Number) && result != TokenType.nul) Error("Despues de un simbolo de ^ se espera un tipo number");
            }
            //SI entra al if y sale sin problemas es porque el tipo de result es Numb, y si no, el tipo es cualquier otro
            return result;
        }

    }
}