using SymbolParser.CustomExceptions;
using SymbolParser.Interfaces;

namespace SymbolParser
{
    /// <summary>
    /// Represents a <see cref="SymbolParser"/>.
    /// </summary>
    public class SymbolParser: ISymbolParser
    {   
        /// <summary>
        /// Creates an instance of <see cref="SymbolParser"/>.
        /// </summary>
        /// <param name="indexSymbol">Index othe parsed symbol.</param>
        /// <exception cref="IndexSymbolOutOfRangeException">Throw when the index of the parsed symbol is negative</exception>
        public SymbolParser(int indexSymbol)
        {
            IndexSymbol = indexSymbol >= 0
                ? indexSymbol
                : throw new IndexSymbolOutOfRangeException($"Invalid index of symbol value: {indexSymbol}");
        }

        /// <summary>
        /// Gets an index to get a symbol from.
        /// </summary>
        public int IndexSymbol { get; }

        /// <summary>
        /// Gets symbol from the input string
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>The symbol of the input string</returns>
        /// <exception cref="InvalidInputStringException">Thrown when the input string is null or empty.</exception>
        /// <exception cref="InvalidLengthInputStringException">Thrown when the length of the input string is less than index of the parsed symbol.</exception>
        public char GetSymbol(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new InvalidInputStringException();
            }

            if (inputString.Length < IndexSymbol)
            {
                throw new InvalidLengthInputStringException(inputString.Length, IndexSymbol);
            }

            return inputString[IndexSymbol];
        }
    }
}
