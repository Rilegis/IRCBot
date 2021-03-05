/**********************************************************************
    Project           : IRCBot
    File name         : Program.cs
    Author            : Rilegis
    Date created      : 20/09/2019
    Purpose           : This is the project's entry point file.

    Revision History  :
    Date        Author      Ref     Revision 
    22/12/2019  Rilegis     1       Added file header.
    03/01/2020  Rilegis     2       Merged IRCBot-v2 into IRCBot.
    09/03/2020  Rilegis     3       Added ability to open multiple bot
                                    sessions from the same executable.
                                    Removed AddonAPI, it will be recreated.
                                    Moved folder system check here
                                    (previously on 'Bot.cs'.)
**********************************************************************/

using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace IRCBot
{
    class Program
    {
        public static Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./etc/config.json")); // Configuration file deserialization

        static void Main(string[] args)
        {
            try
            {
                // Display MOTD
                Console.WriteLine(File.ReadAllText("./etc/motd"));

                // Check if folder system is intact
                Logger.Log("Checking folder system...");
                // Make sure 'addons' folder exists
                if (!Directory.Exists("./scripts"))
                {
                    Logger.Log("'scripts' folder not found...creating a new one.");
                    Directory.CreateDirectory("./scripts");
                }
                // Make sure 'etc' folder exists
                if (!Directory.Exists("./etc"))
                {
                    Logger.Log("'etc' folder not found...creating a new one.");
                    Directory.CreateDirectory("./etc");
                    // Add methos to generate config file.
                    // W.I.P
                }
                // Make sure 'logs' folder exists
                if (!Directory.Exists("./logs"))
                {
                    Logger.Log("'logs' folder not found...creating a new one.");
                    Directory.CreateDirectory("./logs");
                }
                Logger.Log("Done.");

                // Check for SSL mode for database connection
                if (config.Database.UsingSSL) Logger.Log("[+]", "SSL mode for database connection detected.");

                // Foreach network in config.Networks, create a new bot session
                foreach (Network net in config.Networks)
                {
                    new Thread(() => new Bot().Run(net)).Start();
                    // This gives time to each thread to start correctly
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }
    }
}
