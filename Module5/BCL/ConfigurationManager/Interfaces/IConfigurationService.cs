using System.Collections.Generic;
using ConfigurationManager.Models;

namespace ConfigurationManager.Interfaces
{
    /// <summary>
    /// Represence an interface <see cref="IConfigurationService"/>
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <returns></returns>
        CultureConfiguration GetCulture();

        /// <summary>
        /// Gets the default configuration.
        /// </summary>
        /// <returns></returns>
        DefaultConfiguration GetDefaultConfiguration();

        /// <summary>
        /// Gets the list of directories, that should be listened.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListenerConfiguration> GetDirectoriesToListen();

        /// <summary>
        /// Gets the list of the rules that are applied for transformation file name. 
        /// </summary>
        /// <returns></returns>
        IEnumerable<RuleConfiguration> GetFileRules();
    }
}