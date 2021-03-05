import ('IRCBot')
import ('System')

function Ping(network, client, args)
	ScriptingAPI.Send(client, "PRIVMSG " .. args.Channel .. " :" .. args.Nick .. " Pong!")
end

ScriptingAPI.BindCommand("public", "^ping", Ping, "Ping command.");