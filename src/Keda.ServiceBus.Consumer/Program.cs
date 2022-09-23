using System.Text;

using Azure.Messaging.ServiceBus;

using Microsoft.Azure.ServiceBus;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .CreateLogger();

logger.Information("Consumindo Topico do Azure Service Bus...");

string connectionString = "Endpoint=sb://festivaltecnologiams.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dK+QoNjL9ip8IUMqmwnq8/YhJHVtPTZa4zoMWwYK/uw=;";
string topic = "demo";
string subscription = "SubDemo";

ServiceBusClient? client = null;
ServiceBusProcessor? processor = null;

try
{
    client = new ServiceBusClient(connectionString);
    processor = client.CreateProcessor(topic, subscription, new ServiceBusProcessorOptions());

    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();

    while (true)
    {
        Thread.Sleep(1000);
    }
}
catch (Exception ex)
{
    logger.Error($"Exceção: {ex.GetType().FullName} | " +
                    $"Mensagem: {ex.Message}");
}
finally
{
    if (client is not null)
    {
        await client.DisposeAsync();
        logger.Information(
            "Conexao com o Azure Service Bus fechada!");
    }
        
}

static async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");

    await args.CompleteMessageAsync(args.Message);
}

// handle any errors when receiving messages
static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}