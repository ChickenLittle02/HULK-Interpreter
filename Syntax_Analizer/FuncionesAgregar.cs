
public class Function
{
    public List<string> Variables { get; private set; }
    public List<Token> Tokens_Body { get; private set; }
    public TokenType ReturnType { get; private set; }
    public Function(List<string> variables, List<Token> tokens_body, TokenType returnType)
    {
        Variables = variables;
        Tokens_Body = tokens_body;
        ReturnType = returnType;
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

namespace Syntax_Analizer
{
    partial class Syntax
    {

        public Dictionary<string, Function> Get_New_Functions()
        {
            Dictionary<string, Function> Copy = New_Functions;
            return New_Functions;
        }
        private void Add_Function()
        {//las estructuras de las funciones son
         //function nombre_funcion(variables) => cuerpo_funcion;

            Eat(TokenType.Function_Keyword,"");
            Eat(TokenType.Identifier,"Se esperaba un identificador de la funcion");
            string name = actual_token_value.ToString(); //nombre de la funcion
            bool Ya_Agregada = New_Functions.ContainsKey(name); //Esto significa que es una funcion agregada por el usuario
            if (System_Function.Contains(name)) Error("Ya existe una función del sistema con ese nombre");
            //Si esta condicion se cumple es porque es una funcion del sistema

            Dictionary<string, TokenType> VariablesParaComprobar = new Dictionary<string, TokenType>();
            //Inicializando variable de la funcion para realizar su analisis sintactico

            Function New;
            //Inicializando funcion que va a ser devuelta en caso de ser agregada

            if (Ya_Agregada)
            {
                System.Console.WriteLine("Ya existe una función con este mismo nombre, si desea sobreescribirla toque Enter");
                System.Console.WriteLine("En caso de no querer, toque cualquier letra");
                string decision = Console.ReadLine();

                if (decision == "")
                {//Si entra aqui es porque tocó Enter, entonces crea una funcion y la sobreescribe

                    MakeFunction();
                    New_Functions[name] = New;
                    Syntax FunctionCuerpo = new Syntax(New.Tokens_Body, VariablesParaComprobar, New_Functions);
                    //Analiza sintácticamente el cuerpo de la funcion
                }
                else Error("No fue agregada su funcion");
            }
            else
            {
                MakeFunction();
                New_Functions.Add(name, New);
                Syntax FunctionCuerpo = new Syntax(New.Tokens_Body, VariablesParaComprobar, New_Functions);
            }

            void MakeFunction()
            {//Para construir la funcion necesito saber cuantas variables recibe y guardar los tokens del cuerpo
                Eat(TokenType.LEFT_PARENTHESIS,"Se esperaba un (");
                List<string> Var = Function_Variables();//Guarda los nombres de las variables que recibe en una lista
                Eat(TokenType.RIGHT_PARENTHESIS,"Se esperaba un )");
                Eat(TokenType.Arrow, "Se esperaba un =>");
                List<Token> body = Make_Body();//Guarda los tokens del cuerpo en una lista
                New = new Function(Var, body, TokenType.nul);//Crea la funcion con un valor de retorno nulo, porque no se cual es
            }

            List<string> Function_Variables()
            {   //Va agregando las variables de la funcion
                //Recordar que las variables en una funcion estan en la forma (a,b,c)
                //Por tanto cuando se agregue una variable mientras exista una coma, entonces se agregan variables
                //devuelve una lista con los nombres de las variables de la funcion

                List<string> Variable_ALL = new List<string>();
                Eat(TokenType.Identifier,"Se esperaba un identificador de variable");
                string variable = actual_token_value.ToString();
                Variable_ALL.Add(variable);

                while (actual_token.Type == TokenType.Comma)
                {
                    Eat(TokenType.Comma,"");
                    Eat(TokenType.Identifier,"Se esperaba un identificador de variable");
                    variable = actual_token_value.ToString();
                    Variable_ALL.Add(variable);
                    VariablesParaComprobar.Add(variable, TokenType.nul);

                }

                return Variable_ALL;

            }
        }



        private List<Token> Make_Body()
        {//Para construir el cuerpo de una funcion, lo que hago es guardar todos los tokens del cuerpo
         //Para parsearlo cada vez que llamen a mi funcion

            List<Token> Token_Body = new List<Token>();
            while (actual_token.Type != TokenType.Semicolon && actual_token.Type != TokenType.EOT)
            {
                Token_Body.Add(actual_token);
                GetNextToken();
            }

            if (actual_token.Type != TokenType.Semicolon) Error("EL útlimo token debe ser un punto y coma");
            Token_Body.Add(actual_token);//Se agrega el punto y coma
            Eat(TokenType.Semicolon,"Se esperaba un ;");

            
            if(actual_token.Type != TokenType.EOT) Error("Después del punto y coma no puede haber ninguna otra expresión");

            Token_Body.Add(new Token(TokenType.EOT, "EOT"));
            //Se agrega el EOT para poder parsear correctamente el cuerpo de la funcion más adelante

            GetNextToken();

            return Token_Body;
        }



    }
}