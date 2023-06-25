﻿using Confluent.Kafka;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Models;
using ToDoApp.Services.TaskService;

namespace ToDoAppWeb.KafkaConsumer
{
    public class Consumer : BackgroundService
    {
        const string topic = "Users";
        public ITaskService _toDoTaskService;

        public Consumer(ITaskService taskService) : base()
        {
            _toDoTaskService = taskService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

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

                using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        var message = consumer.Consume(10);

                        if (message is null)
                        {
                            Console.WriteLine("No new messages on the topic");
                            await Task.CompletedTask;
                        }
                        else
                        {
                            ProceedMessage(message.Message.Value);
                            Console.WriteLine($"Consumed event from topic {topic} with key {message.Message.Key} and value {message.Message.Value}");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Ctrl-C was pressed.
                    }
                    continue;
                }
                await Task.CompletedTask;
            }
        }

        public async void ProceedMessage(string message)
        {
            var user = JsonConvert.DeserializeObject<User>(message);

            try
            {
                var tasks = await _toDoTaskService.GetTasksByUserId(user.Id);

                try
                {
                    await _toDoTaskService.UpdateUserInfo(tasks, user);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
