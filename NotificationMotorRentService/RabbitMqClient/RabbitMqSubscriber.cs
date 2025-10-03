#region Mainetence
/*
Comment: Added try catch and fixed saving notification.
Updated: 10/03/2025 13:15
Author:  Gabriel MS
*/
#endregion
using NotificationMotorRentService.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationMotorRentService.RabbitMqClient
{
    public class RabbitMqSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly string _nomeDaFila;
        private readonly IConnection _connection;
        private IModel _channel;
        private IProcessaEvento _processaEvento;

        public RabbitMqSubscriber(IConfiguration configuration, IProcessaEvento processaEvento)
        {
            _configuration = configuration;
            
            var hostName = _configuration["RabbitMQHost"] ?? "localhost";
            var port = int.Parse(_configuration["RabbitMQPort"] ?? "5672");
            
            Console.WriteLine($"Connecting to RabbitMQ: {hostName}:{port}");
            
            _connection = new ConnectionFactory() { HostName = hostName, Port = port }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _nomeDaFila = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _nomeDaFila, exchange: "trigger", routingKey: "");
            _processaEvento = processaEvento;
            
            Console.WriteLine($"RabbitMQ subscriber initialized with queue: {_nomeDaFila}");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer? consumidor = new EventingBasicConsumer(_channel);

            consumidor.Received += (ModuleHandle, ea) =>
            {
                try
                {
                    ReadOnlyMemory<byte> body = ea.Body;
                    string? mensagem = Encoding.UTF8.GetString(body.ToArray());
                    
                    Console.WriteLine($"Message received from RabbitMQ: {mensagem}");
                    
                    if (!string.IsNullOrEmpty(mensagem))
                    {
                        _processaEvento.Process(mensagem);
                    }
                    else
                    {
                        Console.WriteLine("Empty message received");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing RabbitMQ message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumidor);
            
            Console.WriteLine("RabbitMQ consumer started successfully");
            return Task.CompletedTask;
        }
    }
}
