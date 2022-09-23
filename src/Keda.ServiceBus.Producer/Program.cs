using System.Text;

using Microsoft.Azure.ServiceBus;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .CreateLogger();

logger.Information("Enviando Topico do Azure Service Bus...");

string connectionString = "Endpoint=sb://festivaltecnologiams.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=dK+QoNjL9ip8IUMqmwnq8/YhJHVtPTZa4zoMWwYK/uw=;";
string topic = "demo";

if (args.Length == 0 || !int.TryParse(args[0], out int quantity))
{
    logger.Information("Argumento de quantidade de mensagens inválido");
    return;
}

TopicClient? client = null;

try
{
    client = new TopicClient(connectionString, topic);

    for (int i = 1; i <= quantity; i++)
    {
        await client.SendAsync(
            new Message(Encoding.UTF8.GetBytes($"Mensagem {i}")));

        logger.Information(
            $"[Mensagem enviada] {i}");
    }

    logger.Information("Concluido o envio de mensagens");
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
        await client.CloseAsync();
        logger.Information(
            "Conexao com o Azure Service Bus fechada!");
    }
        
}
