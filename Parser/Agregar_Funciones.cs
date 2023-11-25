
namespace Parser
{
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

    partial class Parser
    {

        public Dictionary<string, Function> Get_New_Functions()
        {
            Dictionary<string, Function> Copy = New_Functions;
            return New_Functions;
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

        private List<Token> Make_Body()
        {//Para construir el cuepro de una funcion, lo que hago es guardar todos los tokens del cuerpo
         //Para parsearlo cada vez que llamen a mi funcion

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
    }

}