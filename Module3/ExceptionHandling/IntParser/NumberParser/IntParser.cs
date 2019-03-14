using System;
using System.Linq;

namespace NumberParser
{
    /// <summary>
    /// Represents a <see cref="IntParser"/>
    /// </summary>
    public static class IntParser
    {
        private const int ZeroAsciiCode = 48;

        /// <summary>
        /// Parses string to the number.
        /// </summary>
        /// <param name="inputString">The input string</param>
        /// <param name="result">Parsed number</param>
        /// <returns><para>true - string is parsed to the number</para>
        /// <para>false - string isn't parsed to the number</para></returns>
        /// <exception cref="ArgumentNullException">Thrown when the input string is null or empty.</exception>
        public static bool TryParse(string inputString, out int result)
        {
            result = default(int);
            var isParsed = ValidateInputStringTryParse(inputString);

            if (isParsed)
            {
                result = Convert(inputString, IsNegativeNumber(inputString));
            }

            return isParsed;
        }

        /// <summary>
        /// Parses string to the number.
        /// </summary>
        /// <param name="inputString">The input string</param>
        /// <returns>Parsed number</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input string is null or empty.</exception>
        /// <exception cref="FormatException">Thrown when the input string contains not only number symbols.</exception>
        public static int Parse(string inputString)
        {
            ValidateInputStringParse(inputString);

            return Convert(inputString, IsNegativeNumber(inputString));
        }

        #region Private methods
        /// <summary>
        /// Checkes is first symbol "-". 
        /// </summary>
        /// <param name="inputString">The input string</param>
        /// <returns><para>true - the 1st symbol is "-"</para>
        /// <para>false - the 1st symbol isn't "-"</para></returns>
        private static bool IsNegativeNumber(string inputString)
        {          
            return inputString.Length > 1 && inputString[0].Equals('-');
        }

        /// <summary>
        /// Checkes are all symbols number.
        /// </summary>
        /// <returns><para>true - all symbols are numbers</para>
        /// <para>false - all symbols aren't numbers</para></returns>
        private static bool IsOnlyNumberSymbol(string inputString)
        {
            return inputString.All(symbol => char.IsNumber(symbol));
        }

        /// <summary>
        /// Cnverts the string to the number.
        /// </summary>
        /// <param name="inputString">The input string</param>
        /// <param name="isNegative"><para>true - the number is negative</para>
        /// <para>false - the number is positive</para></param>
        /// <returns>Parsed number</returns>
        private static int Convert(string inputString, bool isNegative)
        {
            var result = 0;
            var startIndex = IsNegativeNumber(inputString) ? 1 : 0;

            for (int i = startIndex; i < inputString.Length; i++)
            {
                result = result*10 +(inputString[i] - ZeroAsciiCode);
            }

            return isNegative ? result*-1 : result;
        }

        /// <summary>
        /// Validates data for the parsing. 
        /// </summary>
        /// <param name="inputString">The input string</param>
        private static void ValidateInputStringParse(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException($"Input string can't be null or empty!");
            }

            if ((IsNegativeNumber(inputString) && !IsOnlyNumberSymbol(inputString.Substring(1)))
                || (!IsNegativeNumber(inputString) && !IsOnlyNumberSymbol(inputString)))
            {
                throw new FormatException($"The input string has invalid format!");
            }            
        }

        /// <summary>
        /// Validates data for the parsing. 
        /// </summary>
        /// <param name="inputString">The input string</param>
        /// <returns><para>true - the data is valid.</para>
        /// <para>false - the data is invalid </para></returns>
        private static bool ValidateInputStringTryParse(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException($"Input string can't be null or empty!");
            }

            if ((IsNegativeNumber(inputString) && !IsOnlyNumberSymbol(inputString.Substring(1)))
                || (!IsNegativeNumber(inputString) && !IsOnlyNumberSymbol(inputString)))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
