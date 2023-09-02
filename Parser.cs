using System.Globalization;
class Parser
{
    List<Token> Token_Set { get; set; }
    Token actual_token { get; set; }
    object actual_token_value { get; set; }
    private int position { get; set; }
    int size { get; set; }
    private void Error()
    {
        throw new Exception("Invalid syntax");
    }
    public Parser(List<Token> token_Set)
    {
        Token_Set = token_Set;
        position = 0;
        size = Token_Set.Count();
        if (position != size)
        {
            actual_token = Token_Set[position];
        }
        else
        {
            actual_token = null;
        }
    }

    public void Start()
    {
        object result = Text();
        if(result is string){
            System.Console.WriteLine("Es un texto y es "+result);
        }else if( result is int){
            System.Console.WriteLine("Es un numero y es "+result);
        }
        if (position == size)
        {
            System.Console.WriteLine("Parser exitoso");
        }
        else
        {
            System.Console.WriteLine("Salio mal algo");
        }
    }
    private object Text(){
        
        object result = Form();
        while(actual_token.Type==TokenType.CONCAT_OPERATOR){
            Eat(TokenType.CONCAT_OPERATOR);
            result = Convert.ToString(result)+Convert.ToString(Form());
        }

        return result;
    }

    private object Form()
    {
        object result = Exp();
        System.Console.WriteLine(actual_token.Type);
        while (actual_token.Type == TokenType.SUM_Operator || actual_token.Type == TokenType.REST_Operator)
        {

            if (actual_token.Type == TokenType.SUM_Operator)
            {
                Eat(TokenType.SUM_Operator);
                result = Convert.ToInt32(result) + Convert.ToInt32(Exp());
            }
            if (actual_token.Type == TokenType.REST_Operator)
            {
                Eat(TokenType.REST_Operator);
                result = Convert.ToInt32(result) - Convert.ToInt32(Exp());
            }
        }
        return result;
    }

    private object Exp()
    {
        object result = Pow();

        while (actual_token.Type == TokenType.MULT_Operator || actual_token.Type == TokenType.DIV_Operator)
        {

            if (actual_token.Type == TokenType.MULT_Operator)
            {
                Eat(TokenType.MULT_Operator);
                result = Convert.ToInt32(result) * Convert.ToInt32(Pow());
            }
            if (actual_token.Type == TokenType.DIV_Operator)
            {
                Eat(TokenType.DIV_Operator);
                result = Convert.ToInt32(result) / Convert.ToInt32(Pow());
            }
        }
        return result;
    }
    private object Pow()
    {
        object result = Numb();
        if (result is int)
        {
            System.Console.WriteLine("Es un entero 111111");
        }
        else
        {
            System.Console.WriteLine("No es un entero 111111");
        }
        while (actual_token.Type == TokenType.POW_Operator)
        {
            Eat(TokenType.POW_Operator);
            result = Math.Pow(Convert.ToInt32(result), Convert.ToInt32(Pow()));
            System.Console.WriteLine("Este es el resultado de elevar  " + result);
        }
        return result;
    }
    private object Numb()
    {
        actual_token.Show();
        if (actual_token.Type == TokenType.Number)
        {
            Eat(TokenType.Number);
            object result = Convert.ToInt32(actual_token_value);
            return result;
        }
        else if (actual_token.Type == TokenType.Quotes_Text)
        {
            Eat(TokenType.Quotes_Text);
            object result = actual_token_value;
            return result;
        }
        else
        {
            Console.WriteLine($"Entro con el token ");
            System.Console.WriteLine(actual_token.Type);

            Eat(TokenType.LEFT_PARENTHESIS);
            System.Console.WriteLine("Entro");
            object result = Form();
            Eat(TokenType.RIGHT_PARENTHESIS);
            return result;
        }

    }


    private void Eat(TokenType Type)
    {

        if (Type == actual_token.Type)
        {
            actual_token_value = actual_token.Value;
            GetNextToken();
        }
        else
        {
            Error();
        }
    }

    private void GetNextToken()
    {
        if (position == size - 1)
        {
            position++;
            actual_token = new Token(TokenType.EOT, "EOT");
        }
        else if (position == size)
        {

        }
        else
        {
            position++;
            actual_token = Token_Set[position];
        }

    }
}