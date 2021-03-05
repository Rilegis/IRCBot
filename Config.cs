/**********************************************************************
    Project           : IRCBot
    File name         : Config.cs
    Author            : Rilegis
    Date created      : 20/09/2019
    Purpose           : This file contains the definition of the config
                        file stored in './etc/config.json'.

    Revision History  :
    Date        Author      Ref     Revision 
    22/12/2019  Rilegis     1       Added file header.
    03/01/2020  Rilegis     2       Merged IRCBot-v2 into IRCBot.
    09/03/2020  Rilegis     3       Changed configuration file structure.
                                    Added 'Scripts' (It will contains every
                                    scripts that will be loaded in that bot's
                                    network instance).
    09/03/2020  Rilegis     4       Fixed array 'Owner' to 'Owners'.
    11/03/2020  Rilegis     5       Changed configuration file structure.
    15/03/2020  Rilegis     6       Changed configuration file structure.
**********************************************************************/

namespace IRCBot
{
    public class Config
    {
        public Network[] Networks { get; set; }
        public Database Database { get; set; }
    }

    public class Network
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string AuthService { get; set; }
        public string AuthPassword { get; set; }
        public string RealName { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string UserModes { get; set; }
        public string MainChan { get; set; }
        public string[] Owners { get; set; }
        public string[] Admins { get; set; }
        public string[] Friends { get; set; }
        public string CommandTrigger { get; set; }
        public string[] Scripts { get; set; }
    }

    public class Database
    {
        public string Host { get; set; }
        public uint Port { get; set; }
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool UsingSSL { get; set; }
        public string SSLCertificate { get; set; }
        public string SSLCertificatePassword { get; set; }
    }
}
