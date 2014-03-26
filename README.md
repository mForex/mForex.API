# mForex API
The goal of mForex API is to provide flexible, asynchronous programming model for .NET based clients connecting to mForex Trade Server. 

We are currently conducting beta tests, so our API is only available on demand for demo accounts only. If you would like to participate, please contact us on <api@mforex.pl>. 

## Quick start
mForex API is available on NuGet

```
Install-Package mForex.API
```

### Logging in 
Once you have your account ready, you can log in to our server using following code:

```c#
//Firstly, you have to create APIConnection, specifying which server you want to connect to.
var connection = new APIConnection(ServerType.Demo);

//Secondly, you need to create APIClient.
var client = new APIClient(connection);

//Finally, you can connect and log in to the server.
await client.Connect();
var result = await client.Login(login, password);
```

### Requesting for quotes
Once connection has been established, all relevant data, but ticks, have been setup and are ready to be registered for. ```APIClient``` provides events which can be subscribed to. However, tick data has to be registered using ```.RequestTickRegistration()``` with ```RegistrationAction.Register``` parameter. For example, to receive and process every incoming tick of EURUSD one could:

```c#
client.RequestTickRegistration("EURUSD", RegistrationAction.Register).Wait();

client.Ticks += (Tick[] ticks) =>
    {
        foreach (var tick in ticks)
        {
            [...]
        }
    };
```

### Trade requests
```APIClient``` offers an easy way to manage your orders through an ```ITradeProvider``` interface. For example, sample code which closes all opened positions on EURUSD instrument could look like this:

```c#
TradeRecord[] trades = client.RequestOpenTrades().Result.Trades;
var closeTasks = new List<Task<TradeTransResponsePacket>>();

foreach (var trade in trades)
{
    closeTasks.Add(client.Trade.CloseOrder(trade.Order, trade.Volume));
}

await Task.WhenAll(closeTasks);
```
Note, that you can schedule closing all orders without waiting for first response, which could significantly boost performance in your scenario.

## Asynchronous model
The protocol used to communicate with mForex Trade Server is fundamentally asynchronous. It may be necessary to become familiar with some slightly more advanced .NET features such as asynchronous programming with async and await.

## Problems?
If you encounter any bugs or would like to propose any new features or enhancements, please visit the [issue tracker](https://github.com/mForex/mForex.API/issues) and report the issue. 

## Copyright and License
Copyright 2013 Dom Maklerski mBanku S.A.

Licensed under the [MIT License](https://raw.github.com/mForex/mForex.API/master/LICENSE).
