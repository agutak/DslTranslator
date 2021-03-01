using System;

namespace DslTranslator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test ExpressionEngine!");

            var ee = new ExpressionEngine();
            ee.AddFunctions(typeof(Math));

            while (true)
            {
                Console.WriteLine("Write an expression to evaluate:");
                var expression = Console.ReadLine();

                try
                {
                    var result = ee.Evaluate(expression);
                    Console.WriteLine($"Result: {result}");
                }
                catch
                {
                    Console.WriteLine("Error in provided expression!");
                }
            }
        }
    }
}
