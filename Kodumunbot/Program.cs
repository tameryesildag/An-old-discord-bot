using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using Discord;

namespace SmileBot
{
   public class Program
    {
        private CommandService commands;
        public DiscordSocketClient client;
        private IServiceProvider services;
        string token = "MzQzODk4ODY4MzU1OTU2NzM3.DITJNA.GZ_4w8nIpHvOg6pnBAM0GasNYg0";
        
        public static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();
     
        public async Task Start()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();

            await InstallCommands();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            client.Log += Log;
            await Task.Delay(-1);
        }
            public async Task InstallCommands()
        {
            client.MessageReceived += HandleCommands;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
        public async Task HandleCommands(SocketMessage msgParam)
        {
            

            var msg = msgParam as SocketUserMessage;
            char prefix = '!';
            if (msg == null) return;
            int argPos = 0;
            if (!(msg.HasCharPrefix(prefix, ref argPos) || msg.HasMentionPrefix(client.CurrentUser, ref argPos))) return;

            var context = new CommandContext(client, msg);
            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
        private Task Log (LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }  
 }

