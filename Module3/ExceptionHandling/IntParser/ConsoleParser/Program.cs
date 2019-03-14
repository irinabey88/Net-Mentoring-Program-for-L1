using System;

namespace ConsoleParser
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($"Enter the string for parsing:");
                var str = Console.ReadLine();
                int result;

                try
                {
                    var isConverted = NumberParser.IntParser.TryParse(str, out result);
                    Console.WriteLine($"String is converted = {isConverted}. Received number: {result}");

                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"CATCH {nameof(ArgumentException)} for TryParse: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CATCH EXCEPTION for TryParse: {ex.Message}");
                }

                try
                {
                    result = NumberParser.IntParser.Parse(str);
                    Console.WriteLine($"The converted number: {result}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"CATCH {nameof(ArgumentException)} for Parse: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"CATCH {nameof(FormatException)} for Parse: {ex.Message}");
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"CATCH EXCEPTION for Parse: {ex.Message}");
                }

                Console.WriteLine($"Do you want to exit program press ENTER");
                var exit = Console.ReadKey();

                if (exit.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
        }
    }
}
