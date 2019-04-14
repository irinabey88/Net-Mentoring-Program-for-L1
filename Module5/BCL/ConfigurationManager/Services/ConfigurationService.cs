using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ConfigurationManager.Interfaces;
using ConfigurationManager.Models;
using Microsoft.Extensions.Configuration;

namespace ConfigurationManager.Services
{
    /// <summary>
    /// Represence an object <see cref="ConfigurationService"/>
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _configuration;

        /// <summary>
        /// Initialise an instance of the object <see cref="ConfigurationService"/>
        /// </summary>
        /// <param name="configPath">The path to the configuration file.</param>
        public ConfigurationService(string configPath)
        {
            if (string.IsNullOrWhiteSpace(configPath) || !Directory.Exists(configPath))
            {
                throw new ArgumentException(Resources.Messages.InvalidConfigurationPath);
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json", true, true);

            _configuration = builder.Build();
        }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        /// <returns></returns>
        public CultureConfiguration GetCulture()
        {
            return _configuration.GetSection("Configuration:Culture").Get<CultureConfiguration>();
        }

        /// <summary>
        /// Gets the default configuration.
        /// </summary>
        /// <returns></returns>
        public DefaultConfiguration GetDefaultConfiguration()
        {
            return _configuration.GetSection("Configuration:DefaultConfiguration").Get<DefaultConfiguration>();
        }

        /// <summary>
        /// Gets the list of directories, that should be listened.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ListenerConfiguration> GetDirectoriesToListen()
        {
            return _configuration.GetSection("Configuration:Directories").Get<List<ListenerConfiguration>>();
        }

        /// <summary>
        /// Gets the list of the rules that are applied for transformation file name. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RuleConfiguration> GetFileRules()
        {
            return _configuration.GetSection("Configuration:FileRules").Get<List<RuleConfiguration>>();
        }
    }
}