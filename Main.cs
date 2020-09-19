using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Config;
using System.Diagnostics;

namespace Rezero
{
    class MainParts
    {
        public static DiscordSocketClient client;
        public static CommandService commands;
        public static IServiceProvider services;
        public static ModuleBase<SocketCommandContext> Context;
        public static string token { get; set; }

        static void Main() => new MainParts().Start().GetAwaiter().GetResult();

        private void Checktoken() // tokenの確認
        {
            var ini = new Settings();
            ini.IniFile(Environment.CurrentDirectory + @"\default.ini");
            token = ini.GetToken();
            if (token == "Unknown")
            {
                Console.WriteLine("tokenの取得に失敗しました。default.iniを確認してください。");
                Environment.Exit(0);
            }
        }

        public async Task Start()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();
            client.MessageReceived += CommandRecieved;
            client.Log += Log;
            Checktoken();

            
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await client.SetGameAsync("Status");
            await Task.Delay(-1);
        }

        private async Task CommandRecieved(SocketMessage recievedmassage)
        {
            var message = recievedmassage as SocketUserMessage;
            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);

            if (message == null || message.Author.IsBot) return;
            var context = new SocketCommandContext(client, message);

            var result = await commands.ExecuteAsync(context, 0, services);

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
            }
        }

        private Task Log(LogMessage msg) // ログをコンソールに残すやつ 処理を変えればファイルにも書き込める
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
    }
}
