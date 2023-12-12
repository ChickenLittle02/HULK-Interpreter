namespace Parser{
    partial class Parser{

        private object Expression()
    {
        object result = Bool_Op();

        while (actual_token.Type == TokenType.And_Operator || actual_token.Type == TokenType.Or_Operator)
        {
            if (actual_token.Type == TokenType.And_Operator)
            {
                Eat(TokenType.And_Operator);
                object result1 = Bool_Op();
                result = Convert.ToBoolean(result) && Convert.ToBoolean(result1);
            }
            else if (actual_token.Type == TokenType.Or_Operator)
            {
                Eat(TokenType.Or_Operator);
                object result1 = Bool_Op();
                result = Convert.ToBoolean(result) || Convert.ToBoolean(result1);
            }
        }
        return result;
    }

    TokenType[] Bool_Oper =
    {
    TokenType.Equal_Operator,// ==
    TokenType.Distinct,// /!=
    TokenType.More_Than, //>
    TokenType.More_Equal_Than, //>=
    TokenType.Min_Than,// <
    TokenType.Min_Equal_Than// <=
    };
    
    int ItsBoolOp(TokenType Type)
    {
        for (int i = 0; i < Bool_Oper.Length; i++)
        {
            if (Type == Bool_Oper[i]) return i;
        }

        return Bool_Oper.Length;

    }

    private object Bool_Op()
    {
        object result = Text();
        int Operador = ItsBoolOp(actual_token.Type);

        while (Operador < Bool_Oper.Length)
        {
            if (Operador == 0)
            {
                Eat(TokenType.Equal_Operator);
                object result2 = Text();

                if (result is string) result = Convert.ToString(result) == Convert.ToString(result2);
                else if (result is double) result = Convert.ToDouble(result) == Convert.ToDouble(result2);
                else result = Convert.ToBoolean(result) == Convert.ToBoolean(result2);

                Operador = ItsBoolOp(actual_token.Type);
            }
            else if (Operador == 1)
            {
                Eat(TokenType.Distinct);
                object result2 = Text();

                if (result is string) result = Convert.ToString(result) != Convert.ToString(result2);
                else if (result is double) result = Convert.ToDouble(result) != Convert.ToDouble(result2);
                else result = Convert.ToBoolean(result) != Convert.ToBoolean(result2);

                Operador = ItsBoolOp(actual_token.Type);
            }
            else if (Operador == 2)
            {
                Eat(TokenType.More_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) > Convert.ToDouble(result2);
                Operador = ItsBoolOp(actual_token.Type);
            }
            else if (Operador == 3)
            {
                Eat(TokenType.More_Equal_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) >= Convert.ToDouble(result2);
                Operador = ItsBoolOp(actual_token.Type);
            }
            else if (Operador == 4)
            {
                Eat(TokenType.Min_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) < Convert.ToDouble(result2);
                Operador = ItsBoolOp(actual_token.Type);
            }
            else 
            //if(Operador == 5)
            {
                Eat(TokenType.Min_Equal_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) <= Convert.ToDouble(result2);
                Operador = ItsBoolOp(actual_token.Type);
            }

        }
        return result;
    }
        
    }
}