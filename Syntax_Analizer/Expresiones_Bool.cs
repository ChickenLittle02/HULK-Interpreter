namespace Syntax_Analizer
{
    partial class Syntax
    {

        private TokenType Expression()
        {//Para que procese en caso de ser las expresiones booleanas unidas con & y |
            TokenType result = Bool_Op();
            bool Es_Bool = false;
            if (result == TokenType.Boolean || result == TokenType.nul)
            {//El nul es para el caso que sea una variable y todavia no se ha definido su tipo, entonces se asume que es el tipo correcto
                Es_Bool = true;
            }
            while (actual_token.Type == TokenType.And_Operator || actual_token.Type == TokenType.Or_Operator)
            {
                if (Es_Bool)
                {
                    if (actual_token.Type == TokenType.And_Operator)
                    {
                        Eat(TokenType.And_Operator,"");
                        TokenType result1 = Bool_Op();
                        if (result1 != TokenType.Boolean && result1 != TokenType.nul) Error("El operador & tiene que tener a continuacion un tipo bool");

                    }
                    else if (actual_token.Type == TokenType.Or_Operator)
                    {
                        Eat(TokenType.Or_Operator,"");
                        TokenType result1 = Bool_Op();
                        if (result1 != TokenType.Boolean && result1 != TokenType.nul) Error("El operador | tiene que tener a continuacion un tipo bool");
                    }
                }
                else
                {
                    Error("El operador " + actual_token.Value + " tiene que estar precedido por un tipo bool");
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

        private TokenType Bool_Op()
        {
            TokenType result = Text();
            int Operador = ItsBoolOp(actual_token.Type);
            //ItsBoolOp comprueba que tipo de operador es en el que estoy parado, para saber si es uno bool o no
            //Comprueba que sea alguno de los operadores booleanos
            /*
                0   ==
                1  /!=
                2  >
                3  >=
                4  <
                5  <=*/

            while (Operador < Bool_Oper.Length)
            {
                if (Operador == 0)
                {//operador ==

                    Eat(TokenType.Equal_Operator,"");
                    TokenType result2 = Text();
                    if (result != result2 && result != TokenType.nul) Error("El operador == tiene que tener el mismo tipo en ambos miembros");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);

                }

                if (Operador == 1)
                {//operador !=
                    Eat(TokenType.Distinct,"");
                    TokenType result2 = Text();
                    if (result != result2 && result != TokenType.nul) Error("El operador != tiene que tener el mismo tipo en ambos miembros");

                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);

                }
                if (Operador == 2)
                {//operador >
                    if (result != TokenType.Number && result != TokenType.nul) Error("El operador > tiene que estar precedido por un tipo number");
                    Eat(TokenType.More_Than,"");
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number && result != TokenType.nul) Error("El operador > tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);
                }
                if (Operador == 3)
                {//Operador >=
                    if (result != TokenType.Number && result != TokenType.nul) Error("El operador >= tiene que estar precedido por un tipo number");
                    Eat(TokenType.More_Equal_Than,"");
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number && result != TokenType.nul) Error("El operador >= tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);

                }
                if (Operador == 4)
                {//Operador <
                    if (result != TokenType.Number && result != TokenType.nul) Error("El operador < tiene que estar precedido por un tipo number");
                    Eat(TokenType.Min_Than,"");
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number && result != TokenType.nul) Error("El operador < tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);
                }
                if (Operador == 5)
                {//Operador <=
                    if (result != TokenType.Number && result != TokenType.nul) Error("El operador <= tiene que estar precedido por un tipo number");
                    Eat(TokenType.Min_Equal_Than,"");
                    TokenType result2 = Text();
                    if (result2 != TokenType.Number && result != TokenType.nul) Error("El operador <= tiene que tener despues un tipo number");
                    result = TokenType.Boolean;
                    Operador = ItsBoolOp(actual_token.Type);
                }

            }
            return result;
        }

        int ItsBoolOp(TokenType Type)
        {
            for (int i = 0; i < Bool_Oper.Length; i++)
            {
                if (Type == Bool_Oper[i]) return i;
            }

            return Bool_Oper.Length;

        }
    }
}