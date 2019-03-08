using System;

namespace NetStandartLibrary
{
    public class Greeting
    {
        public static string SayHi(string name)
        {
            return $"{DateTime.Now}, Hello, {name}";
        }
    }
}
