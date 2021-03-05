/**********************************************************************
    Project           : IRCBot
    File name         : CoreCommands.cs
    Author            : Rilegis
    Date created      : 20/09/2019
    Purpose           : This file contains commands that are admin-restricted.

    Revision History  :
    Date        Author      Ref     Revision 
    22/12/2019  Rilegis     1       Added file header.
    03/01/2020  Rilegis     2       Merged IRCBot-v2 into IRCBot.
    10/03/2020  Rilegis     3       Updated to fit instance-bound bindings.
**********************************************************************/

using Meebey.SmartIrc4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace IRCBot
{
    public static class CoreCommands
    {
        private static string _tmpString;

        // user@host PRIVMSG BotName :status
        public static void Status(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Admins.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if the user that executed the command is an admin
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command.");
                else
                {
                    if (args.MessageArray.Length > 1) client.WriteLine($"NOTICE {args.Nick} :Too many arguments.");
                    else
                    {
                        client.WriteLine($"NOTICE {args.Nick} :Bot status informations:");
                        client.WriteLine($"NOTICE {args.Nick} :Version: {client.CtcpVersion}   Trigger: '{network.CommandTrigger}'   Main channel: {network.MainChan}");
                        client.WriteLine($"NOTICE {args.Nick} :Nickname: {client.Nickname}   Server: {network.Address}:{network.Port}   Ping: {client.Lag.Milliseconds}ms");
                        foreach (string channel in client.JoinedChannels) _tmpString = $"{_tmpString} {channel}";
                        client.WriteLine($"NOTICE {args.Nick} :Channels:{_tmpString}");
                        _tmpString = "";
                        foreach (string owner in network.Owners) _tmpString = $"{_tmpString} {owner}";
                        client.WriteLine($"NOTICE {args.Nick} :Owners:{_tmpString}");
                        _tmpString = "";
                        foreach (string admin in network.Admins) _tmpString = $"{_tmpString} {admin}";
                        client.WriteLine($"NOTICE {args.Nick} :Admins:{_tmpString}");
                        _tmpString = "";
                        foreach (string friend in network.Friends) _tmpString = $"{_tmpString} {friend}";
                        client.WriteLine($"NOTICE {args.Nick} :Friends:{_tmpString}");
                        _tmpString = "";
                        foreach (string script in network.Scripts) _tmpString = $"{_tmpString} {script}";
                        client.WriteLine($"NOTICE {args.Nick} :Loaded scripts:{_tmpString}");
                        _tmpString = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :restart
        // user@host PRIVMSG BotName :restart message
        public static void Restart(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Admins.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if the user that executed the command is an admin
                {
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command. This event will be logged.");
                    Logger.Log("[x] [NO ACCESS]", $"{args.From} tried to perform restart command.");
                    Logger.EventLogger($"[x] [NO ACCESS] {args.From} tried to perform restart command."); // W.I.P
                }
                else
                {
                    if (args.MessageArray.Length < 2)
                    {
                        client.RfcQuit("Restarting...");
                        Logger.Log("[!]", $"Restart command performed by {args.From}.");
                        Logger.EventLogger($"[!] Restart command performed by {args.From}."); // W.I.P
                    }
                    else
                    {
                        string message = args.Message.Substring(8);
                        client.RfcQuit(message);
                        Logger.Log("[!]", $"Restart command performed by {args.From} with reason {message}.");
                        Logger.EventLogger($"[!] Restart command performed by {args.From} with reason {message}."); // W.I.P
                    }

                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.FriendlyName);
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :shutdown
        // user@host PRIVMSG BotName :shutdown message
        public static void Shutdown(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Owners.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if the user that executed the command is an owner
                {
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command. This event will be logged.");
                    Logger.Log("[x] [NO ACCESS]", $"{args.From} tried to perform shutdown command.");
                    Logger.EventLogger($"[x] [NO ACCESS] {args.From} tried to perform shutdown command."); // W.I.P
                }
                else
                {
                    if (args.MessageArray.Length < 2)
                    {
                        client.RfcQuit("Shutting down...");
                        Logger.Log("[!]", $"Shutdown command performed by {args.From}.");
                        Logger.EventLogger($"[!] Shutdown command performed by {args.From}."); // W.I.P
                    }
                    else
                    {
                        string message = args.Message.Substring(8);
                        client.RfcQuit(message);
                        Logger.Log("[!]", $"Shutdown command performed by {args.From} with reason {message}.");
                        Logger.EventLogger($"[!] Shutdown command performed by {args.From} with reason {message}."); // W.I.P
                    }

                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :join #channel
        // user@host PRIVMSG BotName :join #channel key
        public static void Join(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Admins.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if the user that executed the command is an admin
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command.");
                else
                {
                    if (args.MessageArray.Length < 2) client.WriteLine($"NOTICE {args.Nick} :No destination channel specified.");
                    else if ((args.MessageArray.Length < 3) && (args.MessageArray[1][0] != '#')) client.WriteLine($"NOTICE {args.Nick} :{args.MessageArray[1]} is not a channel.");
                    else if ((args.MessageArray.Length < 3) && (args.MessageArray[1][0] == '#')) client.WriteLine($"JOIN {args.MessageArray[1]}");
                    else if ((args.MessageArray.Length < 4) && (args.MessageArray[1][0] == '#')) client.WriteLine($"JOIN {args.MessageArray[1]} {args.MessageArray[2]}");
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :part #channel
        public static void Part(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Admins.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if the user that executed the command is an admin
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command.");
                else
                {
                    if (args.MessageArray.Length < 2) client.WriteLine($"NOTICE {args.Nick} :No parting channel specified.");
                    else if ((args.MessageArray.Length < 3) && (args.MessageArray[1][0] != '#')) client.WriteLine($"NOTICE {args.Nick} :{args.MessageArray[1]} is not a channel.");
                    else
                    {
                        string[] channels = client.GetChannels();

                        if (!channels.Contains(args.MessageArray[1])) client.WriteLine($"NOTICE {args.Nick} :I am not in that channel.");
                        else client.WriteLine($"PART {args.MessageArray[1]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :say #channel text
        public static void Say(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Owners.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if user is in bot's owners list
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command.");
                else
                {
                    string[] channels = client.GetChannels();

                    if (args.MessageArray.Length < 2) client.WriteLine($"NOTICE {args.Nick} :Not enough arguments.");
                    else if ((args.MessageArray.Length < 3) && (args.MessageArray[1][0] != '#')) client.WriteLine($"NOTICE {args.Nick} :No target channel specified.");
                    else if (args.MessageArray.Length < 3) client.WriteLine($"NOTICE {args.Nick} :No message to echo.");
                    else if (!channels.Contains(args.MessageArray[1])) client.WriteLine($"NOTICE {args.Nick} :I am not in that channel.");
                    else if ((channels.Contains(args.MessageArray[1])) && (args.MessageArray.Length > 2))
                        client.WriteLine($"PRIVMSG {args.MessageArray[1]} :{args.Message.Substring(4 + (args.MessageArray[1].Length + 1))}");
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :act #channel text
        public static void Act(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Owners.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if user is in bot's owners list
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command.");
                else
                {
                    string[] channels = client.GetChannels();

                    if (args.MessageArray.Length < 2) client.WriteLine($"NOTICE {args.Nick} :Not enough arguments.");
                    else if ((args.MessageArray.Length < 3) && (args.MessageArray[1][0] != '#')) client.WriteLine($"NOTICE {args.Nick} :No target channel specified.");
                    else if (args.MessageArray.Length < 3) client.WriteLine($"NOTICE {args.Nick} :No action specified.");
                    else if (!channels.Contains(args.MessageArray[1])) client.WriteLine($"NOTICE {args.Nick} :I am not in that channel.");
                    else if ((channels.Contains(args.MessageArray[1])) && (args.MessageArray.Length > 2))
                        client.WriteLine($"PRIVMSG {args.MessageArray[1]} :ACTION {args.Message.Substring(4 + (args.MessageArray[1].Length + 1))}");
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :quit
        public static void Quit(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Owners.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if the user that executed the command is an owner
                {
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command. This event will be logged.");
                    Logger.Log("[x] [NO ACCESS]", $"{args.From} tried to perform quit command.");
                    Logger.EventLogger($"[x] [NO ACCESS] {args.From} tried to perform quit command."); // W.I.P
                }
                else
                {
                    client.RfcQuit("Quit requested by owner...");
                    Logger.Log("[!]", $"Quit command performed by {args.From} on {network.Address}.");
                    Logger.EventLogger($"[!] Quit command performed by {args.From}."); // W.I.P
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :whois ip
        // user@host PRIVMSG #channel :^whois ip
        public static void IPLookup(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                if (!network.Admins.Contains($"{args.Ident.Replace("~", "")}@{args.Host}")) // Check if user is in bot's owners list
                    client.WriteLine($"NOTICE {args.Nick} :You don't have access to this command.");
                else
                {
                    if (args.MessageArray.Length < 2) client.WriteLine($"NOTICE {args.Nick} :Not enough arguments.");
                    else if (args.MessageArray.Length < 3)
                    {
                        string jsonString;
                        string url = $"http://free.ipwhois.io/json/{args.MessageArray[1]}";
                        // Performs IP whois
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using Stream stream = response.GetResponseStream();
                        using StreamReader reader = new StreamReader(stream);
                        jsonString = reader.ReadToEnd();
                        dynamic jsonObj = JsonConvert.DeserializeObject(jsonString); // Deserialize the response

                        // Print results
                        if ((mode.Equals("private")) && (jsonObj.success == true))
                        {
                            client.WriteLine($"PRIVMSG {args.Nick} :IP Address: {args.MessageArray[1]}");
                            client.WriteLine($"PRIVMSG {args.Nick} :Area: {jsonObj.continent}\\{jsonObj.country}");
                            client.WriteLine($"PRIVMSG {args.Nick} :Organization: {jsonObj.org}");
                            client.WriteLine($"PRIVMSG {args.Nick} :ISP: {jsonObj.isp}");
                        }
                        else if ((mode.Equals("public")) && (jsonObj.success == true))
                        {
                            client.WriteLine($"PRIVMSG {args.Channel} :IP Address: {args.MessageArray[1]}");
                            client.WriteLine($"PRIVMSG {args.Channel} :Area: {jsonObj.continent}\\{jsonObj.country}");
                            client.WriteLine($"PRIVMSG {args.Channel} :Organization: {jsonObj.org}");
                            client.WriteLine($"PRIVMSG {args.Channel} :ISP: {jsonObj.isp}");
                        }
                        else client.WriteLine($"NOTICE {args.Nick} :Invalid IP Address.");
                    }
                    else if (args.MessageArray.Length > 2)
                        client.WriteLine($"NOTICE {args.Nick} :Too many arguments.");
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG BotName :help
        // user@host PRIVMSG BotName :help cathegory
        // user@host PRIVMSG #channel :^help
        // user@host PRIVMSG #channel :^help cathegory
        public static void Help(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                client.WriteLine($"NOTICE {args.Nick} :This command is currently W.I.P.");
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        // user@host PRIVMSG #channel :^ping
        public static void Ping(string mode, Network network, IrcClient client, MessageArgs args)
        {
            try
            {
                client.WriteLine($"PRIVMSG {args.Channel} :{args.Nick}: Pong!");
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }
    }
}
