using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezeroDiscord.Modules
{   
    public interface Main : IDisposable
    {
        public class one : ModuleBase<SocketCommandContext>
        {
            public static Dictionary<int, string> dict = new Dictionary<int, string>();
            
            public static DiscordSocketClient client;
            public static BaseSocketClient socket;
            public static int count = 1;
            
            [Command("join")]
            public async Task Join()
            {
                string user = Context.Message.Author.ToString();
                string id = Context.User.Mention.ToString();

                if (dict.ContainsValue(id))
                    await ReplyAsync("追加済みです！！");
                else
                {
                    dict.Add(count, id);
                    var builder = new EmbedBuilder()
        .WithTitle("submitted^^")
        .WithColor(new Color(0xFC6721))
        .WithAuthor(author =>
        {
            author
                .WithName("lottery bot")
                .WithIconUrl("https://miro.medium.com/max/800/1*eU3ZHh9Ew7QIEvaLRjJ11w.png");
        })
        .AddField("Number", count, true)
        .AddField("Name", user+"("+id+")", true);

                    var embed = builder.Build();
                    await Context.Channel.SendMessageAsync(
                        null,
                        embed: embed)
                        .ConfigureAwait(false);

                    count += 1;
                }
            }

            [Command("Init")]
            public async Task Init()
            {
                dict.Clear();
                count = 1;
                await ReplyAsync("初期化しました～");
            }

            [Command("leave")]
            public async Task Leave()
            {
                string user = Context.User.Mention.ToString();
                if (dict.ContainsValue(user))
                {
                    var pair = dict.FirstOrDefault(c => c.Value == user);
                    var key = pair.Key;

                    dict.Remove(key);
                    await ReplyAsync("削除しました！");

                    count -= 1;
                }
                else await ReplyAsync("見つからなかったか、削除済みです！");
            }

            [Command("lottery")]
            public async Task Lottery()
            {
                await ReplyAsync("抽選を開始します！");
                await Task.Delay(1000);
                await ReplyAsync("今回の参加者数は"+dict.Count+"人！");
                await Task.Delay(1000);
                int per = 100 / dict.Count;
                await ReplyAsync("当選確率は約"+per+"%！");
                await Task.Delay(1000);

                Random rand = new Random();

                int rank = rand.Next(Math.Max(1, dict.Count-1))+1;
                await ReplyAsync("当選者は....");
                await Task.Delay(500);
                try
                {
                    await ReplyAsync(dict[rank].ToString() + "さんです！！\nおめでとうございます！");
                }
                catch
                {
                    await ReplyAsync("ごめんなさい！！内部でバグりました！！");
                }
                dict.Clear();
                count = 1;
            }
        }
    }
}
