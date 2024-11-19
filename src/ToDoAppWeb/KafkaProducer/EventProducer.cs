using Confluent.Kafka;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoAppWeb.KafkaProducer
{
    public class EventProducer : BackgroundService
    {
        const string topic = "todoTasks";
        public ToDoTask Message = null;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var settings = builder.GetSection("KafkaSettings").Get<IDictionary<string, string>>();

            using (var producer = new ProducerBuilder<string, string>(
                settings).Build())
            {
                var message = GenerateMessage();

                if (message is null) return Task.CompletedTask;

                producer.Produce(topic, message,
                    (deliveryReport) =>
                    {
                        if (deliveryReport.Error.Code != ErrorCode.NoError)
                        {
                            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                        }
                        else
                        {
                            Console.WriteLine(message.Value);
                        }
                    });

                producer.Flush(TimeSpan.FromSeconds(10));
                return Task.CompletedTask;
            }
        }

        protected Message<string, string> GenerateMessage()
        {
            var rawMessage = Message;

            if (rawMessage is null) return null;

            var task = new ToDoTask()
            {
                Id = rawMessage.Id,
                Title = rawMessage.Title,
                Description = rawMessage.Description,
                AssignedTo = rawMessage.AssignedTo,
                IsCompleted = rawMessage.IsCompleted,
            };

            var taskAsJson = JsonConvert.SerializeObject(task);
            var message = new Message<string, string> { Key = "", Value = taskAsJson };

            return message;
        }

        public Task Produce(string userId, ToDoTask rawMessage)
        {
            Message = rawMessage;
            Message.Id = userId;
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            return ExecuteAsync(token);
        }
    }
}


