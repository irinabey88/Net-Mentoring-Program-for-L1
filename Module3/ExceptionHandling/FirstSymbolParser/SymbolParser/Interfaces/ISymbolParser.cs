namespace SymbolParser.Interfaces
{
    /// <summary>
    /// Represents an <see cref="ISymbolParser"/> interface.
    /// </summary>
    public interface ISymbolParser
    {
        /// <summary>
        /// Gets symbol from the input string
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>The symbol of the input string</returns>
        char GetSymbol(string inputString);

        /// <summary>
        /// Gets an index to get a symbol from.
        /// </summary>
        int IndexSymbol { get; }
    }
}