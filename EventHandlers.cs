/**********************************************************************
    Project           : IRCBot
    File name         : EventHandlers.cs
    Author            : Rilegis
    Date created      : 20/09/2019
    Purpose           : This file contains the code for the events that
                        will happen after the chatbot has been started.

    Revision History  :
    Date        Author      Ref     Revision 
    22/12/2019  Rilegis     1       Added file header.
    03/01/2020  Rilegis     2       Merged IRCBot-v2 into IRCBot.
    09/03/2020  Rilegis     3       Minor changes, mostly nonsense.
    10/03/2020  Rilegis     4       Updated to fit instance-bound bindings.
    11/03/2020  Rilegis     5       Added event OnRegistered.
                                    Moved Authentication and usermode setting
                                    from 'Bot.cs' to event OnRegistered.
**********************************************************************/

using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;

namespace IRCBot
{
    public static class EventHandlers
    {

        public static void OnConnecting(string server, int port, string userName)
        {
            try
            {
                Logger.Log("[+]", $"Connecting to server: {server}:{port} as {userName}");
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnConnected()
        {
            try
            {
                Logger.Log("Connected!");
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnConnectionError()
        {
            try
            {
                Logger.Log("[x]", "An error has occurred while trying to connect to the server.");
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnRegistered(Network network, IrcClient client)
        {
            try
            {
                // Authenticate to services
                if (network.AuthService.Contains("AuthServ")) client.RfcPrivmsg(network.AuthService, $"AUTH {network.Username} {network.AuthPassword}");
                else if (network.AuthService.Contains("NickServ")) client.RfcPrivmsg(network.AuthService, $"IDENTIFY {network.AuthPassword}");

                // Set user-modes from config file
                client.WriteLine($"MODE {client.Nickname} {network.UserModes}");

                // Join network's main channel
                client.RfcJoin(network.MainChan);
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnRawMessage(IrcEventArgs e) // DEBUG-ONLY
        {
            try
            {
                Console.WriteLine(e.Data.RawMessage);
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnQueryMessage(Network network, IrcClient client, IrcEventArgs ev, List<Command> bindings)
        {
            try
            {
                // Convert IrcEventArgs into CommandArgs for cleaner utilization
                MessageArgs args = new MessageArgs(ev);

                // Check if there is a private command that matches all requirements
                foreach (Command cmd in bindings)
                {
                    if ((cmd.mode == "private") && (cmd.commandName == ev.Data.MessageArray[0])) cmd.method("private", network, client, args);
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnChannelMessage(Network network, IrcClient client, IrcEventArgs ev, List<Command> bindings)
        {
            try
            {
                // Convert IrcEventArgs into CommandArgs for cleaner utilization
                MessageArgs args = new MessageArgs(ev);

                // Check if there is a public command that matches all requirements
                foreach (Command cmd in bindings)
                {
                    if ((cmd.mode == "public") && (cmd.commandName == ev.Data.MessageArray[0])) cmd.method("public", network, client, args);
                }
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnCtcpRequest(IrcClient client, CtcpEventArgs ev)
        {
            try
            {
                if (ev.CtcpCommand == "PING") client.WriteLine($"NOTICE {ev.Data.Nick} :PONG PONG!");
                else if (ev.CtcpCommand == "FINGER") client.WriteLine($"NOTICE {ev.Data.Nick} :FINGER Whatc'ya doin!?");
                else if (ev.CtcpCommand == "VERSION") client.WriteLine($"NOTICE {ev.Data.Nick} :VERSION {client.CtcpVersion}");
                else if (ev.CtcpCommand == "USERINFO") client.WriteLine($"NOTICE {ev.Data.Nick} :USERINFO {client.CtcpVersion}");
                else if (ev.CtcpCommand == "CLIENTINFO") client.WriteLine($"NOTICE {ev.Data.Nick} :USERINFO {client.CtcpVersion}");
                else if (ev.CtcpCommand == "SOURCE") client.WriteLine($"NOTICE {ev.Data.Nick} :USERINFO {client.CtcpVersion}");
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }

        public static void OnError(ErrorEventArgs ev)
        {
            try
            {
                Logger.Log("[x]", $"Error: {ev.ErrorMessage}");
                Logger.Log("Logging error...");
                Logger.SocketErrorLogger(ev.ErrorMessage);
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }
    }
}
