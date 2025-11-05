using ClearBank.DeveloperTest.Infrastructure.Interfaces;
using System.Configuration;

namespace ClearBank.DeveloperTest.Infrastructure.Config
{
    public class ConfigSettings : IConfigSettings
    {
        public string GetDataStoreType => ConfigurationManager.AppSettings["DataStoreType"];
    }
}
