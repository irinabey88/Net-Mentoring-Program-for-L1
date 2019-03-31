using System.Collections.Generic;
using ConfigurationManager.Models;

namespace ConfigurationManager.Interfaces
{
    public interface IConfigurationService
    {
        CultureConfiguration GetCulture();

        DefaultConfiguration GetDefaultConfiguration();

        IEnumerable<ListenerConfiguration> GetDirectoriesToListen();

        IEnumerable<RuleConfiguration> GetFileRules();
    }
}