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

namespace SmileBo
{

    public class benimbot : ModuleBase
    {

       string[] kills;
       string[] deaths;
       string[] assists;
       string[] result;
       static int üstlimitsaniye;
       static int saniye;
       public  static string[] secenekler;
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
        [Command("Anket")]
        [Summary("")]
        public async Task Anket([Remainder] string soru = "")
        {
           sorust = soru;
           await Context.Channel.SendMessageAsync(Context.User.Mention + " Soru ayarlandı. Seçenekleri ayarlayın !secenekler <secenek> <secenek> ...");
        }
        [Command("Secenekler")]
        public async Task secenek(string secenek1 = "", string secenek2 = "", string secenek3 = "", string secenek4 = "", string secenek5 = "")
        {
            if(sorust == "") {
                await Context.Channel.SendMessageAsync("İlk önce soruyu ayarlayın. !anket <soru>");
                return;
            }
           if(secenek2 == null)
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention + " Bir anket oluşturabilmeniz için en az 2 seçenek olmalı.");
                return;
            }
            secenekler = new string[5];
            secenekler[0] = secenek1;
            secenekler[1] = secenek2;
            secenekler[2] = secenek3;
            secenekler[3] = secenek4;
            secenekler[4] = secenek5;
            secenekler = secenekler.Except(new string[] {""}).ToArray();
            int sayi = 1;
            string metin = Context.User.Mention + " Bir oylama oluşturdu. !oy <numara> \n\n" + sorust + "\n";
            foreach(var vr in secenekler)
            {
                metin += "\n\n**" + sayi + "- " + vr + "**";
                sayi += 1; 
            }
            var eb = new EmbedBuilder() { Title = ":clipboard: Anket", Description = metin, Color = Color.LightOrange };
            await Context.Channel.SendMessageAsync("", false, eb);
            idler = new ulong[100];
        }
        [Command("Oy")]
        public async Task Oy(int secenek)
        {
            foreach(var vr in idler)
            {
                if(Context.User.Id == vr)
                {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " Zaten oy verdiniz.");
                    return;
                }
            }
            oylar = new int[secenekler.Length];
            if(secenek == 1)
            {
                oylar[0] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[0] + "** isimli seçeneğe oy verdiniz.");
            }
            if(secenek == 2)
            {
                oylar[1] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[1] + "** isimli seçeneğe oy verdiniz.");
            }
            if(secenek == 3)
            {
                oylar[2] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[2] + "** isimli seçeneğe oy verdiniz.");
            }
            if(secenek == 4)
            {
                oylar[3] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[3] + "** isimli seçeneğe oy verdiniz.");
            }
            if(secenek == 5)
            {
                oylar[4] += 1;
                idler[idlersira] = Context.User.Id;
                idlersira += 1;
                await Context.Channel.SendMessageAsync(Context.User.Mention + " **" + secenekler[4] + "** isimli seçeneğe oy verdiniz.");
            } 
        }
        [Command("Anketbitir")]
        [Summary("")]
        public async Task Anketbitir()
        {
            if (secenekler[2] == null)
            {
                await Context.Channel.SendMessageAsync(Context.User.Mention + " Bitirilecek bir anket yok");
                return;
            } 
            string metin = "Oylama bitti. Sonuçlar: \n\n";
            int maxvalue = oylar.Max();
            int sıra = oylar.ToList().IndexOf(maxvalue);
           // await Context.Channel.SendMessageAsync(secenekler[sıra]);
           for(int i = 0; i< secenekler.Length; i++)
            {
                if (i == sıra)
                {
                    metin += "**" + (i + 1).ToString() + "- " + secenekler[i] + " " + oylar[i] + "**\n\n";
                }
                else
                {
                    metin += "" + (i + 1).ToString() + "- " + secenekler[i] + " " + oylar[i] + "\n\n";
                }
            }
            var eb = new EmbedBuilder() { Title = ":clipboard: Anket", Description = metin, Color = Color.LightOrange };
            await Context.Channel.SendMessageAsync("", false, eb);
            sorust = null;
            
        }
        [Command("say")]
        public async Task Gerisay(int zaman, string birim)
        {
            if(birim == "dk")
            {
                üstlimitsaniye = zaman * 60;
                await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            }
            if(birim == "dakika")
            {
                üstlimitsaniye = zaman * 60;
                await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            }
            if(birim == "sn")
            {
                üstlimitsaniye = zaman;
                await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            }
            if(birim == "saniye")
            {
                üstlimitsaniye = zaman;
                await Context.Channel.SendMessageAsync("Geri sayım başladı. Biteceği zaman: **" + DateTime.Now.AddSeconds(üstlimitsaniye).ToString() + "**");
            }
            ch = Context.Channel;
            
            Timer t = new Timer(1000);
            t.Elapsed += async (sender, e) => await timerislem();
            t.Start();
        }

        [Command("Rastgele")]
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
        [Summary("")]
        public async Task Davet()
        {
            IInvite davet = await (Context.Channel as IGuildChannel).CreateInviteAsync();
            var eb = new EmbedBuilder() { Title = "Davet", Description = davet.Url, Color = Color.Blue };
            await Context.Channel.SendMessageAsync("", false, eb);
            
        }
        [Command("Çağır")]
        [Summary("")]
        public async Task deneme()
        {
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();
            var stream = client.CreatePCMStream(AudioApplication.Music);
        }
        [Command("isim")]
        [Summary("")]
        [Alias("ad","name")]
        public async Task isim(IUser kullanici, string isim)
        {
            string eski = kullanici.Username;
            await (kullanici as IGuildUser).ModifyAsync(x => x.Nickname = isim);
            await Context.Channel.SendMessageAsync("**" + eski + "**" + " isimli kullanıcının takma adı " + kullanici.Mention + " olarak ayarlandı");
        }
        [Command("Oyun")]
        [Summary("")]
        public async Task Oyun([Remainder] string oyun)
        {
           await (Context.Client as DiscordSocketClient).SetGameAsync(oyun);
           await Context.Channel.SendMessageAsync("Oyun " + "**" + oyun + "**" + " Olarak ayarlandı.");
        }
        [Command("Saat")]
        [Summary("")]
        [Alias("Zaman","Tarih","Gün","Ay")]
        public async Task Saat()
        {
            await Context.Channel.SendMessageAsync("Sistem zamanı: " + "**" + DateTime.Now.ToString() + "**");
        }
        [Command("Asal")]
        [Summary("")]
        public async Task Asal(int num1)
        {
            if (num1 == 0 || num1 == 1)
            {
                //  Console.WriteLine(num1 + " is not prime number");
               await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " Bir asal sayı değil.");
                
            }
            else
            {
                for (int a = 2; a <= num1 / 2; a++)
                {
                    if (num1 % a == 0)
                    {
                        await Context.Channel.SendMessageAsync("**" + num1.ToString() + "**" + " Bir asal sayı değil.");
                        return;
                    }

                }
                await Context.Channel.SendMessageAsync("**" + num1.ToString()+ "**" + " Bir asal sayı.");
            }
        }
        [Command("Hesapla")]
        [Summary("")]
        [Alias("İşlem","Hesap")]
        public async Task Hesapla(double sayi1 = 0, string isaret = "", double sayi2 = 0)
        {
            if(sayi1 == 0)
            {
               await Context.Channel.SendMessageAsync(Context.User.Mention + " Neyi?");
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
  
            var eb = new EmbedBuilder() { Title = "**" + başlık + "**", Description = açıklama, Color = Color.Blue };
            await Context.Channel.SendMessageAsync("", false, eb); 
        }
        [Command("Sağırlaştır")]
        [Alias("Sağır")]
        [Summary("")]
        public async Task Sağırlaştır(IUser kullanici)
        {
            if ((kullanici as IGuildUser).IsDeafened == false)
            {
                await (kullanici as IGuildUser).ModifyAsync(x => x.Deaf = true);
               await Context.Channel.SendMessageAsync(kullanici.Mention + " sağırlaştırıldı.");
            }
            else
            {
                await (kullanici as IGuildUser).ModifyAsync(x => x.Deaf = false);
                await Context.Channel.SendMessageAsync(kullanici.Mention + " sağırlaştırılması kaldırıldı.");
            }

        }
        [Command("Sustur")]
        [Alias("Mute")]
        [Summary("")]
        public async Task Sustur(IUser kullanici)
        {
            if ((kullanici as IGuildUser).IsMuted == false) {
                await (kullanici as IGuildUser).ModifyAsync(x => x.Mute = true);
                await Context.Channel.SendMessageAsync(kullanici.Mention + " susturuldu.");
               }
            else
            {
                await (kullanici as IGuildUser).ModifyAsync(x => x.Mute = false);
                await Context.Channel.SendMessageAsync(kullanici.Mention + " susturulması kaldırıldı");
            }
            
        }
        [Command("Taşı")]
        [Summary("")]
        public async Task taşı(IUser kullanici,IChannel kanal)
        {
            await (kullanici as IGuildUser).ModifyAsync(x => x.ChannelId = kanal.Id);
        }
        

        [Command("Yardım")]
        [Alias("Komutlar")]
        public async Task Yardim()
        {
            string description = "\n!sil <mesaj sayısı>\n!sihirdar <sihirdar ismi>\n!taşı <kullanıcı ismi> <kanal>\n!sustur <kullanıcı ismi>\n!sağırlaştır <kullanıcı ismi>\n!embed <başlık> <açıklama>\n!hesapla <sayı> <işlem> <sayı>\n!saat" + 
                "\n!oyun <isim>\n!isim <kullanici ismi> <yeni isim>\n!davet\n!apikey <Riot api anahtarı>\n!rastgele <alt limit> <üst limit>\n!ping\n!asal <sayı>\n!say <süre> <Sn / Dk>";
            var eb = new EmbedBuilder() { Title = "Kullanılabilir Komutlar", Description = "**" + description + "**", Color = Color.Blue };
            await Context.Channel.SendMessageAsync("",false,eb);
        }
        [Command("Sihirdar")]
        public async Task Sihirdar([Remainder] string isim)
        {
           await Context.Channel.SendMessageAsync(isim + " Aranıyor...");
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
            string newname = riot.summonername;
            newname.First().ToString().ToUpper();
            EmbedAuthorBuilder MyAuthorBuilder = new EmbedAuthorBuilder();
            MyAuthorBuilder.WithName(newname + " - Tüm profil için tıkla");
            MyAuthorBuilder.WithIconUrl("https://i.hizliresim.com/JlvA4B.jpg");
            eb.WithAuthor(MyAuthorBuilder);
            for (int i = 0; i < 5; i++)
            {
                if (result[i] == "Zafer")
                {
                    result[i] = ":white_check_mark: Zafer";
                }
                if (result[i] == "Bozgun")
                {
                    result[i] = ":no_entry_sign: Bozgun";
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

        [Command("Sil")]
        [Summary("Mesajları siler")]
        public async Task Sil(int sayi, string kullanici = null)
        {
            
            int Amount = 0;
            foreach (var Item in await Context.Channel.GetMessagesAsync(sayi).Flatten())
            {
                if (kullanici != null)
                {
                    if (Item.Author.Username == kullanici)
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
        private async Task timerislem()
        {
            saniye += 1;
            if(saniye == üstlimitsaniye)
            {
              await  ch.SendMessageAsync("Geri sayım bitti. (**" + üstlimitsaniye + "** saniye)");
            }
        }
    }
}            
