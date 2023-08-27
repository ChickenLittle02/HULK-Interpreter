namespace Run
{
    class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Escriba su codigo");
            string texto = Console.ReadLine();
            Tokenizer Prueba = new Tokenizer(texto);
            Prueba.Show_TokenSet();
            
        }

    }
}