/**********************************************************************
    Project           : IRCBot
    File name         : MessageArcg.cs
    Author            : Rilegis
    Date created      : 15/03/2020
    Purpose           : This file contains a custom class used to store
                        incoming data from IRC.

    Revision History  :
    Date        Author      Ref     Revision 
    22/12/2019  Rilegis     1       Added file header.
    03/01/2020  Rilegis     2       Merged IRCBot-v2 into IRCBot.
    09/03/2020  Rilegis     3       Moved Command Structure here and
                                    changed file name from 'CommandArgs.cs'
                                    to 'CommandStructs.cs'.
    10/03/2020  Rilegis     4       Updated to fit instance-bound bindings.
**********************************************************************/

using Meebey.SmartIrc4net;

namespace IRCBot
{
    public class MessageArgs // Will be renamed to MessageArgs later.
    {
        public MessageArgs(IrcEventArgs ev)
        {
            Channel = ev.Data.Channel;
            From = ev.Data.From;
            Host = ev.Data.Host;
            Ident = ev.Data.Ident;
            Message = ev.Data.Message;
            MessageArray = ev.Data.MessageArray;
            Nick = ev.Data.Nick;
            RawMessage = ev.Data.RawMessage;
            RawMessageArray = ev.Data.RawMessageArray;
        }

        public string Channel { get; set; }
        public string From { get; set; }
        public string Host { get; set; }
        public string Ident { get; set; }
        public string Message { get; set; }
        public string[] MessageArray { get; set; }
        public string Nick { get; set; }
        public string RawMessage { get; set; }
        public string[] RawMessageArray { get; set; }
    }
}