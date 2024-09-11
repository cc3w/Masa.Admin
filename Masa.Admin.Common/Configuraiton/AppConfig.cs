using Masa.Contrib.Configuration;

namespace Masa.Admin.Common.Configuraiton
{
    public class AppConfig : LocalMasaConfigurationOptions
    {
        public List<string> AllowCors { get; set; }

        public JWTConfig JWTConfig { get; set; }
    }

    public class JWTConfig
    {
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public string Audience { get; set; }
    }

}
