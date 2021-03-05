/**********************************************************************
    Project           : IRCBot
    File name         : Bot.cs
    Author            : Rilegis
    Date created      : 20/09/2019
    Purpose           : This file contains the code needed to start a
                        bot session.

    Revision History  :
    Date        Author      Ref     Revision 
    22/12/2019  Rilegis     1       Added file header.
    03/01/2020  Rilegis     2       Merged IRCBot-v2 into IRCBot.
    09/03/2020  Rilegis     3       Fixed auth strings.
    10/03/2020  Rilegis     4       Added support for instance-bound bindings.
    11/03/2020  Rilegis     5       Moved Authentication and usermode setting
                                    to event EventHandlers.OnRegistered.
**********************************************************************/

using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;

namespace IRCBot
{
    public class Bot
    {
        public void Run(Network network)
        {
            try
            {
                string _dateTime = DateTime.Now.ToString("[dd/MM/yyyy - HH:mm:ss]");
                IrcClient client = new IrcClient
                {
                    // IRC client settings
                    Encoding = System.Text.Encoding.UTF8,
                    SendDelay = 100,
                    ActiveChannelSyncing = true,
                    AutoReconnect = true,
                    AutoRetry = true,
                    AutoRetryDelay = 5,
                    AutoJoinOnInvite = true,
                    CtcpVersion = "C# IRCBot v2"
                };
                List<Command> bindings = new List<Command>(); // List containing instance-bound commands (not shared between bots instances).

                Logger.Log("[+]", "Binding instance-bound core commands...");
                // Private commands
                bindings.Add(new Command { mode = "private", commandName = "status", method = CoreCommands.Status, methodDescription = "Returns status information of the bot and network." });
                bindings.Add(new Command { mode = "private", commandName = "restart", method = CoreCommands.Restart, methodDescription = "Restarts the bot." });
                bindings.Add(new Command { mode = "private", commandName = "shutdown", method = CoreCommands.Shutdown, methodDescription = "Shuts down the bot." });
                bindings.Add(new Command { mode = "private", commandName = "join", method = CoreCommands.Join, methodDescription = "Makes the bot join the specified channel." });
                bindings.Add(new Command { mode = "private", commandName = "part", method = CoreCommands.Part, methodDescription = "Makes the bot part the specified channel." });
                bindings.Add(new Command { mode = "private", commandName = "say", method = CoreCommands.Say, methodDescription = "Echoes the specified message to a specified channel." });
                bindings.Add(new Command { mode = "private", commandName = "act", method = CoreCommands.Act, methodDescription = "Executes an action in the specified channel." });
                bindings.Add(new Command { mode = "private", commandName = "quit", method = CoreCommands.Quit, methodDescription = "Makes the bot quit the network." });
                bindings.Add(new Command { mode = "private", commandName = "help", method = CoreCommands.Help, methodDescription = $"For more information on a specific command, type /msg {network.Username} HELP command." });
                bindings.Add(new Command { mode = "private", commandName = "lookup", method = CoreCommands.IPLookup, methodDescription = $"Performs a whois action on the specified IP address." });
                // Public commands
                bindings.Add(new Command { mode = "public", commandName = $"{network.CommandTrigger}help", method = CoreCommands.Help, methodDescription = $"For more information on a specific command, type /msg {network.Username} HELP command." });
                bindings.Add(new Command { mode = "public", commandName = $"{network.CommandTrigger}ping", method = CoreCommands.Ping, methodDescription = $"Ping command." });
                bindings.Add(new Command { mode = "public", commandName = $"{network.CommandTrigger}lookup", method = CoreCommands.IPLookup, methodDescription = $"Performs a whois action on the specified IP address." });
                Logger.Log("[+]", "Binding instance-bound scripts commands... ");
                // Scripting API W.I.P.

                // Event handlers
                client.OnError += (sender, e) => { EventHandlers.OnError(e); };

                client.OnConnecting += (sender, e) => { EventHandlers.OnConnecting(network.Address, network.Port, network.Username); };
                client.OnConnected += (sender, e) => { EventHandlers.OnConnected(); };
                client.OnRegistered += (sender, e) => { EventHandlers.OnRegistered(network, client); };
                client.OnConnectionError += (sender, e) => { EventHandlers.OnConnectionError(); };

                // client.OnRawMessage += (sender, e) => { EventHandlers.OnRawMessage(e); }; // DEBUG-ONLY
                client.OnChannelMessage += (sender, e) => { EventHandlers.OnChannelMessage(network, client, e, bindings); };
                client.OnQueryMessage += (sender, e) => { EventHandlers.OnQueryMessage(network, client, e, bindings); };
                client.OnCtcpRequest += (sender, e) => { EventHandlers.OnCtcpRequest(client, e); };

                // Connect to network
                try
                {
                    client.Connect(network.Address, network.Port);
                }
                catch (Exception ex)
                {
                    Logger.ExceptionLogger(ex);
                }

                // Login to network
                try
                {
                    client.Login(network.Username, network.RealName, 0, network.Username);
                }
                catch (Exception ex)
                {
                    Logger.ExceptionLogger(ex);
                }

                // Listen incoming data
                client.Listen();

                // Disconnect
                client.Disconnect();
            }
            catch (Exception ex)
            {
                Logger.ExceptionLogger(ex);
            }
        }
    }
}
