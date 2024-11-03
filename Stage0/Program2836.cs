partial class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Welcome2836();
        Welcome9106();
        Console.ReadKey();

        }
        static partial void Welcome9106();
        private static void Welcome2836()
        {
            Console.WriteLine("Enter your name: ");
            string userName = System.Console.ReadLine();
            Console.WriteLine("{0} ,welcome to my first cosole application", userName);
        }
    }
}