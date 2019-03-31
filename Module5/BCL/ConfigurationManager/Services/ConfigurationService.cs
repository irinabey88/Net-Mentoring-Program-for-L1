using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ConfigurationManager.Interfaces;
using ConfigurationManager.Models;
using Microsoft.Extensions.Configuration;

namespace ConfigurationManager.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRoot _configuration;

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

        public CultureConfiguration GetCulture()
        {
            return _configuration.GetSection("Configuration:Culture").Get<CultureConfiguration>();
        }

        public DefaultConfiguration GetDefaultConfiguration()
        {
            return _configuration.GetSection("Configuration:DefaultConfiguration").Get<DefaultConfiguration>();
        }

        public IEnumerable<ListenerConfiguration> GetDirectoriesToListen()
        {
            return _configuration.GetSection("Configuration:Directories").Get<List<ListenerConfiguration>>();
        }

        public IEnumerable<RuleConfiguration> GetFileRules()
        {
            return _configuration.GetSection("Configuration:FileRules").Get<List<RuleConfiguration>>();
        }
    }
}