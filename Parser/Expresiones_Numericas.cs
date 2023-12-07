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
            while (actual_token.Type == TokenType.SUM_Operator || actual_token.Type == TokenType.REST_Operator)
            {

                if (actual_token.Type == TokenType.SUM_Operator)
                {
                    Eat(TokenType.SUM_Operator);
                    result = Convert.ToDouble(result) + Convert.ToDouble(Exp());
                }
                else
                {//La opcion que queda es que sea REST_Operator

                    Eat(TokenType.REST_Operator);
                    result = Convert.ToDouble(result) - Convert.ToDouble(Exp());
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
                    Eat(TokenType.MULT_Operator);
                    result = Convert.ToDouble(result) * Convert.ToDouble(Pow());
                }
                else
                {//La opcion que quueda es qye sea DIV_Operator
                    Eat(TokenType.DIV_Operator);
                    double denominador = Convert.ToDouble(Pow());
                    if (denominador == 0) Error("No es posible realizar una division por 0");
                    result = Convert.ToDouble(result) / denominador;
                }
            }
            return result;
        }
        private object Pow()
        {//Aqui se comprueba una posible potencia
            object result = Rest();
            while (actual_token.Type == TokenType.POW_Operator)
            {
                Eat(TokenType.POW_Operator);
                result = Math.Pow(Convert.ToDouble(result), Convert.ToDouble(Pow()));
            }
            return result;
        }

        private object Rest()
        {//COmprueba que en caso de que el operador sea el de calcular resto, 
            object result = Numb();
            if (actual_token.Type == TokenType.REST_Operator)
            {
                
                Eat(TokenType.REST_Operator);
                result = Convert.ToDouble(result)%Convert.ToDouble(Numb());
            }

            return result;
        }




    }
}