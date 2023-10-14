namespace Run
{
    class Program
    {
        public static void Main(string[] args)
        {
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
            Prueba.Show_TokenSet();
            Parser Syntax = new Parser(Prueba.TokenSet);
            Syntax.Start();
            }catch
            {
                continue;
            }


            }

        }

    }
}