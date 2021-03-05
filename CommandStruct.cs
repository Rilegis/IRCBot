/**********************************************************************
    Project           : IRCBot
    File name         : CommandStruct.cs
    Author            : Rilegis
    Date created      : 20/09/2019
    Purpose           : This file contains the definition of the 'Command'
                        structure.

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
using System;

namespace IRCBot
{
    public struct Command
    {
        public string commandName;
        public string mode;
        public Action<string, Network, IrcClient, MessageArgs> method;
        public string methodDescription;
    }
}
