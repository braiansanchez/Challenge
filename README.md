To run this solution.

-Download and install Erlang (needed for rabbitmq server)
https://www.erlang.org/downloads

-Download rabbitmq-server-3.10.5 (current version)
https://www.rabbitmq.com/install-windows.html#installer

-Change the ConnectionStrings in appsettings in the Challenge project, pointing to the local server.

-Run an instance of Challenge project.

-Run an instance of BotStockQuote project.

About this Solution.
Challenge project has the login, register (Net Identity) and 3 ChatRooms to join.
Only the login data is stored in the database.
Users can send a "/stock=" command with a code. Then a BOT in BotStockQuote project will retrieve information from https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv and it will publish a message with the data in a queue. This message is consumed from the queue and shown in the chat in the Challenge project.
The chat is ordered by their timestamps and shows the last 50 messages.
Any error in the BOT answer will show a message informing it.
