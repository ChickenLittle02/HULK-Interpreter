namespace Parser
{
    partial class Parser
    {
        private object Text()
        {

            object result = Form();
            while (actual_token.Type == TokenType.CONCAT_OPERATOR)
            {
                Eat(TokenType.CONCAT_OPERATOR);
                result = Convert.ToString(result) + Convert.ToString(Form());
            }

            return result;
        }

        private object Form()
        {//Aqui se compreueba el caso en que sea una expresion de suma o resta
            object result = Exp();
            while (actual_token.Type == TokenType.SUM_Operator || actual_token.Type == TokenType.SUBSTRACTION_Operator)
            {

                if (actual_token.Type == TokenType.SUM_Operator)
                {
                    double left;
                    if (!double.TryParse(result.ToString(), out left)) Error("Se esperaba un tipo number");

                    Eat(TokenType.SUM_Operator);

                    double right;
                    if (!double.TryParse(Exp().ToString(), out right)) Error("Se esperaba un tipo number");

                    result = left + right;
                }
                else
                {//La opcion que queda es que sea SUBSTRACTION_Operator

                    double left;
                    if (!double.TryParse(result.ToString(), out left)) Error("Se esperaba un tipo number");

                    Eat(TokenType.SUBSTRACTION_Operator);

                    double right;
                    if (!double.TryParse(Exp().ToString(), out right)) Error("Se esperaba un tipo number");

                    result = left - right;
                }
            }
            return result;
        }

        private object Exp()
        {//Aqui se comprueba una posible multiplicacion o division
            object result = Pow();

            while (actual_token.Type == TokenType.MULT_Operator || actual_token.Type == TokenType.DIV_Operator)
            {

                if (actual_token.Type == TokenType.MULT_Operator)
                {

                    double left;
                    if (!double.TryParse(result.ToString(), out left)) Error("Se esperaba un tipo number");

                    Eat(TokenType.MULT_Operator);

                    double right;
                    if (!double.TryParse(Pow().ToString(), out right)) Error("Se esperaba un tipo number");

                    result = left * right;
                }
                else
                {//La opcion que quueda es qye sea DIV_Operator

                    double numerador;
                    if (!double.TryParse(result.ToString(), out numerador)) Error("Se esperaba un tipo number");

                    Eat(TokenType.DIV_Operator);

                    double denominador;
                    if (!double.TryParse(Pow().ToString(), out denominador)) Error("Se esperaba un tipo number");
                    if (denominador == 0) Error("No es posible realizar una division por 0");

                    result = numerador / denominador;
                }
            }
            return result;
        }
        private object Pow()
        {//Aqui se comprueba una posible potencia
            object result = Rest();
            while (actual_token.Type == TokenType.POW_Operator)
            {
                double basis;//base
                if (!double.TryParse(result.ToString(), out basis)) Error("Se esperaba un tipo number");

                Eat(TokenType.POW_Operator);


                double exponente;
                if (!double.TryParse(Pow().ToString(), out exponente)) Error("Se esperaba un tipo number");

                result = Math.Pow(basis, exponente);
            }
            return result;
        }

        private object Rest()
        {//COmprueba que en caso de que el operador sea el de calcular resto, 
            object result = Numb();
            if (actual_token.Type == TokenType.REST_Operator)
            {
                double left;
                if (!double.TryParse(result.ToString(), out left)) Error("Se esperaba un tipo number");

                Eat(TokenType.REST_Operator);

                double right;
                if (!double.TryParse(Exp().ToString(), out right)) Error("Se esperaba un tipo number");


                result = left % right;
            }

            return result;
        }




    }
}