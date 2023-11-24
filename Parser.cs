using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.XPath;

class Function
{//COmo en el analizador sintactico se comprueban que los tipos estan correctos, entonces en el parser solo se ejecuta el codigo
    public List<string> Variables { get; set; }
    public List<Token> Tokens_Body { get; set; }
    public Function(List<string> variables, List<Token> tokens_body)
    {
        Variables = variables;
        Tokens_Body = tokens_body;
    }

    public void Show_Variables()
    {

        System.Console.WriteLine("Variables");
        System.Console.WriteLine(" ");
        foreach (var item in this.Variables)
        {
            System.Console.WriteLine(item);
        }
    }

    public void Show_Body()
    {
        System.Console.WriteLine("Body");
        System.Console.WriteLine(" " + this.Tokens_Body.Count);
        foreach (var item in this.Tokens_Body)
        {
            item.Show();
        }
    }
}
class Parser
{
    List<Token> Token_Set { get; set; }
    Token actual_token { get; set; }
    object actual_token_value { get; set; }
    private int position { get; set; }
    int size { get; set; }
    List<Dictionary<string, object>> Variables_Set { get; set; }
    Dictionary<string, Function> New_Functions { get; set; }
    //Aqui se encuentran todas las funciones agregadas

    public Dictionary<string, Function> Get_New_Functions()
    {
        Dictionary<string, Function> Copy = New_Functions;
        return New_Functions;
    }

    List<string> Function_State = new List<string>
    {"sqrt","cos","sin","exp","log","rand"};
    //FUnction_State me dice quienes son las funciones que tengo


    int variable_subset { get; set; }
    private void Error()
    {
        throw new Exception("Invalid syntax");
    }
    private void Error(string message)
    {
        throw new Exception(message);
    }
    public Parser(List<Token> token_Set, Dictionary<string, object> Var_Subset, Dictionary<string, Function> new_functions)
    {
        Token_Set = token_Set;
        position = 0;
        size = Token_Set.Count();
        Variables_Set = new List<Dictionary<string, object>>
        {
            Var_Subset
        };

        variable_subset = 0;

        New_Functions = new_functions;
        Renovando_Funciones();
        if (position != size)
        {
            actual_token = Token_Set[position];
        }
        else
        {
            actual_token = null;
        }
    }
    private void Renovando_Funciones()
    {

        if (New_Functions.Count != 0)
        {
            foreach (var item in New_Functions)
            {
                Function_State.Add(item.Key);
            }
        }
    }

    public Parser(List<Token> token_Set, Dictionary<string, Function> new_functions)
    {
        Token_Set = token_Set;
        position = 0;
        size = Token_Set.Count();
        Variables_Set = new List<Dictionary<string, object>>();
        variable_subset = -1;
        New_Functions = new_functions;
        Renovando_Funciones();
        if (position != size)
        {
            actual_token = Token_Set[position];
        }
        else
        {
            actual_token = null;
        }
    }


    public object Start()
    {
        object result;
        if (actual_token.Type != TokenType.Function_Keyword) result = Expression();
        else
        {
            Add_Function();
            foreach (var item in New_Functions)
            {
                System.Console.WriteLine("La funcion es " + item.Key);
                item.Value.Show_Variables();
                item.Value.Show_Body();
            }
            result = "Funcion agregada con éxito";
        }

        return result;
    }

    private void Add_Function()
    {//las estructuras de las funciones son
     //function nombre_funcion(variables) => cuerpo_funcion;
        Eat(TokenType.Function_Keyword);
        Eat(TokenType.Identifier);
        string name = actual_token_value.ToString();
        bool Ya_Agregada = New_Functions.ContainsKey(name);

        if (Function_State.Contains(name) && (!Ya_Agregada)) Error("Ya existe una función del sistema con ese nombre");
        //Si esta condicion se cumple es porque es una funcion del sistema

        if (Ya_Agregada)
        {
            System.Console.WriteLine("Ya existe una función con este mismo nombre, desea sobreescribirla");
            System.Console.WriteLine("Toque Enter para sobreescribirla, o cualquier letra si no desea sobreescribirla");
            string decision = Console.ReadLine();

            if (decision == "") Error("No fue agregada su funcion");
            // else Elimina el elemento del diccionario 
        }

        Eat(TokenType.LEFT_PARENTHESIS);
        List<string> Var = Function_Variables();
        Eat(TokenType.RIGHT_PARENTHESIS);
        Eat(TokenType.Arrow);
        List<Token> body = Make_Body();
        Function New = new Function(Var, body);
        New_Functions.Add(name, New);
        Function_State.Add(name);
        System.Console.WriteLine("Function_State.Count" + Function_State.Count);
        foreach (var item in Function_State)
        {
            System.Console.WriteLine(item);
        }

    }
    private List<string> Function_Variables()
    {   //Va agregando las variables de la funcion
        //Recordar que las variables en una funcion estan en la forma (a,b,c)
        //Por tanto cuando se agregue una variable mientras exista una coma, entonces se agregan variables
        //devuelve una lista con los nombres de las variables de la funcion

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

        if (actual_token.Type == TokenType.EOT) Error("EL útlimo token debe ser un punto y coma");

        Token_Body.Add(actual_token);

        System.Console.WriteLine("TOken_Body tiene " + Token_Body.Count + "elementos");
        GetNextToken();

        return Token_Body;
    }
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

            if (Operador == 1)
            {
                Eat(TokenType.Distinct);
                object result2 = Text();

                if (result is string) result = Convert.ToString(result) != Convert.ToString(result2);
                else if (result is double) result = Convert.ToDouble(result) != Convert.ToDouble(result2);
                else result = Convert.ToBoolean(result) != Convert.ToBoolean(result2);

                Operador = ItsBoolOp(actual_token.Type);
            }
            if (Operador == 2)
            {
                Eat(TokenType.More_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) > Convert.ToDouble(result2);
                Operador = ItsBoolOp(actual_token.Type);
            }
            if (Operador == 3)
            {
                Eat(TokenType.More_Equal_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) >= Convert.ToDouble(result2);
                Operador = ItsBoolOp(actual_token.Type);
            }
            if (Operador == 4)
            {
                Eat(TokenType.Min_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) < Convert.ToDouble(result2);
                Operador = ItsBoolOp(actual_token.Type);
            }
            if (Operador == 5)
            {
                Eat(TokenType.Min_Equal_Than);
                object result2 = Text();
                result = Convert.ToDouble(result) <= Convert.ToDouble(result2);
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
        object result = Numb();
        while (actual_token.Type == TokenType.POW_Operator)
        {
            Eat(TokenType.POW_Operator);
            result = Math.Pow(Convert.ToDouble(result), Convert.ToDouble(Pow()));
        }
        return result;
    }
    private object Numb()
    {
        actual_token.Show();

        if (actual_token.Type == TokenType.Number)
        {
            Eat(TokenType.Number);
            object result = Convert.ToDouble(actual_token_value);
            return result;
        }
        else if (actual_token.Type == TokenType.SUM_Operator)
        {
            Eat(TokenType.SUM_Operator);
            object result = Expression();
            return result;
        }
        else if (actual_token.Type == TokenType.REST_Operator)
        {
            Eat(TokenType.REST_Operator);
            double result = 0 - Convert.ToDouble(Expression());
            return result;
        }
        else if (actual_token.Type == TokenType.Boolean)
        {//Puede ser true o false
            Eat(TokenType.Boolean);
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
        { //System.Console.WriteLine("Entro a que es un if");
            Token if_token = actual_token;
            Eat(TokenType.Keyword);
            object decision = Expression();
            if (decision is true)
            {
                object result = Expression();
                position = if_token.final_else - 1;
                GetNextToken();
                return result;

            }
            else
            {
                position = if_token.inicio_else-1;
                //Esta es la posicion del else correspondiente a este if
                GetNextToken();
                System.Console.WriteLine("Este token es " + actual_token.Value);
                object result2 = Expression();
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
            //          System.Console.WriteLine("No he eliminado las variables");
            Variables_Set.RemoveAt(variable_subset);
            //      System.Console.WriteLine("Ya eliminé las variables");
            variable_subset--;
            return result;

        }
        else if (actual_token.Type == TokenType.Identifier)
        {
            Eat(TokenType.Identifier);
            //Comprobar si es una funcion
            //      System.Console.WriteLine("Quejeto");
            System.Console.WriteLine("Antes de comprobar lo del parentesis para entrar en que sea o no una funcion");
            actual_token.Show();
            if (IsNext(TokenType.LEFT_PARENTHESIS))
            {
                //Es porque es una funcion
                string function_name = actual_token_value.ToString();
                bool Existence1 = Check_Function_Existence();
                if (!Existence1) Error("Esta funcion no existe");

                object result = Choosing_Function(function_name);

                return result;
            }
            //si no es una funcion entonces es una variable
            (object, bool) Existence = Check_Var_Existence();
            if (!Existence.Item2) Error("La variable no existe en este entorno");
            //     System.Console.WriteLine("la variable es " + Existence.Item1);

            return Existence.Item1;

        }
        else if (actual_token.Type == TokenType.Not_Operator)
        {
            Eat(TokenType.Not_Operator);
            object result = Expression();
            //            if(!(result is bool)) Error("Despues del operador ! se espera un tipo bool");
            return !Convert.ToBoolean(result);
        }
        else
        {
            if (actual_token.Value.ToString() == "else") Error("Antes de un else siempre debe existir un if");
            if (actual_token.Value.ToString() == "in") Error("Antes de un in siempre debe existir un let");
            Eat(TokenType.LEFT_PARENTHESIS);
            object result = Expression();
            Eat(TokenType.RIGHT_PARENTHESIS);
            return result;
        }

    }

    int SearchElse()
    {
        Stack<string> If_Stack = new Stack<string>();

        int if_position = 0;

        for (int i = position; i < Token_Set.Count; i++)
        {
            if (Token_Set[i].Value.ToString() == "if")
            {
                If_Stack.Push("if");
            }
            else if (Token_Set[i].Value.ToString() == "else")
            {
                if (If_Stack.Count == 0) return i;
                else If_Stack.Pop();
            }
        }
        return if_position;
    }

    private bool IsTrue(object choise)
    {
        if (choise is true) return true;

        return false;
    }

    private bool IsNext(TokenType Expected_Type)
    {
        System.Console.WriteLine(Expected_Type);
        return actual_token.Type == Expected_Type;
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

        if (actual_token.Value.ToString() == "let") Error("Si va a declarar una expresión let dentro de una variable debe hacerla entre parentesis");

        value = Expression();
        return (variable_name, value);

    }

    (object, bool) Check_Var_Existence()
    {

        for (int i = variable_subset; i >= 0; i--)
        {
            if (Variables_Set[i].ContainsKey(actual_token_value.ToString()))
            return (Variables_Set[i][actual_token_value.ToString()], true);   
        }
        return ("", false);
    }

    bool Check_Function_Existence()
    {//Va iterando por el diccionario de funciones buscando si el nombre de la funcion existe
        // System.Console.WriteLine("Function_State " + Function_State.Count);
        for (int i = Function_State.Count - 1; i >= 0; i--)
        {
            //System.Console.WriteLine(Function_State[i]);
            if (Function_State[i] == actual_token_value.ToString())
            {
                //      System.Console.WriteLine(actual_token_value + "  existe en " + i);
                return true;
            }

        }
        return false;
    }

    object Choosing_Function(string function_name)
    {
        //Metodo para procesar funciones, primero descarta que sea una de las globales y 
        //después va para las funciones temporales

        switch (function_name)
        {
            case "sin":
                //Parsea la expresion coge el valor y pasaselo al metodo

                Eat(TokenType.LEFT_PARENTHESIS);
                object numero1 = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);
                return Math.Sin((double)numero1);
            case "cos":
                //Parsea la expresion coge el valor y pasaselo al metodo
                Eat(TokenType.LEFT_PARENTHESIS);
                object numero2 = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);
                return Math.Cos((double)numero2);


            case "sqrt":
                //Parsea la expresion coge el valor y pasaselo al metodo

                Eat(TokenType.LEFT_PARENTHESIS);
                object numero3 = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);
                return Math.Sqrt((double)numero3);

            case "exp":
                //Parsea la expresion coge el valor y pasaselo al metodo

                
                Eat(TokenType.LEFT_PARENTHESIS);
                object numero4 = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);
                return Math.Pow(Math.E, (double)numero4);

            case "log":
                //Parsea la expresion coge el valor y pasaselo al metodo

                
                Eat(TokenType.LEFT_PARENTHESIS);
                object basis = Expression();
                Eat(TokenType.Comma);
                object argumento = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);

                return Math.Log((double)basis, (double)argumento);

            case "print":
                //Se toma como la funcion identidad

                Eat(TokenType.LEFT_PARENTHESIS);
                object ToPrint = Expression();
                Eat(TokenType.RIGHT_PARENTHESIS);
                return ToPrint;

            case "rand":

                Eat(TokenType.LEFT_PARENTHESIS);
                Eat(TokenType.RIGHT_PARENTHESIS);

                return 0;

            default:
                Eat(TokenType.LEFT_PARENTHESIS);

                //Que me parsee las variables y por cada una que parsee guarde la cantidad, si son diferentes
                //del lenght de la lista de variables que lance un error porque solo pueden haber n variables

                Dictionary<string, object> Function_Variables = Make_Function_Variables(function_name);
                Eat(TokenType.RIGHT_PARENTHESIS);

                System.Console.WriteLine("Llego a aaqui ");
                System.Console.WriteLine("Variables de la funcion");
                foreach (var item in Function_Variables)
                {
                    System.Console.WriteLine(item.Key + "  " + item.Value);
                }
                //Ahora con las variables procesa el cuerpo de la funcion
                Parser Parse_Function = new Parser(New_Functions[function_name].Tokens_Body, Function_Variables, New_Functions);
                object result = Parse_Function.Start();
                return result;


        }
    }


    private Dictionary<string, object> Make_Function_Variables(string name)
    {//Va construyendo los valores de la variable
        Dictionary<string, object> Function_Variables = new Dictionary<string, object>();
        List<object> Values = new List<object>();
        object var_value = Expression();
        Values.Add(var_value);
        while (actual_token.Type == TokenType.Comma)
        {
            Eat(TokenType.Comma);
            var_value = Expression();
            Values.Add(var_value);
        }
        int total_var = New_Functions[name].Variables.Count;

        if (total_var != Values.Count) Error("La funcion " + name + " tiene que tener " + total_var + " variables en su declaracion");

        for (int i = 0; i < total_var; i++)
        {
            Function_Variables.Add(New_Functions[name].Variables[i], Values[i]);
        }

        return Function_Variables;

    }




    private void Eat(TokenType Type)
    {
        //    System.Console.WriteLine(actual_token.Type + "  " + actual_token.Value + " type debe ser " + Type);
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