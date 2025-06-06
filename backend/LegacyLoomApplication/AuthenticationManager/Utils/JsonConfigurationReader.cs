using AuthenticationManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationManager.Utils
{
    public class JsonConfigurationReader
    {
        public static JwtConfigurationModel ReadJwtConfigurationModelFromRoot()
        {
			try
			{
                var filePath = "C:\\Users\\Risha\\Desktop\\LegacyLoom\\backend\\LegacyLoomApplication\\AuthenticationManager\\jwt_secrets.json";
                var jsonString = File.ReadAllText(filePath);
                JwtConfigurationModel? jwtConfigurationModel = JsonSerializer.Deserialize<JwtConfigurationModel>(jsonString);
                if (jwtConfigurationModel == null) throw new Exception("Configuration Deserialize returns null object");
                return jwtConfigurationModel;
            }
			catch (Exception ex)
			{
                Console.WriteLine(ex);
                Environment.Exit(0);
			}
            return null;
        }
    }
}
