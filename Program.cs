using Parser;
using Syntax_Analizer;
using Lexer_Analizer;

namespace Run
{
    class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, Function> Saving_Functions = new Dictionary<string, Function>();

            while (true)
            {
                System.Console.Write("> ");
                string texto = Console.ReadLine();
                if (texto == "")
                {
                    System.Console.WriteLine("Recuerde que si toca Enter con una entrada vacía es para salir,");
                    System.Console.WriteLine("Si realmente desea salir toque Enter nuevamente, si no desea salir escriba cualquier cosa y toque Enter");
                    string entrada = Console.ReadLine();
                    if (entrada == "") break;
                    else continue;
                }
                try
                {
                    Tokenizer Prueba = new Tokenizer(texto);
                    Syntax CheckSyntax = new Syntax(Prueba.TokenSet, Saving_Functions);
                    TokenType Result = CheckSyntax.Start();
 
                    if (Result != TokenType.nul)
                    {//En caso de que esto ocurra es porque la entrada no fue una declaracion de funcion
                        Parser.Parser Interprete = new Parser.Parser(Prueba.TokenSet, Saving_Functions);
                        object result = Interprete.Start();
                        System.Console.WriteLine(result);
                    }

                    Saving_Functions = CheckSyntax.Get_New_Functions();
 
                }
                catch (Exception text)
                {
                    System.Console.WriteLine(text);
                    continue;
                }


            }

        }

    }
}