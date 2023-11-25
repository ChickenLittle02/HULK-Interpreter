using Parser;
using Syntax_Analizer;
using Lexer_Analizer;

namespace Run
{
    class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, Parser.Function> Saving_Functions = new Dictionary<string, Parser.Function>();
            while(true){
                
            System.Console.Write("> ");
            string texto = Console.ReadLine();
            if(texto==""){
               System.Console.WriteLine("Recuerde que si toca Enter con una entrada vacía es para salir,");
               System.Console.WriteLine("Si realmente desea salir toque Enter nuevamente, si no desea salir escriba cualquier cosa y toque Enter");
                string entrada = Console.ReadLine();
                if(entrada=="") break;
                else continue;
            }
            try{
            Tokenizer Prueba = new Tokenizer(texto);
            Syntax CheckSyntax = new Syntax(Prueba.TokenSet);
            CheckSyntax.Start();
            //Crear un constructor que reciba dos argumentos para pasarle las funciones creadas
            //Prueba.Show_TokenSet();
            Parser.Parser Syntax = new Parser.Parser(Prueba.TokenSet, Saving_Functions);
            object result = Syntax.Start();
            Saving_Functions = Syntax.Get_New_Functions();
            System.Console.WriteLine(result);
            }catch(Exception text)
            {
                System.Console.WriteLine(text);
                continue;
            }


            }

        }

    }
}