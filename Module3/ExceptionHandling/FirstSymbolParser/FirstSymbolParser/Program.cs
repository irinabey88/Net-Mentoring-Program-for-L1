using System;
using SymbolParser.CustomExceptions;

namespace FirstSymbolParser
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                SymbolParser.SymbolParser symbolParser;
                try
                {
                    symbolParser = new SymbolParser.SymbolParser(0);
                }
                catch (IndexSymbolOutOfRangeException ex)
                {
                    Exit($"CATCH {nameof(IndexSymbolOutOfRangeException)} {ex.Message}");
                    break;
                }
                catch (Exception ex)
                {          
                    Exit(ex.Message);
                    break;
                }

                Console.WriteLine($"Enter string and press ENTER");
                var inputString = Console.ReadLine();

                try
                {
                    Console.WriteLine(
                        $"Symbol #{symbolParser.IndexSymbol} is: {symbolParser.GetSymbol(inputString)} \r\n");
                }
                catch (InvalidInputStringException ex)
                {
                    Console.WriteLine($"CATCH EXCEPTION {nameof(InvalidInputStringException)}. {ex.Message}");
                }
                catch(InvalidLengthInputStringException ex)
                {
                    Console.WriteLine($"CATCH EXCEPTION {nameof(InvalidLengthInputStringException)}. {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine($"Do you want to exit program press ENTER");
                var exit = Console.ReadKey();
                if (exit.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
        }

        private static void Exit(string err)
        {
            Console.WriteLine(err);
            Console.WriteLine($"Program is terminated!");
            Console.ReadKey();
        }
    }
}
