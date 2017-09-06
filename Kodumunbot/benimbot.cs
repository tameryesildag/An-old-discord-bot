using System;
using Discord;
using Discord.Audio;
using Discord.WebSocket;
using Discord.Commands;
using Discord.API;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Xml;

namespace SmileBo
{

    public class benimbot : ModuleBase
    {
        //deneme
        static int sıra;
        string[] kills;
        string[] deaths;
        string[] assists;
        string[] result;
        static int l = 0;    //0 = İngilizce, 1= Türkçe
        static int üstlimitsaniye;
        static int saniye;
        public static string[] secenekler;
        public static int[] oylar;
        public static ulong[] idler;
        int idlersira = 0;
        public static string sorust;
        IMessageChannel ch;
        SmileBot.Riot riot = new SmileBot.Riot();
        Ping p;
        [Command("Ping")]
        [Summary("")]
        public async Task Ping([Remainder] string question = null)
        {
            p = new Ping();
            await Context.Channel.SendMessageAsync("**" + p.Send("www.discordapp.com").RoundtripTime.ToString() + "ms" + "**");
        }
        [Command("Ping")]
        [Summary("")]
        public async Task typetest()
        {
            p = new Ping();
            await Context.Channel.SendMessageAsync("**" + p.Send("www.discordapp.com").RoundtripTime.ToString() + "ms" + "**");
        }
        [Command("Dil")]
        [Alias("Language")]
        public async Task Dil(string d)
        {
            if (d == "en")
            {
                l = 0;
                await Context.Channel.SendMessageAsync("Language is set to **English**.");
            }
            if (d == "tr")
            {
                l = 1;
                await Context.Channel.SendMessageAsync("Dil **Türkçeye** ayarlandı.");
            }
            if (d != "tr" & d != "en")
            {
                await Context.Channel.SendMessageAsync("!language <tr / en>");
            }
        }
        [Command("sunucular")]
        [Summary("")]
        public async Task sunucular()
        {
            if (Context.User.Id == 244451433812852736)
            {
                string metin = "";
                foreach (IGuild gui in (Context.Client as DiscordSocketClient).Guilds)
                {
                    metin += "- " + gui.Name + " ";
                }
                await Context.Channel.SendMessageAsync("```" + metin + "```");
            }
            else
            {
                if (l == 1) await Context.Channel.SendMessageAsync("Sunucuları görüntülemeye yetkiniz yok.");
                else await Context.Channel.SendMessageAsync("You do not have permission to view servers.");
            }
        }
        [Command("Kapa")]
        [Summary("")]
        public async Task Kapa()
        {
            if (Context.User.Id == 244451433812852736)
            {
                if (l == 1) await Context.Channel.SendMessageAsync("Bot çevrimdışı.");
                else await Context.Channel.SendMessageAsync("Bot is offline.");
                Environment.Exit(0);
            }
            else
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention + " Yetkiniz yok.");
            }
        }
        [Command("Hava")]
        [Alias("Havadurumu")]
        public async Task Hava(string şehir)
        {
            SmileBot.havadurumu hd = new SmileBot.havadurumu();
            hd.sicaklikbul(şehir);
            if (l == 1) await Context.Channel.SendMessageAsync(hd.mesaj + "**" + hd.derece.ToString() + "** derece." + "\nYağış oranı **" + hd.yagisorani + "**" + "\nNem **" + hd.nem + "**");
            else await Context.Channel.SendMessageAsync("The Weather in " + şehir + " **" + hd.derece.ToString() + "** degree" + "\nPrecipitation rate **" + hd.yagisorani + "**" + "\nHumidty **" + hd.nem + "**");
        }
        [Command("Havaradar")]
        [Alias("radar", "weatherradar")]
        public async Task Havaradar()
        {
            if (l == 1)
            {
                string localFilename = @"D:\Visual studio projects\SmileBot\Kodumunbot\bin\Debug\radar.jpg";
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://www.mgm.gov.tr/FTPDATA/uzal/radar/comp/compppi15.jpg", localFilename);
                }
                await Context.Channel.SendFileAsync("radar.jpg", DateTime.Now.ToString());
            }
            else
            {
                await Context.Channel.SendMessageAsync("This feature is disabled in English Version.");
            }
        }
        [Command("Özürlü")]
        [Alias("Disabled")]
        public async Task Özürlü([Remainder] string metin)
        {
           await Context.Message.DeleteAsync();
            char[] charlar;
            charlar = metin.ToCharArray();
            char[] charlar2 = new char[charlar.Length];
            foreach (char c in charlar)
            {
                Console.WriteLine(c);
            }
            for(int i = 0; i < charlar.Length; i++)
            {
                if(sıra == 0)
                {
                  charlar2[i] = Char.ToUpper(charlar[i]);
                    sıra = 1;
                    continue;
                }
                if (sıra == 1)
                {
                    charlar2[i] = Char.ToLower(charlar[i]);
                    sıra = 0;
                }
            }
            string s = new string(charlar2);
            await Context.Channel.SendFileAsync("sponge.jpg","**" + s + "**");
        }
        [Command("Steamdurum")]
        [Summary("")]
        [Alias("steam", "steamserver", "steamonline", "steamstatus")]
        public async Task Steamserver()
        {
            p = new Ping();
            string temp = "";
            try
            {
                temp = ("**" + p.Send("store.steampowered.com").RoundtripTime.ToString() + "ms" + "**");
                if (l == 1) await Context.Channel.SendMessageAsync("**Steam market sunucuları çevrimiçi** ✅");
                else await Context.Channel.SendMessageAsync("**Steam market servers are online** ✅");
            }
            catch
            {
                if (l == 1) await Context.Channel.SendMessageAsync("**Steam market sunucuları çevrimdışı** ❌");
                else await Context.Channel.SendMessageAsync("**Steam market servers are offline** ❌");
            }
        }
        [Command("Anket")]
        [Alias("Oylama", "Poll")]
        public async Task Anket([Remainder] string soru = "")
        {
            sorust = soru;
            if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Soru ayarlandı. Seçenekleri ayarlayın !secenekler <secenek> <secenek> ...");
            else await Context.Channel.SendMessageAsync(Context.User.Mention + " Question has been set. Set the options with !Options <option> <option> ...");
        }
        [Command("Secenekler")]
        [Alias("Options", "Choices","Seçenekler")]
        public async Task secenek(string secenek1 = "", string secenek2 = "", string secenek3 = "", string secenek4 = "", string secenek5 = "")
        {
            if (sorust == "") {
                if (l == 1) await Context.Channel.SendMessageAsync("İlk önce soruyu ayarlayın. !anket <soru>");
                else await Context.Channel.SendMessageAsync("Set the question first. !poll <question>");
                return;
            }
            if (secenek2 == "")
            {
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Bir anket oluşturabilmeniz için en az 2 seçenek olmalı.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + " There must be at least 2 options to create a poll.");
                return;
            }
            secenekler = new string[5];
            secenekler[0] = secenek1;
            secenekler[1] = secenek2;
            secenekler[2] = secenek3;
            secenekler[3] = secenek4;
            secenekler[4] = secenek5;
            secenekler = secenekler.Except(new string[] { "" }).ToArray();
            int sayi = 1;
            string metin = Context.User.Mention + " Bir oylama oluşturdu. !oy <numara> \n\n" + sorust + "\n";
            if (l == 0) metin = Context.User.Mention + " Created a poll. !vote <number> \n\n" + sorust + "\n";
            string title = "Anket";
            if (l == 0) title = "Poll";
            foreach (var vr in secenekler)
            {
                metin += "\n\n**" + sayi + "- " + vr + "**";
                sayi += 1;
            }
            var eb = new EmbedBuilder() { Title = ":clipboard: " + title, Description = metin, Color = Color.LightOrange };
            await Context.Channel.SendMessageAsync("", false, eb);
            idler = new ulong[100];
        }
        [Command("Oy")]
        [Alias("Oyver", "vote")]
        public async Task Oy(int secenek)
        {
            foreach (var vr in idler)
            {
                if (Context.User.Id == vr)
                {
                    if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Zaten oy verdiniz.");
                    else await Context.Channel.SendMessageAsync(Context.User.Mention + " You already voted.");
                    return;
                }
            }
            oylar = new int[secenekler.Length];
            if (secenek == 1)
            {
                oylar[0] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[0] + "** isimli seçeneğe oy verdiniz.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + "You voted for **" + secenekler[0] + "**");
            }
            if (secenek == 2)
            {
                oylar[1] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[1] + "** isimli seçeneğe oy verdiniz.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + "You voted for **" + secenekler[1] + "**");
            }
            if (secenek == 3)
            {
                oylar[2] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[2] + "** isimli seçeneğe oy verdiniz.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + "You voted for **" + secenekler[2] + "**");
            }
            if (secenek == 4)
            {
                oylar[3] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[3] + "** isimli seçeneğe oy verdiniz.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + "You voted for **" + secenekler[3] + "**");
            }
            if (secenek == 5)
            {
                oylar[4] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[4] + "** isimli seçeneğe oy verdiniz.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + "You voted for **" + secenekler[0] + "**");
            }
        }
        [Command("Anketbitir")]
        [Alias("Anketbit", "Anketkapa", "pollfinish", "pollresults")]
        public async Task Anketbitir()
        {
            if (secenekler[1] == null)
            {
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Bitirilecek bir anket yok.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + " There is no poll to finish.");
                return;
            }
            string metin = "Oylama bitti. Sonuçlar: \n\n";
            if (l == 0) metin = "Poll has finished. Results: \n\n";
            // await Context.Channel.SendMessageAsync(secenekler[sıra]);
            for (int i = 0; i < secenekler.Length; i++)
            {
                metin += "" + (i + 1).ToString() + "- " + secenekler[i] + " " + oylar[i] + "\n\n";
            }
            string title = "Anket";
            if (l == 0) title = "Poll";
            var eb = new EmbedBuilder() { Title = ":clipboard: " + title, Description = metin, Color = Color.LightOrange };
            await Context.Channel.SendMessageAsync("", false, eb);
            sorust = null;

        }
        [Command("say")]
        [Alias("gerisay", "geriyesay", "count", "countback", "countdown")]
        public async Task Gerisay(int zaman, string birim = "sn")
        {
            if (birim == "dk" || birim == "dakika" || birim == "m" || birim == "minute")
            {
                üstlimitsaniye = zaman * 60;
                if (l == 1) await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
                else await Context.Channel.SendMessageAsync("Countdown has started. Countdown will finish at **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            }
          /*  if (birim == "dakika")
            {
                üstlimitsaniye = zaman * 60;
                if (l == 1) await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
                else await Context.Channel.SendMessageAsync("Countdown has started. Countdown will finish at **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            } */
            if (birim == "sn" || birim == "saniye" || birim == "s" || birim == "second")
            {
                üstlimitsaniye = zaman;
                if (l == 1) await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
                else await Context.Channel.SendMessageAsync("Countdown has started. Countdown will finish at **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            }
           /* if (birim == "saniye")
            {
                üstlimitsaniye = zaman;
                if (l == 1) await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
                else await Context.Channel.SendMessageAsync("Countdown has started. Countdown will finish at **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            } */
            ch = Context.Channel;

            Timer t = new Timer(1000);
            t.Elapsed += async (sender, e) => await timerislem();
            t.Start();
        }

        [Command("Rastgele")]
        [Alias("Rasgele", "Random")]
        public async Task Rastgele(int altlimit, int üstlimit)
        {
            int sonuc;
            Random rastgele = new Random();
            sonuc = rastgele.Next(altlimit, üstlimit);
            await Context.Channel.SendMessageAsync("**" + sonuc.ToString() + "**");
        }

        [Command("apikey")]
        [Alias("api", "key")]
        [Summary("")]
        public async Task apikey(string key)
        {
            riot.apikey = key;
            await Context.Channel.SendMessageAsync("API anahtarı güncellendi.");
        }
        [Command("Davet")]
        [Alias("Invite","Createinvite","davetoluştur","davetolustur")]
        public async Task Davet()
        {
            IInvite davet = await (Context.Channel as IGuildChannel).CreateInviteAsync();
            var eb = new EmbedBuilder() { Title = "Davet", Description = davet.Url, Color = Color.Blue };
            await Context.Channel.SendMessageAsync("", false, eb);
        }
        [Command("Çağır")]
        [Alias("Cagir","Gel")]
        public async Task deneme()
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();
            var stream = client.CreatePCMStream(AudioApplication.Music);
        }
        [Command("isim")]
        [Summary("")]
        [Alias("ad","name")]
        public async Task isim(IUser kullanici, [Remainder] string isim)
        {
            string eski = kullanici.Username;
            await (kullanici as IGuildUser).ModifyAsync(x => x.Nickname = isim);
            await Context.Channel.SendMessageAsync("**" + eski + "**" + " isimli kullanıcının takma adı " + kullanici.Mention + " olarak ayarlandı");
        }
        [Command("Oyun")]
        [Alias("Oyunayarla")]
        public async Task Oyun([Remainder] string oyun)
        {
           await (Context.Client as DiscordSocketClient).SetGameAsync(oyun);
            if (l == 1) await Context.Channel.SendMessageAsync("Oyun " + "**" + oyun + "**" + " Olarak ayarlandı.");
            else await Context.Channel.SendMessageAsync("Game is set as " + "**" + oyun + "**" + ".");
        }
        [Command("Saat")]
        [Summary("")]
        [Alias("Zaman","Tarih","Gün","Ay","Time","hour","clock","Month","Day")]
        public async Task Saat()
        {
            if (l == 1) await Context.Channel.SendMessageAsync("Sistem zamanı: " + "**" + DateTime.Now.ToString() + "**");
            else await Context.Channel.SendMessageAsync("System time " + "**" + DateTime.Now.ToString() + "**");
        }
        [Command("Asal")]
        [Alias("prime","primenumber","isprime","isprimenumber")]
        public async Task Asal(int num1)
        {
            if (num1 == 0 || num1 == 1)
            {
                //  Console.WriteLine(num1 + " is not prime number");
                if (l == 1) await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " Bir asal sayı değil.");
                else await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " is not a prime number.");
                
            }
            else
            {
                for (int a = 2; a <= num1 / 2; a++)
                {
                    if (num1 % a == 0)
                    {
                        if (l == 1) await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " Bir asal sayı değil.");
                        else await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " is not a prime number.");
                        return;
                    }

                }
                if (l == 1) await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " Bir asal sayı.");
                else await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " is a prime number.");
            }
        }
        [Command("Hesapla")]
        [Summary("")]
        [Alias("İşlem","Hesap","calculate")]
        public async Task Hesapla(double sayi1 = 0, string isaret = "", double sayi2 = 0)
        {
            if(sayi1 == 0)
            {
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Neyi?");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + " Calculate what?");
            }
            if(isaret == "+")
            {
                double sonuc;
                sonuc = sayi1 + sayi2;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " " + sonuc.ToString());
                
            }
            if(isaret == "-")
            {
                double sonuc;
                sonuc = sayi1 - sayi2;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " " + sonuc.ToString());
            }
            if(isaret == "*")
            {
                double sonuc;
                sonuc = sayi1 * sayi2;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " " + sonuc.ToString());
            }
            if(isaret == "%")
            {
                double sonuc;
                sonuc = (sayi1 * sayi2) / 100;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " " + sonuc.ToString());
            }
            if (isaret == "^")
            {
                double sonuc = sayi1;
                for(double i = sayi2; i > 1; i--)
                {
                    sonuc = sonuc * sayi1;
                }
                await Context.Channel.SendMessageAsync(Context.User.Mention + " " + sonuc.ToString());
            }
            if (isaret == "!")
            {
                double sonuc = sayi1;
                for(double i = sayi1 - 1; i > 0; i--)
                {
                    sonuc = sonuc * i;
                }
                await Context.Channel.SendMessageAsync(Context.User.Mention + " " + sonuc.ToString());
            }
        }
        [Command("Embed")]
        [Summary("")]
        public async Task Embed(string başlık, [Remainder] string açıklama)
        {
            await Context.Message.DeleteAsync();
            var eb = new EmbedBuilder() { Title = "**" + başlık + "**", Description = açıklama, Color = Color.Blue };
            await Context.Channel.SendMessageAsync("", false, eb); 
        }
        [Command("Sağırlaştır")]
        [Alias("Sağır","deaf","deafen","Sagir","Sagirlastir")]
        public async Task Sağırlaştır(IUser kullanici)
        {
            if ((Context.User as IGuildUser).GetPermissions(Context.Channel as ITextChannel).ManageChannel)
            {
                if ((kullanici as IGuildUser).IsDeafened == false)
                {
                    await (kullanici as IGuildUser).ModifyAsync(x => x.Deaf = true);
                    if (l == 1) await Context.Channel.SendMessageAsync(kullanici.Mention + " sağırlaştırıldı.");
                    else await Context.Channel.SendMessageAsync(kullanici.Mention + " is deafened.");
                }
                else
                {
                    await (kullanici as IGuildUser).ModifyAsync(x => x.Deaf = false);
                    if (l == 1) await Context.Channel.SendMessageAsync(kullanici.Mention + " sağırlaştırılması kaldırıldı.");
                    else await Context.Channel.SendMessageAsync(kullanici.Mention + " is undeafened");
                }
            }
            else
            {
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Bir kullanıcıyı sağırlaştırmak için sunucuda kanalları yönetme yetkisine sahip olmalısınız");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + " You need to have 'Manage Channels' permission in server to deafen a user.");
            }

        }
        [Command("Sustur")]
        [Alias("Mute")]
        public async Task Sustur(IUser kullanici)
        {
            if ((Context.User as IGuildUser).GetPermissions(Context.Channel as ITextChannel).ManageChannel)
            {
                if ((kullanici as IGuildUser).IsMuted == false)
                {
                    await (kullanici as IGuildUser).ModifyAsync(x => x.Mute = true);
                    if (l == 1) await Context.Channel.SendMessageAsync(kullanici.Mention + " susturuldu.");
                    else await Context.Channel.SendMessageAsync(kullanici.Mention + " is muted.");
                }
                else
                {
                    await (kullanici as IGuildUser).ModifyAsync(x => x.Mute = false);
                    if (l == 1) await Context.Channel.SendMessageAsync(kullanici.Mention + " susturulması kaldırıldı.");
                    else await Context.Channel.SendMessageAsync(kullanici.Mention + " is unmuted.");
                }
            }
            else
            {
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Bir kullanıcıyı susturmak için sunucuda kanalları yönetme yetkisine sahip olmanız gerek.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + " You need to have 'Manage Channels' permission in server to mute a user.");
            }
        }
        [Command("Taşı")]
        [Summary("")]
        [Alias("Tasi")]
        public async Task taşı(IUser kullanici,IChannel kanal)
        {
            if ((Context.User as IGuildUser).GetPermissions(Context.Channel as ITextChannel).ManageChannel)
            {
                await (kullanici as IGuildUser).ModifyAsync(x => x.ChannelId = kanal.Id);
            }
            else
            {
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Mention + " Bir kullanıcının ses kanalını değiştirmek için sunucuda kanalları yönetme yetkisine sahip olmanız gerek.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + " You need to have 'Manage Channels' permission in server to move a user to a different voice channel.");
            }
        }
        

        [Command("Yardım")]
        [Alias("Komutlar","Help","Yardim")]
        public async Task Yardim()
        {
            string description = "\n!sil <mesaj sayısı> {kullanici} \n!sihirdar <sihirdar ismi>\n!taşı <kullanıcı> <kanal>\n!sustur <kullanıcı>\n!sağırlaştır <kullanıcı>\n!embed <başlık> <açıklama>\n!hesapla <sayı> <işlem> <sayı>\n!saat" +
                "\n!oyun <isim>\n!isim <kullanıcı> <yeni isim>\n!davet\n!̶a̶p̶i̶k̶e̶y̶ ̶<̶R̶i̶o̶t̶ ̶a̶p̶i̶ ̶a̶n̶a̶h̶t̶a̶r̶ı̶>̶\n!rastgele <alt limit> <üst limit>\n!ping\n!asal <sayı>\n!say <süre> {Sn / Dk}\n!anket <soru>\n!secenekler <secenek> <secenek> ..\n!oy <secenek>\n!anketbitir"
                + "\n!hava <şehir>\n!havaradar";
            
            var eb = new EmbedBuilder() { Title = "Github için tıklayın", Description = "Kullanılabilir komutlar: \nSüslü parantez içerisinde olan parametreler isteğe bağlıdır.\n**" + description + "**", Color = Color.Blue };
            var fb = new EmbedFieldBuilder();
            eb.WithUrl("https://www.github.com/tmr0222/SmileBot");
            await Context.Channel.SendMessageAsync("",false,eb);
        }
        [Command("Sihirdar")]
        public async Task Sihirdar([Remainder] string isim)
        {
            try
            {
                if (l == 1) await Context.Channel.SendMessageAsync(isim + " Aranıyor...");
                else await Context.Channel.SendMessageAsync("Searching for " + isim + "...");
                kills = new string[5];
                deaths = new string[5];
                assists = new string[5];
                result = new string[5];
                isim.Replace(" ", "%20");


                // 1
                riot.summonerbul(isim);
                riot.macbilgi(riot.summonerid, riot.mac1id);
                kills[0] = riot.kill;
                deaths[0] = riot.death;
                assists[0] = riot.assist;
                result[0] = riot.macsonuc;
                // 2
                riot.summonerbul(isim);
                riot.macbilgi(riot.summonerid, riot.mac2id);
                kills[1] = riot.kill;
                deaths[1] = riot.death;
                assists[1] = riot.assist;
                result[1] = riot.macsonuc;
                // 3
                riot.summonerbul(isim);
                riot.macbilgi(riot.summonerid, riot.mac3id);
                kills[2] = riot.kill;
                deaths[2] = riot.death;
                assists[2] = riot.assist;
                result[2] = riot.macsonuc;
                // 4
                riot.summonerbul(isim);
                riot.macbilgi(riot.summonerid, riot.mac4id);
                kills[3] = riot.kill;
                deaths[3] = riot.death;
                assists[3] = riot.assist;
                result[3] = riot.macsonuc;
                // 5
                riot.summonerbul(isim);
                riot.macbilgi(riot.summonerid, riot.mac5id);
                kills[4] = riot.kill;
                deaths[4] = riot.death;
                assists[4] = riot.assist;
                result[4] = riot.macsonuc;

                var eb = new EmbedBuilder() { Title = "", Description = "**Maçlar**", Color = Color.Blue };
                if (l == 0) eb = new EmbedBuilder() { Title = "", Description = "**Matches**", Color = Color.Blue };
                string newname = riot.summonername;
                newname.First().ToString().ToUpper();
                EmbedAuthorBuilder MyAuthorBuilder = new EmbedAuthorBuilder();
                if (l == 1) MyAuthorBuilder.WithName(newname + " - Tüm profil için tıkla");
                else MyAuthorBuilder.WithName(newname + " - Click for complete profile");
                MyAuthorBuilder.WithIconUrl("https://i.hizliresim.com/JlvA4B.jpg");
                eb.WithAuthor(MyAuthorBuilder);
                for (int i = 0; i < 5; i++)
                {
                    if (result[i] == "Zafer")
                    {
                      if(l == 1)  result[i] = ":white_check_mark: Zafer";
                      else result[i] = ":white_check_mark: Win";
                    }
                    if (result[i] == "Bozgun")
                    {
                       if(l == 1) result[i] = ":no_entry_sign: Bozgun";
                       else result[i] = ":no_entry_sign: Loss";
                    }
                }
                EmbedFieldBuilder MyEmbedField = new EmbedFieldBuilder();
                MyEmbedField.WithIsInline(true);
                MyEmbedField.WithName(result[0]);
                MyEmbedField.WithValue(kills[0] + "/" + deaths[0] + "/" + assists[0]);
                eb.AddField(MyEmbedField);

                EmbedFieldBuilder MyEmbedField2 = new EmbedFieldBuilder();
                MyEmbedField2.WithIsInline(true);
                MyEmbedField2.WithName(result[1]);
                MyEmbedField2.WithValue(kills[1] + "/" + deaths[1] + "/" + assists[1]);
                eb.AddField(MyEmbedField2);

                EmbedFieldBuilder MyEmbedField3 = new EmbedFieldBuilder();
                MyEmbedField3.WithIsInline(true);
                MyEmbedField3.WithName(result[2]);
                MyEmbedField3.WithValue(kills[2] + "/" + deaths[2] + "/" + assists[2]);
                eb.AddField(MyEmbedField3);

                EmbedFieldBuilder MyEmbedField4 = new EmbedFieldBuilder();
                MyEmbedField4.WithIsInline(true);
                MyEmbedField4.WithName(result[3]);
                MyEmbedField4.WithValue(kills[3] + "/" + deaths[3] + "/" + assists[3]);
                eb.AddField(MyEmbedField4);

                EmbedFieldBuilder MyEmbedField5 = new EmbedFieldBuilder();
                MyEmbedField5.WithIsInline(true);
                MyEmbedField5.WithName(result[4]);
                MyEmbedField5.WithValue(kills[4] + "/" + deaths[4] + "/" + assists[4]);
                eb.AddField(MyEmbedField5);

                EmbedFieldBuilder MyEmbedField6 = new EmbedFieldBuilder();
                MyEmbedField6.WithIsInline(true);
                MyEmbedField6.WithName("...");
                MyEmbedField6.WithValue("...");
                eb.AddField(MyEmbedField6);

                eb.WithUrl("http://www.lolking.net/summoner/tr/" + riot.summonerid2);

                await Context.Channel.SendMessageAsync("", false, eb);
            }
            catch(Exception e)
            {
                if (l == 1) await Context.Channel.SendMessageAsync("```İşlemi gerçekleştirirken bir hata meydana geldi.\n" + e.Message + "\nAPI Anahtarını güncelleyin. \ndeveloper.riotgames.com```");
                else await Context.Channel.SendMessageAsync("```Error: \n" + e.Message + "" + "\nUpdate the API key. \ndeveloper.riotgames.com```");
            }
        }

        [Command("Sil")]
        [Summary("Mesajları siler")]
        public async Task Sil(int sayi, IUser kullanici = null)
        {
            await Context.Message.DeleteAsync();
            if ((Context.User as IGuildUser).GetPermissions(Context.Channel as ITextChannel).ManageMessages)
            {
                int Amount = 0;
                foreach (var Item in await Context.Channel.GetMessagesAsync(sayi).Flatten())
                {
                    if (kullanici != null)
                    {
                        if (Item.Author == kullanici)
                        {
                            await Item.DeleteAsync();
                        }
                    }
                    else
                    {
                        Amount++;
                        await Item.DeleteAsync();
                    }


                }
            }
            else
            {
                if (l == 1) await Context.Channel.SendMessageAsync(Context.User.Username + " Mesaj silebilmeniz için sunucuda mesajları yönetme yetkisine sahip olmanız gerek.");
                else await Context.Channel.SendMessageAsync(Context.User.Mention + " You need to have 'Manage Messages' permission in server to delete messages.");
            }
            
        }
        private async Task timerislem()
        {
            saniye += 1;
            if(saniye == üstlimitsaniye)
            {
                if (l == 1) await ch.SendMessageAsync("Geri sayım bitti. (**" + üstlimitsaniye + "** saniye)");
                else await ch.SendMessageAsync("Countdown is over. (**" + üstlimitsaniye + "** seconds)");
            }
        }
    }
}            
