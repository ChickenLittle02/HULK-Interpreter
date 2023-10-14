class Funciones
{
    string Name { get; set; }
    List<string> Variables { get; set; }
    List<Token> Tokens_Body { get; set; }
    public Funciones(string name, List<string> variables, List<Token> tokens_body)
    {
        Name = name;
        Variables = variables;
        List<Token> Tokens_Body = tokens_body;
    }
}
class Parsereando
{
    List<Token> Token_Set { get; set; }
    Token actual_token { get; set; }
    object actual_token_value { get; set; }
    private int position { get; set; }
    int size { get; set; }
    List<Dictionary<string, object>> Variables_Set { get; set; }
    List<string> Total_Functions = new List<string> { };
    //FUnction_State me dice el estado de la funcion, si es 0 es xq es del sistema y si es 1 es porque la agregué
    List<(string, int)> Function_State = new List<(string, int)>
    {("sqrt",0),("cos",0),("sin",0),("exp",0),("log",0),("rand",0)};
    Dictionary<string, string> Function_Set;

    int variable_subset { get; set; }
    private void Error()
    {
        throw new Exception("Invalid syntax");
    }
    public Parsereando(List<Token> token_Set)
    {
        Token_Set = token_Set;
        position = 0;
        size = Token_Set.Count();
        Variables_Set = new List<Dictionary<string, object>>();
        variable_subset = -1;
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
        object result = Expression();
        System.Console.WriteLine("El resultado es ");
        System.Console.WriteLine(result);

        if (position == size)
        {
            System.Console.WriteLine("Parser exitoso");
        }
        else
        {
            System.Console.WriteLine("Salio mal algo");
        }
    }
    
    private void Add_Function()
    {//las estricturas de las funciones son
     //function nombre_funcion(variables) => cuerpo_funcion;
        Eat(TokenType.Function_Keyword);
        Eat(TokenType.Identifier);
        string name = actual_token_value.ToString();
        Eat(TokenType.LEFT_PARENTHESIS);
        List<string> Var = Function_Variables();
        Eat(TokenType.RIGHT_PARENTHESIS);
        Eat(TokenType.Arrow);
        List<Token> body = Make_Body();
        Function New = new Function(name, Var, body);
        Function_State.Add((name,1));

    }
    private List<string> Function_Variables()
    {
        List<string> Variable_ALL = new List<string>();
        Eat(TokenType.Identifier);
        string variable = actual_token_value.ToString();
        Variable_ALL.Add(variable);

        while (actual_token.Type == TokenType.Comma)
        {
            Eat(actual_token.Type);
            Eat(TokenType.Identifier);
            variable = actual_token_value.ToString();
            Variable_ALL.Add(variable);
        }

        return Variable_ALL;

    }

    private List<Token> Make_Body()
    {
        List<Token> Token_Body = new List<Token>();
        while (actual_token.Type != TokenType.Semicolon && actual_token.Type != TokenType.EOT)
        {
            Token_Body.Add(actual_token);
            GetNextToken();
        }

        if (actual_token.Type == TokenType.EOT) Error();

        Token_Body.Add(actual_token);

        GetNextToken();

        return Token_Body;
    }
    private object Expression()
    {
        object result = Bool_Op();
        bool Es_Bool = false;
        if (result is bool)
        {
            Es_Bool = true;
        }
        while (actual_token.Type == TokenType.And_Operator || actual_token.Type == TokenType.Or_Operator)
        {
            if (actual_token.Type == TokenType.And_Operator)
            {
                if (Es_Bool)
                {
                    Eat(TokenType.And_Operator);
                    object result1 = Bool_Op();
                    if (result1 is bool)
                    {
                        result = Convert.ToBoolean(result) && Convert.ToBoolean(result1);
                    }
                    else
                    {
                        Error();
                    }
                }
                else
                {
                    Error();
                }
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

    private object Bool_Op()
    {
        object result = Text();
        int Operador = ItsBoolOp(actual_token.Type);
        if (Operador < Bool_Oper.Length)
        {

            System.Console.WriteLine("Dice que es el operador " + Bool_Oper[Operador]);
        }
        else { System.Console.WriteLine("No es ninguno de los operadores booleanos"); }
        while (Operador < Bool_Oper.Length)
        {
            if (Operador == 0)
            {
                Eat(TokenType.Equal_Operator);
                object result2 = Text();
                if (result2.GetType() == result.GetType())
                {
                    if (result is string)
                    {
                        result = Convert.ToString(result) == Convert.ToString(result2);
                    }
                    else if (result is int)
                    {
                        result = Convert.ToInt32(result) == Convert.ToInt32(result2);
                    }
                    else
                    {
                        result = Convert.ToBoolean(result) == Convert.ToBoolean(result2);
                    }
                    Operador = Bool_Oper.Length;
                }
                else
                {
                    Error();
                }
            }

            if (Operador == 1)
            {
                Eat(TokenType.Distinct);
                object result2 = Text();
                if (result2.GetType() == result.GetType())
                {

                    if (result is string)
                    {
                        result = Convert.ToString(result) != Convert.ToString(result2);
                    }
                    else if (result is int)
                    {
                        result = Convert.ToInt32(result) != Convert.ToInt32(result2);

                    }
                    else
                    {

                        result = Convert.ToBoolean(result) != Convert.ToBoolean(result2);

                    }
                    Operador = Bool_Oper.Length;
                }
                else
                {
                    Error();
                }
            }
            if (Operador == 2)
            {
                Eat(TokenType.More_Than);
                object result2 = Text();
                if (result2 is int && result is int)
                {
                    result = Convert.ToInt32(result) > Convert.ToInt32(result2);
                    Operador = Bool_Oper.Length;
                }
                else
                {
                    Error();
                }
            }
            if (Operador == 3)
            {
                Eat(TokenType.More_Equal_Than);
                object result2 = Text();
                if (result2 is int && result is int)
                {
                    result = Convert.ToInt32(result) >= Convert.ToInt32(result2);
                    Operador = Bool_Oper.Length;
                }
                else
                {
                    Error();
                }
            }
            if (Operador == 4)
            {
                System.Console.WriteLine("El tipo de Token es " + actual_token.Type);
                Eat(TokenType.Min_Than);
                System.Console.WriteLine("Llego aqui e este punto");
                object result2 = Text();
                if (result2 is int && result is int)
                {
                    result = Convert.ToInt32(result) < Convert.ToInt32(result2);
                    Operador = Bool_Oper.Length;
                }
                else
                {
                    Error();
                }
            }
            if (Operador == 5)
            {
                Eat(TokenType.Min_Equal_Than);
                object result2 = Text();
                if (result2 is int && result is int)
                {
                    result = Convert.ToInt32(result) <= Convert.ToInt32(result2);
                    Operador = Bool_Oper.Length;
                }
                else
                {
                    Error();
                }
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
    {
        object result = Exp();
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
        while (actual_token.Type == TokenType.POW_Operator)
        {
            Eat(TokenType.POW_Operator);
            result = Math.Pow(Convert.ToInt32(result), Convert.ToInt32(Pow()));
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
        else if (actual_token.Type == TokenType.SUM_Operator)
        {
            Eat(TokenType.SUM_Operator);
            Eat(TokenType.Number);
            return actual_token_value;
        }
        else if (actual_token.Type == TokenType.REST_Operator)
        {
            Eat(TokenType.REST_Operator);
            Eat(TokenType.Number);
            double result = 0 - Convert.ToDouble(actual_token_value);
            return result;
        }
        else if (actual_token.Type == TokenType.Bool_True)
        {
            Eat(TokenType.Bool_True);
            object result = Convert.ToBoolean(actual_token_value);
            return result;
        }
        else if (actual_token.Type == TokenType.Bool_False)
        {
            Eat(TokenType.Bool_False);
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
        { System.Console.WriteLine("Entro a que es un if");

            Eat(TokenType.Keyword);
            object decision = Expression();
            if (!(decision is bool)) Error();
            object result1 = Expression();
            if (!(actual_token.Value.ToString() == "else")) Error();
            Eat(TokenType.Keyword);
            object result2 = Expression();
            
            if(decision is true) return result1;

            return result2;

        }
        else if (actual_token.Type == TokenType.Let_Keyword)
        {
            Eat(TokenType.Let_Keyword);
            Dictionary<string, object> Var_Subset = Variables_Subset();
            Variables_Set.Add(Var_Subset);
            variable_subset++;
            Eat(TokenType.In_Keyword);
            object result = Expression();
            System.Console.WriteLine("No he eliminado las variables");
            Variables_Set.RemoveAt(variable_subset);
            System.Console.WriteLine("Ya eliminé las variables");
            variable_subset--;
            return result;

        }
        else if (actual_token.Type == TokenType.Identifier)
        {
            Eat(TokenType.Identifier);
            //Comprobar si es una variable
            int i = 0;
            foreach (var item in Variables_Set)
            {
                System.Console.WriteLine(i);
                foreach (var variable in item)
                {
                    System.Console.WriteLine(variable.Key + "   " + variable.Value);
                }
                i++;
            }

            System.Console.WriteLine("Quejeto");
            (object, bool) Existence = Check_Var_Existence();
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
            if(actual_token.Value.ToString()=="else") Error();
            if(actual_token.Value.ToString()=="in") Error();
            Eat(TokenType.LEFT_PARENTHESIS);
            object result = Expression();
            Eat(TokenType.RIGHT_PARENTHESIS);
            return result;
        }

    }

    private bool IsTrue(object choise)
    {
        if (choise is true) return true;

        return false;
    }

    private Dictionary<string, object> Variables_Subset()
    {
        Dictionary<string, object> Var_Set = new Dictionary<string, object>();

        (string, object) Var = Variable();
        Var_Set.Add(Var.Item1, Var.Item2);

        while (actual_token.Type == TokenType.Comma)
        {
            Eat(TokenType.Comma);
            Var = Variable();
            Var_Set.Add(Var.Item1, Var.Item2);
        }

        return Var_Set;
    }

    private (string, object) Variable()
    {
        Eat(TokenType.Identifier);
        string variable_name = actual_token_value.ToString();
        Eat(TokenType.Asignation_Operator);
        object value = null;

        if (actual_token.Value == "let") Error();
        //revisar esta linea

        value = Expression();
        return (variable_name, value);

    }

    (object, bool) Check_Var_Existence()
    {

        for (int i = variable_subset; i >= 0; i--)
        {

            if (Variables_Set[i].ContainsKey(actual_token_value.ToString()))
            {
                System.Console.WriteLine(actual_token_value + "  existe en " + i);
                return (Variables_Set[i][actual_token_value.ToString()], true);
            }

        }
        return ("", false);
    }

    void Choosing_Function(string function_name)
    {
        //Metodo para procesar funciones, primero descarta que sea una de las globales y 
        //después va para las funciones temporales
        switch (function_name)
        {
            case "sin":
                //Parsea la expresion coge el valor y pasaselo al metodo

                Eat(TokenType.LEFT_PARENTHESIS);
                string numero1 = Numb().ToString();
                Eat(TokenType.RIGHT_PARENTHESIS);
                float conversion1;
                if (!float.TryParse(numero1, out conversion1)) Error();
                Math.Sin(conversion1);
                break;
            case "cos":
                //Parsea la expresion coge el valor y pasaselo al metodo

                Eat(TokenType.LEFT_PARENTHESIS);
                string numero2 = Numb().ToString();
                Eat(TokenType.RIGHT_PARENTHESIS);
                float conversion2;
                if (!float.TryParse(numero2, out conversion2)) Error();
                Math.Cos(conversion2);
                break;
            case "sqrt":
                //Parsea la expresion coge el valor y pasaselo al metodo

                Eat(TokenType.LEFT_PARENTHESIS);
                string numero3 = Numb().ToString();
                Eat(TokenType.RIGHT_PARENTHESIS);
                float conversion3;
                if (!float.TryParse(numero3, out conversion3)) Error();
                Math.Sqrt(conversion3);
                break;
            case "exp":
                //Parsea la expresion coge el valor y pasaselo al metodo

                Eat(TokenType.LEFT_PARENTHESIS);
                string numero4 = Numb().ToString();
                Eat(TokenType.RIGHT_PARENTHESIS);
                float conversion4;
                if (!float.TryParse(numero4, out conversion4)) Error();
                Math.Pow(Math.E, conversion4);
                break;

            case "log":
                //Parsea la expresion coge el valor y pasaselo al metodo

                Eat(TokenType.LEFT_PARENTHESIS);

                string numero5 = Numb().ToString();
                float basis;
                if (!float.TryParse(numero5, out basis)) Error();
                Eat(TokenType.Comma);
                string numero6 = Numb().ToString();
                float argumento;
                if (!float.TryParse(numero5, out argumento)) Error();
                Eat(TokenType.RIGHT_PARENTHESIS);
                Math.Log(basis, argumento);

                break;
            case "print":
                //Parsea la expresion coge el valor y pasaselo al metodo
                //Console.WriteLine(Parse.Expresion) y devuelve ese valor
                break;

            case "rand":
                break;

            default:
                //Parse_Corpus_Function()
                break;

        }
    }




    private void Eat(TokenType Type)
    {
        System.Console.WriteLine(actual_token.Type + "  " + actual_token.Value + " type debe ser " + Type);
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