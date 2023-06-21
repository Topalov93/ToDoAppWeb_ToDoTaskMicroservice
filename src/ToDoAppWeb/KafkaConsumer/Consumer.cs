using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

namespace ToDoAppWeb.KafkaConsumer
{
    public class Consumer : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string topic = "Users";

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var settings = builder.GetSection("KafkaSettings").Get<IDictionary<string, string>>();

            using (var consumer = new ConsumerBuilder<string, string>(
                settings).Build())
            {
                consumer.Subscribe(topic);
                try
                {
                    while (true)
                    {
                        var cr = consumer.Consume(cts.Token);
                        Console.WriteLine($"Consumed event from topic {topic} with key {cr.Message.Key,-10} and value {cr.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ctrl-C was pressed.
                }
                finally
                {
                    consumer.Close();
                }
                return Task.CompletedTask;
            }
        }
    }
}
