using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;
using Amazon.Util.Internal.PlatformServices;
using System.Collections.Generic;
using System.IO;

namespace ToDoAppWeb.KafkaProducer
{
    public class Producer : BackgroundService
    {
        const string topic = "purchases";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string[] users = { "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther" };
            string[] items = { "book", "alarm clock", "t-shirts", "gift card", "batteries" };

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var settings = builder.GetSection("KafkaSettings").Get<IDictionary<string, string>>();

            using (var producer = new ProducerBuilder<string, string>(
                settings).Build())
            {
                var numProduced = 0;
                Random rnd = new Random();
                const int numMessages = 10;
                for (int i = 0; i < numMessages; ++i)
                {
                    var user = users[rnd.Next(users.Length)];
                    var item = items[rnd.Next(items.Length)];

                    producer.Produce(topic, new Message<string, string> { Key = user, Value = item },
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced event to topic {topic}: key = {user,-10} value = {item}");
                                numProduced += 1;
                            }
                        });
                }

                producer.Flush(TimeSpan.FromSeconds(10));
                Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
                return Task.CompletedTask;
            }
        }
    }
}

