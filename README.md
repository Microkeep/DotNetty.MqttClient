# DotNetty.MqttClient
A lightweight and efficient MQTT client based on [DotNetty](https://github.com/Azure/DotNetty) (from [Azure](https://github.com/Azure) team) that only supports [MQTT-v5.0](https://docs.oasis-open.org/mqtt/mqtt/v5.0/mqtt-v5.0.html).

[![NuGet Badge](https://buildstats.info/nuget/DotNetty.Codec.Mqtt)](https://www.nuget.org/packages/DotNetty.Codec.Mqtt)
[![NuGet Badge](https://buildstats.info/nuget/DotNetty.MqttClient)](https://www.nuget.org/packages/DotNetty.MqttClient)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)

## Feature(implemented)
-----------
* Digital certificate (Two-way or One-way)
* Extended Authentication
* Auto reconnect
* Support ILogger
* User Properties
* Payload Format Indicator & Content Type
* Shared Subscriptions
* Reason Codes & Reason Strings
* Session management: Session Expiry & Message Expiry
* Repeated topics when publishing
* Publication Expiry interval
* Publish Reason Codes
* Disconnect notification

## Support platform
-----------
* Windows
* Linux
* MacOS

## Examples
-----------
```csharp
//init
var options = new MqttClientOptionsBuilder()
        .WithConnectionUri("mqtt://localhost")
        .WithCredentials("user", "password")
        .WithCleanStart()
        .WithUserProperty("name", "value 1")
        .WithUserProperty("name", "value 2")
        .WithWillTopic("will")
        .WithWillPayload("will message")
        .WithWillQos(MqttQos.ExactlyOnce)
        .WithWillUserProperty("name", "value 3")
        .Build();

//var loggerFactory = LoggerFactory.Create(x => x.AddConsole());
//var logger = loggerFactory.CreateLogger<MqttClient>();
//var client = new MqttClient(options, logger);

var services = new ServiceCollection();
services.AddMqttClient(options).AddLogging(x => x.AddConsole());
var serviceProvider = services.BuildServiceProvider();
var client = serviceProvider.GetService<MqttClient>();

var i = 0;

//connected
client.OnConnectedAsync = async () =>
{
    Console.WriteLine("### CONNECTED WITH SERVER ###");
    Console.WriteLine($"### CLIENT ID: {client.Options.ClientId} ###");

    var topicFilters = new TopicFiltersBuilder()
        .WithTopicFilter("topic/1", MqttQos.ExactlyOnce)
        .WithTopicFilter("topic/2", MqttQos.ExactlyOnce)
        .Build();
    var subscribeResult = await client.SubscribeAsync(topicFilters);
    foreach (var item in subscribeResult)
        Console.WriteLine($"+ ReasonCode = {item.ReasonCode}, Topic = {item.TopicName}");

    while (client.IsConnected)
    {
        await Task.Delay(1000);
        Console.WriteLine("### PUBLISH MESSAGE ###");
        i++;

        await client.PublishAsync(x => x
            .WithTopic("topic/1")
            .WithPayload($"Hello MQTT-v5.0, {i}")
            .WithUserProperty("name1", "value 1")
            .WithUserProperty("name2", "value 2")
            .WithUserProperties(new[] { new UserProperty("name3", "value 3"), new UserProperty("name4", "value 4") })
            .WithQos(MqttQos.ExactlyOnce));
    }

    await Task.CompletedTask;
};

//connection lost
client.OnConnectionLostAsync = async ex =>
  {
      Console.WriteLine("### CONNECTION LOST ###");
      Console.WriteLine($"### Exception: {ex.Message} ###");
      await Task.CompletedTask;
  };

//receive message
client.OnApplicationMessageReceivedAsync = async x =>
{
    Console.WriteLine($"### RECEIVED APPLICATION MESSAGE ###");
    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
    Console.WriteLine($"+ Topic   = {x.TopicName}");
    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(x.Payload)}");
    Console.WriteLine($"+ QoS     = {x.Qos}");
    Console.WriteLine($"+ Retain  = {x.Retain}");
    Console.WriteLine($"+ UserProperties = {string.Join(";  ", x.UserProperties.Select(x => $"{x.Name}: {x.Value}").ToArray())}");
    Console.WriteLine();

    await Task.CompletedTask;
};

//disconnected
client.OnDisconnectedAsync = async ex =>
{
    Console.WriteLine("### DISCONNECTED FROM SERVER ###");
    throw ex;
};


//connect to server
var result = await client.ConnectAsync();
Console.WriteLine($"### CONNECT RESULT ###");
Console.WriteLine(result.ReasonCode);

//await Task.Delay(10000);
//await client.DisconnectAsync();

Console.ReadKey();
```

## References
-----------
[OASIS](https://docs.oasis-open.org/mqtt/mqtt/v5.0/mqtt-v5.0.html)

[Azure DotNetty Mqtt Codec](https://github.com/Azure/DotNetty/tree/dev/src/DotNetty.Codecs.Mqtt)

[MQTTnet](https://github.com/dotnet/MQTTnet)

[MqttFx](https://github.com/linfx/MqttFx)

Special thanks are hereby given to the above projects.