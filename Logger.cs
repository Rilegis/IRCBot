/**********************************************************************
    Project           : IRCBot
    File name         : Logger.cs
    Author            : Rilegis
    Date created      : 20/09/2019
    Purpose           : This file contains loggers methods.

    Revision History  :
    Date        Author      Ref     Revision 
    22/12/2019  Rilegis     1       Added file header.
    03/01/2020  Rilegis     2       Merged IRCBot-v2 into IRCBot.
    09/03/2020  Rilegis     3       Updated Loggers.
    10/03/2020  Rilegis     4       Updated EventLogger.
**********************************************************************/

using System;
using System.IO;

namespace IRCBot
{
    public static class Logger
    {
        private static string _dateTime;

        public static void ExceptionLogger(Exception ex)
        {
            if (!Directory.Exists("./logs/exceptions")) Directory.CreateDirectory("./logs/exceptions");

            _dateTime = DateTime.Now.ToString("[dd/MM/yyyy - HH:mm:ss]");
            Console.Error.WriteLine($"{_dateTime}\tERROR! Showing stack trace:\n{ex}\n{_dateTime}\tSaving stack trace...");
            // Dump stack trace on a file
            var fileName = $"EXCEPTION_{DateTime.Now.ToString("ddMMyy_HHmmss")}.log";
            using (StreamWriter file = new StreamWriter($"./logs/exceptions/{fileName}", true))
            {
                file.WriteLine(ex.ToString());
                file.Dispose();
                file.Close();
            }
        }

        public static void EventLogger(string ev)
        {
            if (!Directory.Exists("./logs")) Directory.CreateDirectory("./logs");

            _dateTime = DateTime.Now.ToString("[dd/MM/yyyy - HH:mm:ss]");
            using (StreamWriter file = File.AppendText($"./logs/events.log"))
            {
                file.WriteLine($"{_dateTime} {ev}");
                file.Dispose();
                file.Close();
            }
        }

        public static void SocketErrorLogger(string error)
        {
            if (!Directory.Exists("./logs/socketerrors")) Directory.CreateDirectory("./logs/socketerrors");

            // Dump error on a file
            var fileName = $"SOCKETERROR_{DateTime.Now.ToString("ddMMyy_HHmmss")}.log";
            using (StreamWriter file = new StreamWriter($"./logs/socketerrors/{fileName}", true))
            {
                file.WriteLine(error);
                file.Dispose();
                file.Close();
            }
        }

        public static void Log(string log)
        {
            _dateTime = DateTime.Now.ToString("[dd/MM/yyyy - HH:mm:ss]");
            Console.WriteLine($"{_dateTime}     {log}");
        }

        public static void Log(string mode, string log)
        {
            _dateTime = DateTime.Now.ToString("[dd/MM/yyyy - HH:mm:ss]");
            Console.WriteLine($"{_dateTime} {mode} {log}");
        }
    }
}

