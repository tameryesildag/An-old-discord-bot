using System;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.API;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace Kodumunbot
{
    
  public class benimbot
    {
        static int oyuncunumarasi = 0;
        static int z = 0;
        static int a = 0; 
        public static int i = 0;
        public static int i2 = 0;
        public static int temp = 0;
        public static int temp2 = 0;
        public static int tomp = 0;
        public static int limitstatik = 5;
        static DiscordClient discord;
        public static string orospucocugu = "yok";
        public static Timer zamanlayici = new Timer();
        public static Timer zamanlayici2 = new Timer();
        static Channel yazilanchannel;
        static string statiksecenek1;
        static string statiksecenek2;
        static string statiksecenek3;
        static string statiksecenek4;
        static bool gerisayimvar = false;
        static int sec1oy = 0;
        static int sec2oy = 0;
        static int sec3oy = 0;
        static int sec4oy = 0;
        static bool oldu1 = false;
        static bool oldu2 = false;
        static string[] oyverecekuyeler;
        static bool oylamabaslatildi = false;
        static bool oyverebilir = true;
        static bool ilkbaslayis = true;
        int cfpara = 0;
        string cfoyuncu1 = "Yok";
        string cfoyuncu2 = "Yok";
        bool cf = false;
        string gercekanahtar = "sifre123";
        Oyuncu[] oyuncular;
        static Ruletoyuncu[] ruletoyuncular;
        bool cfkatilinmis = false;
        static bool ruleticinsay = false;
        string yasaklananuser = "";
        public static Message[] ruletmesaj;
        int ruletoyuncusayi;
        string boslukluhali;
        public benimbot()
        {


            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = log;

            });
            discord.UsingCommands(x =>
            {

                x.PrefixChar = Convert.ToChar("!");

                Console.WriteLine("Prefix Char = " + x.PrefixChar);
                x.AllowMentionPrefix = true;
            });

            var commands = discord.GetService<CommandService>();
            
            commands.CreateCommand("ping")
                .Do(async (e) =>
                {
                  await e.Channel.SendMessage("pong");
                });
            commands.CreateCommand("deneme").Parameter("metin",ParameterType.Required)
                .Do(async (e) =>
                {
                    string metin = e.GetArg("metin");
                    Replacer replacer_ = new Replacer();
                    replacer_.Replace(metin);
                    await e.Channel.SendMessage(replacer_.replaced);
                });
            //test
            commands.CreateCommand("sil-ozel").Parameter("isim",ParameterType.Required).Parameter("sayi",ParameterType.Required)
          .Do(async (e) =>
           {
               int silinenmesajsayisi = 0;
               int mesajsayisi = Int32.Parse(e.GetArg("sayi"));
               string isim = e.GetArg("isim");
               Message[] silinecekmesajlar;
               silinecekmesajlar = await e.Channel.DownloadMessages(mesajsayisi + 1);
            //   await e.Channel.SendMessage(silinecekmesajlar[1].User.Name);
               foreach (Message mesaj in silinecekmesajlar)
               {
                   if (mesaj.User.Name == isim)
                   {
                      await mesaj.Delete();
                       silinenmesajsayisi += 1;
                   }
                   else
                   {
                    //   await e.Channel.SendMessage(mesaj.User.Name);
                   }
               }
               await e.Channel.SendMessage(e.User.Mention + " son " + mesajsayisi + " mesajdan " + silinenmesajsayisi + " tanesi " + isim + " adlı kişi tarafından gönderildiği için silindi");
           });
            commands.CreateCommand("mute").Parameter("isim",ParameterType.Required).Parameter("anahtar",ParameterType.Required)
                .Do(async (e) =>
                {
                    string girilenanahtar;
                    girilenanahtar = e.GetArg("anahtar");
                    if (girilenanahtar == gercekanahtar)
                    {
                        yasaklananuser = e.GetArg("isim");
                        await e.Channel.SendMessage(yasaklananuser + " İsimli kişinin konuşması yasaklandı");
                    }
                });
            discord.MessageReceived += async (s, e) =>
            {
                if(e.User.Name == yasaklananuser)
                {
                    await e.Message.Delete();
                }
            };
            commands.CreateCommand("rulet").Parameter("secim", ParameterType.Required).Parameter("ypara", ParameterType.Optional).Parameter("renk", ParameterType.Optional)
                .Do(async (e) =>
                {
                    string secim = e.GetArg("secim");
                    if(secim == "baslat")
                    {
                        Timerbaslat(1000, 9999);
                        ruleticinsay = true;
                        yazilanchannel = e.Channel;
                        Random rastgele = new Random();
                        if (ilkbaslayis)
                        {
                            tomp = rastgele.Next(1, 12);
                            ilkbaslayis = false;
                        }
                        await e.Channel.SendMessage("Rulet Dönüyor...");
                        await e.Channel.SendMessage(":hourglass: :hourglass:  :hourglass:  :hourglass:  :hourglass:  :hourglass:  :hourglass:  :hourglass:  :hourglass: :hourglass: :hourglass: :hourglass:  :hourglass: ");
                    }
                    if(secim == "yatir")
                    {
                        if(e.GetArg("ypara") != null)
                        {
                            Console.WriteLine("Rulete para yatiriliyor (Asama 1)");
                            ruletoyuncular = new Ruletoyuncu[10];
                            ruletoyuncular[ruletoyuncusayi] = new Ruletoyuncu();
                            Console.WriteLine("Rulete para yatiran oyuncunun bilgileri yukleniyor (Asama 2)");
                            ruletoyuncular[ruletoyuncusayi].isim = e.User.Name;
                            Console.WriteLine("oyuncunun ismi belirleniyor");
                            Console.WriteLine("Oyuncudan gelen para" + e.GetArg("ypara"));
                            int ppara = Int32.Parse(e.GetArg("ypara"));
                            Console.WriteLine("Para integer degerine cevrildi. " + ppara.ToString());
                            Console.WriteLine("oyuncunun yatiracagi para belirleniyor");
                            ruletoyuncular[ruletoyuncusayi].yatirdigipara = ppara;
                            string yatiracagirenk = "yok";
                            string prenk = e.GetArg("renk");
                            Console.WriteLine("Renk belirleniyor (Asama 3)");
                            if (prenk == "kirmizi")
                            {
                                yatiracagirenk = ":red_circle:";
                                await e.Channel.SendMessage(e.User.Mention + " Kırmızıya " + ppara + " yatırdınız");
                            }
                            if(prenk == "siyah")
                            {
                                yatiracagirenk = ":black_circle:";
                                await e.Channel.SendMessage(e.User.Mention + " Siyaha " + ppara + " yatırdınız");
                            }
                            if(prenk == "yesil")
                            {
                                yatiracagirenk = ":green_heart:";
                                await e.Channel.SendMessage(e.User.Mention + " Yeşile " + ppara + " yatırdınız");
                            }

                            ruletoyuncular[ruletoyuncusayi].yatirdigirenk = yatiracagirenk;
                            Console.WriteLine("Bitti");   
                            
                        }
                    }
                });
            commands.CreateCommand("para-sil").Parameter("isim",ParameterType.Required).Parameter("para", ParameterType.Required).Parameter("Anahtar",ParameterType.Required)
                .Do(async (e) =>
                {
                    string girilenanahtar;
                    girilenanahtar = e.GetArg("Anahtar");
                    if(girilenanahtar == gercekanahtar)
                    {
                        int silinecek = Int32.Parse(e.GetArg("para"));
                        Writer _writer = new Writer();
                        _writer.parasil(e.GetArg("isim"), silinecek);
                        _writer.Read(e.GetArg("isim"));
                        await e.Channel.SendMessage(e.GetArg("isim") + " Adlı hesaptan " + silinecek.ToString() + " para silindi");
                    }
                    else
                    {
                        await e.Channel.SendMessage(e.User.Mention + " Yanlış anahtar");
                    }

                });
            commands.CreateCommand("coinflip").Parameter("secim", ParameterType.Required).Parameter("para", ParameterType.Optional)
                .Do(async (e) =>
                {
                    if (e.GetArg("secim") == "komutlar")
                    {
                        await e.Channel.SendMessage("!Coinflip olustur {para} - girilen miktarda bir coinflip başlatır");
                        await e.Channel.SendMessage("!Coinflip bilgi - devam etmekte olan coinflip ile ilglili bilgileri gösterir");
                        await e.Channel.SendMessage("!Coinflip katil - devam etmekte olan coinflipe katılınılır");
                        await e.Channel.SendMessage("!Coinflip baslat - Coinflipi baslatir");
                    }
                    if(e.GetArg("secim") == "olustur")
                    {
                       
                        cfpara = Int32.Parse(e.GetArg("para"));
                        if (cfpara != 0)
                        {
                            cfoyuncu1 = e.User.Name;
                            cf = true;
                            await e.Channel.SendMessage(e.User.Mention + " Bahis başlattı katılmak için !coinflip katil");
                            int silinecek = cfpara;
                            Writer _writer = new Writer();
                            _writer.parasil(cfoyuncu1, silinecek);
                            _writer.Read(cfoyuncu1);
                            await e.Channel.SendMessage(cfoyuncu1 + " Adlı hesaptan " + silinecek.ToString() + " para silindi");

                        }
                        else
                        {
                            await e.Channel.SendMessage("Tanımlanamayan para değeri");
                            return;
                        }
                        
                    }
                    if(e.GetArg("secim") == "katil")
                    {
                       
                            if (cf == true)
                            {
                            if (cfkatilinmis == false)
                            {
                                cfoyuncu2 = e.User.Name;
                                await e.Channel.SendMessage(e.User.Mention + " bahise katıldı");
                                cfkatilinmis = true;
                                int silinecek = cfpara;
                                Writer _writer = new Writer();
                                _writer.parasil(cfoyuncu2, silinecek);
                                _writer.Read(cfoyuncu2);
                                await e.Channel.SendMessage(cfoyuncu2 + " Adlı hesaptan " + silinecek.ToString() + " para silindi");
                            }
                            else
                            {
                                await e.Channel.SendMessage(e.User.Mention + " Bahis dolu");
                            }
                            }
                            else
                            {
                                await e.Channel.SendMessage(e.User.Mention + " Şuan katılınabilir bir bahis yok");
                            }
                        
                        
                    }
                    
                    if (e.GetArg("secim") == "baslat")
                    {
                        
                        if (cf == true)
                        {
                            if(cfoyuncu2 == "Yok")
                            {
                                await e.Channel.SendMessage("Katılımcı bulunamadı\nPara geri iade edilecek");
                                Writer _writer = new Writer();
                                _writer.paraekle(cfoyuncu1, cfpara);
                                _writer.Read(cfoyuncu1);
                                await e.Channel.SendMessage(cfoyuncu1 + " Adlı hesaba " + cfpara.ToString() + " geri iade edildi.");
                                return;
                            }
                            Random rastgele = new Random();
                            int sonuc = rastgele.Next(0, 2);

                            if (sonuc == 0)
                            {
                                
                                await e.Channel.SendMessage("[1] kazanan: " + cfoyuncu1);
                                await e.Channel.SendMessage("kazandığı para: " + cfpara.ToString());
                                int yuklenecekpara = 2 * cfpara;
                                Writer _writer = new Writer();
                                _writer.paraekle(cfoyuncu1, yuklenecekpara);
                                _writer.Read(cfoyuncu1);
                                await e.Channel.SendMessage(cfoyuncu1 + " Adlı hesaba " + yuklenecekpara.ToString() + " para yüklendi");
                            }
                            if (sonuc == 1)
                            {

                                
                                await e.Channel.SendMessage("[2] kazanan: " + cfoyuncu2);
                                await e.Channel.SendMessage("kazandığı para: " + cfpara.ToString());
                                int yuklenecekpara = 2 * cfpara;
                                Writer _writer = new Writer();
                                _writer.paraekle(cfoyuncu2, yuklenecekpara);
                                _writer.Read(cfoyuncu2);
                                await e.Channel.SendMessage(cfoyuncu2 + " Adlı hesaba " + yuklenecekpara.ToString() + " para yüklendi");
                            }
                            cfpara = 0;
                            cfkatilinmis = false;
                            cfoyuncu1 = null;
                            cfoyuncu2 = null;
                            cf = false;

                        }
                        else
                        {
                            await e.Channel.SendMessage("Başlatılacak bir bahis yok");
                        }
                    }
                    if(e.GetArg("secim") == "bilgi")
                    {
                        await e.Channel.SendMessage("Oyuncu 1 - " + cfoyuncu1);
                        await e.Channel.SendMessage("Oyuncu 2 - " + cfoyuncu2);
                        await e.Channel.SendMessage("Para: " + cfpara.ToString());
                    }


                });
            commands.CreateCommand("geriyesay").Parameter("mesaj", ParameterType.Required).Parameter("mesaj2", ParameterType.Optional)
                .Do(async (e) =>
                {
                    yazilanchannel = e.Channel;
                    if (gerisayimvar)
                    {
                        await e.Channel.SendMessage("Zaten bir gerisayim yapilmakta");
                    }
                if (gerisayimvar == false)
                    {
                        Timerbaslat(1000, Int32.Parse(e.GetArg("mesaj")));
                        await e.Channel.SendMessage("Sure baslatildi: " + e.GetArg("mesaj") + " saniye");
                        gerisayimvar = true;
                    }
                    // await fonksiyon(e);
                   //  await e.Channel.SendMessage(e.GetArg("mesaj"));


                });
            commands.CreateCommand("oylamabaslat").Parameter("secenek1", ParameterType.Optional).Parameter("secenek2", ParameterType.Optional).Parameter("secenek3", ParameterType.Optional).Parameter("secenek4", ParameterType.Optional).Parameter("secenek5", ParameterType.Optional)
                .Do(async (e) =>
                {
                    if (oylamabaslatildi == false)
                    {
                        sec1oy = 0;
                        sec2oy = 0;
                        sec3oy = 0;
                        sec4oy = 0;
                        oyverecekuyeler = new string[e.Server.UserCount];
                        await e.Channel.SendMessage("Oylama Oluşturuluyor...\nBu biraz zaman alabilir.");
                        try
                        {
                            await e.Channel.SendMessage(e.GetArg("secenek1"));
                            statiksecenek1 = e.GetArg("secenek1");
                        }
                        catch
                        {
                            await e.Channel.SendMessage("Oylamanın başlayabilmesi için en az 2 seçenek olmalıdır.");
                            return;
                        }
                        try
                        {
                            await e.Channel.SendMessage(e.GetArg("secenek2"));
                            statiksecenek2 = e.GetArg("secenek2");
                        }
                        catch
                        {
                            await e.Channel.SendMessage("Oylamanın başlayabilmesi için en az 2 seçenek olmalıdır.");
                            return;
                        }
                        try
                        {
                            statiksecenek3 = e.GetArg("secenek3");
                            await e.Channel.SendMessage(statiksecenek3);
                            oldu1 = true;
                        }
                        catch
                        {

                        }
                        try
                        {
                            statiksecenek4 = e.GetArg("secenek4");
                            await e.Channel.SendMessage(statiksecenek4);
                            oldu2 = true;
                        }
                        catch
                        {

                        }
                        await e.Channel.SendMessage("Oylama başlatıldı. !oyver {numara}");
                        oylamabaslatildi = true;
                        await e.Channel.SendMessage("1- " + statiksecenek1);
                        await e.Channel.SendMessage("2- " + statiksecenek2);
                        if (oldu1)
                        {
                            await e.Channel.SendMessage("3- " + statiksecenek3);
                        }
                        if (oldu2)
                        {
                            await e.Channel.SendMessage("4- " + statiksecenek4);
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("Zaten devam etmekte olan bir oylama var !oylamabitir");
                    }


                });
            commands.CreateCommand("oyver").Parameter("oyverileceksecenek", ParameterType.Required)
                .Do(async (e) =>
                {
                    if (oylamabaslatildi)
                    {
                        var userRoles = e.Server.Roles;
                        var rol = userRoles.Where(input => input.Name == "silenced").FirstOrDefault();
                        if (e.User.HasRole(rol))
                        {
                            await e.Channel.SendMessage(e.User.Mention + " Zaten oy verdiniz.");
                        }
                        else
                        {
                            if (e.GetArg("oyverileceksecenek") == "1")
                            {
                                sec1oy += 1;
                                await e.Channel.SendMessage(e.User.Mention + " 1 numaralı seçeneğe(" + statiksecenek1 + ") oy verdiniz");
                                await oyverildi(e);

                            }
                            if (e.GetArg("oyverileceksecenek") == "2")
                            {
                                sec2oy += 1;
                                await e.Channel.SendMessage(e.User.Mention + " 2 numaralı seçeneğe(" + statiksecenek2 + ") oy verdiniz");
                                await oyverildi(e);
                            }

                            if (e.GetArg("oyverileceksecenek") == "3")
                            {
                                sec3oy += 1;
                                await e.Channel.SendMessage(e.User.Mention + " 3 numaralı seçeneğe(" + statiksecenek3 + ") oy verdiniz");
                                await oyverildi(e);
                            }

                            if (e.GetArg("oyverileceksecenek") == "4")
                            {
                                sec4oy += 1;
                                await e.Channel.SendMessage(e.User.Mention + " 4 numaralı seçeneğe(" +statiksecenek4+ ") oy verdiniz");
                                await oyverildi(e);
                            }
                        }
                    }
                    else
                    {
                        await e.Channel.SendMessage("Devam etmekte olan bir oylama yok");
                    }
                });
            commands.CreateCommand("oylamabitir")
                .Do(async (e) =>
                {
                    if (oylamabaslatildi)
                    {
                        int[] oylar;
                        oylar = new int[4];
                        oylar[0] = sec1oy;
                        oylar[1] = sec2oy;
                        oylar[2] = sec3oy;
                        oylar[3] = sec4oy;
                        int enbuyukoy = oylar.Max();
                        if (sec1oy == enbuyukoy)
                        {
                            await e.Channel.SendMessage("Oylama bitti. Çıkan sonuç " + oylar.Max().ToString() + " oyla: " + statiksecenek1);
                        }
                        if (sec2oy == enbuyukoy)
                        {
                            await e.Channel.SendMessage("Oylama bitti. Çıkan sonuç " + oylar.Max().ToString() + " oyla: " + statiksecenek2);
                        }
                        if (sec3oy == enbuyukoy)
                        {
                            await e.Channel.SendMessage("Oylama bitti. Çıkan sonuç " + oylar.Max().ToString() + " oyla: " + statiksecenek3);
                        }
                        if (sec4oy == enbuyukoy)
                        {
                            await e.Channel.SendMessage("Oylama bitti. Çıkan sonuç " + oylar.Max().ToString() + " oyla: " + statiksecenek4);
                        }
                        await oybitti(e);
                        oylamabaslatildi = false;
                    }
                    else
                    {
                        await e.Channel.SendMessage("Devam etmekte olan bir oylama yok");
                    }
                 


                });
            commands.CreateCommand("ayril")
                .Do(async (e) =>
                {
                   
                   await discord.Disconnect();
                   Environment.Exit(1);


                });
            commands.CreateCommand("hesapla").Parameter("ilksayi", ParameterType.Required).Parameter("isaret", ParameterType.Required).Parameter("ikincisayi", ParameterType.Required)
                .Do(async (e) =>
                {
                    int ilksayi = Int32.Parse(e.GetArg("ilksayi"));
                    int ikincisayi = Int32.Parse(e.GetArg("ikincisayi"));
                    string isaret = e.GetArg("isaret");
                    if(isaret == "+")
                    {
                        int toplam = ilksayi + ikincisayi;
                        await e.Channel.SendMessage(ilksayi.ToString() + " + " + ikincisayi.ToString() + " = " + toplam.ToString());
                    }
                    if(isaret == "-")
                    {
                        int fark = ilksayi - ikincisayi;
                        await e.Channel.SendMessage(ilksayi.ToString() + " - " + ikincisayi.ToString() + " = " + fark.ToString());
                    }
                    if(isaret == "/")
                    {
                        int bolum = ilksayi / ikincisayi;
                        await e.Channel.SendMessage(ilksayi.ToString() + " / " + ikincisayi.ToString() + " = " + bolum.ToString());
                    }
                    if(isaret == "x")
                    {
                        int carpim = ilksayi * ikincisayi;
                        await e.Channel.SendMessage(ilksayi.ToString() + " x " + ikincisayi.ToString() + " = " + carpim.ToString());
                    }


                });
            commands.CreateCommand("orospucocugu").Parameter("mesaj3", ParameterType.Optional)
                .Do(async (e) =>
                {
                    
                        await e.Channel.SendMessage(orospucocugu);
                    
                    
                });
            discord.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;

            });
            commands.CreateCommand("yanimagel").Description("sa")
                .Do(async (e) =>
                {


                    var seskanal = e.User.VoiceChannel;                       // = discord.GetChannel(325570718484135937);
                    if(seskanal == null)
                    {
                        await e.Channel.SendMessage(e.User.Mention + " Bir sesli sohbet kanalında değilsiniz");
                    }
                    await e.Channel.SendMessage(seskanal.Name + " isimli kanala aktariliyor");
                    var _vClient = await discord.GetService<AudioService>() // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                            .Join(seskanal); // Join the Voice Channel, and return the IAudioClient.



                });
            commands.CreateCommand("orospucocugusec").Parameter("mesaj3", ParameterType.Optional)
                .Do(async (e) =>
                {

                   orospucocugu = e.GetArg("mesaj3");
                    await e.Channel.SendMessage("Orospu cocugu secildi!\n!orospucocugu");
                    

                });
            commands.CreateCommand("sunger")
                .Do(async (e) =>
                {

                    await e.Channel.SendFile("sponge.jpg");
                });
            commands.CreateCommand("yardim")
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage("!geriyesay {saniye} - Geriye sayma işlemi başlatır \n!sunger - sunger resmi gönderir \n!orospucocugu - orospu cocugunu görüntüler \n!orospucocugusec {isim} - orospu cocugunu seçer\n!sil {silinecekmesajsayisi} - girilen sayı kadar mesajı siler\n!ping - ..." + 
                        "\n!oylamabaslat {secenek1} {secenek2} {secenek3} {secenek4} - En fazla 4 secenekli bir oylama olusturur" + 
                        "\n!oyver {numara} - varolan bir oylamaya oy verir\n!oylamabitir - varolan oylamayi sonlandırır\n!ayril - bot kanaldan ayrılır ve çevrimdışı olur\n!yanimagel - Bot mesajı yazan kişinin bulunduğu sesli sohbet kanalına girer" + 
                        "\n!hesapla {sayı1} {işaret} {sayi2} - Yazılan matematik işleminin sonucunu söyler\n!yukle - Uyelerin bilgilerini okur ve sisteme yukler ardından üyelerin bilgilerini kanalda listeler" + 
                        "\n!para-ekle {hesap ismi} {para miktarı} {anahtar} - seçilen hesaba belirtilen miktarda para yükler\n!para-sil {hesap ismi} {para miktarı} {anahtar} - seçilen hesaptan belirtilen miktar kadar para siler\n kaydol {sifre} - Seçilen şifreyle bir hesap açarsınız" + 
                        "\n!girisyap {sifre} - Varolan bir hesaba giris yapar \n!bilgi {isim} varolan bir hesabın bilgilerini gösterir" + 
                        "\n!pm - Botla aranızda bir özel mesajlaşma sekmesi açılır\n!mute {isim} {anahtar} - girilen isime sahip kişinin konuşması yasaklanır\n!Coinflip komutlar - Coinflip komutlarini gösterir");
                  //  await e.Channel.SendMessage("bY MaMeR MeŞiLdAğ");


                });
            commands.CreateCommand("sil").Parameter("mesaj4",ParameterType.Required)
                .Do(async (e) =>
                {
                    Message[] silinecekmesajlar;
                    silinecekmesajlar = await e.Channel.DownloadMessages(Int32.Parse(e.GetArg("mesaj4")) + 1);
                    await e.Channel.DeleteMessages(silinecekmesajlar);
                    
                //   await e.Channel.SendMessage("Son " + silinecekmesajlar.Length.ToString() + " mesaj temizlendi.");
                    

                });
            commands.CreateCommand("girisyap").Parameter("sifre", ParameterType.Required)
               .Do(async (e) =>
               {
                  if(z == 0)
                   {
                       oyuncular = new Oyuncu[10];
                       z += 1;
                   }
                  
                  Writer _writer = new Writer();
                  _writer.Read(e.User.Name);
                   if(_writer.okunanyazi1 == e.GetArg("sifre"))
                   {
                       await e.Channel.SendMessage("Giriş Başarılı");
                       oyuncular[oyuncunumarasi].isim = e.GetArg("ad");
                       
                   }
                   else
                   {
                       await e.Channel.SendMessage("Giriş Başarısız");
                   //    await e.Channel.SendMessage("Olması gereken şifre: " + _writer.okunanyazi);
                   //    await e.Channel.SendMessage("Sizin Yazdığınız: " + e.GetArg("sifre"));
                   }
                   
               });
            commands.CreateCommand("kaydol").Parameter("yenisifre", ParameterType.Required)
               .Do(async (e) =>
               {
                   Writer _writer = new Writer();
                   _writer.Write(e.User.Name,e.GetArg("yenisifre"),0);
                   await e.Channel.SendMessage(e.User.Mention + " Kayıt oldunuz");
               });

            commands.CreateCommand("pm")
               .Do(async (e) =>
               {
                  await e.User.SendMessage("Burdan bütün komutları girebilirsiniz \n!yardim");
                   await e.Channel.SendMessage(e.User.Mention + " Özel mesaj isteği gönderildi. Özel mesajdan kayıt olabilir veya giriş yapabilirsiniz.");
               });
            commands.CreateCommand("para-ekle").Parameter("isim", ParameterType.Required).Parameter("para", ParameterType.Required).Parameter("Anahtar", ParameterType.Required)
               .Do(async (e) =>
               {
                   string girilenanahtar;
                   girilenanahtar = e.GetArg("Anahtar");
                   if(girilenanahtar == gercekanahtar)
                   {
                       int yuklenecekpara = Int32.Parse(e.GetArg("para"));
                       Writer _writer = new Writer();
                       _writer.paraekle(e.GetArg("isim"), yuklenecekpara);
                       _writer.Read(e.GetArg("isim"));
                       await e.Channel.SendMessage(e.GetArg("isim") + " Adlı hesaba " + yuklenecekpara.ToString() + " para yüklendi");
                   }
                   else
                   {
                       await e.Channel.SendMessage(e.User.Mention + " Yanlış anahtar");
                   }
               });
            commands.CreateCommand("bilgi").Parameter("isim", ParameterType.Required)
               .Do(async (e) =>
               {
                   Writer _writer = new Writer();
                   _writer.Read(e.GetArg("isim"));

                   await e.Channel.SendMessage(e.GetArg("isim") + "\nPara: " + _writer.okunanyazi2);
               });
            commands.CreateCommand("oyuncular")
               .Do(async (e) =>
               {
                   int b = 0;
                   DirectoryInfo d = new DirectoryInfo(@"D:\Visual studio projects\Kodumunbot\Kodumunbot\bin\Debug\userData");
                   foreach (var file in d.GetFiles("*.txt"))
                   {
                       b += 1;
                   }
                   await e.Channel.SendMessage("Sistemde kayıtlı " + b.ToString() + " oyuncu var");
                   b = 0;
               });
            commands.CreateCommand("yukle")
               .Do(async (e) =>
               {
                   await e.Channel.SendMessage("Oyuncuların kayıtları dinamik hafızaya yukleniyor. Bu oyuncu sayısına göre biraz zaman alabilir.");
                   Writer _writer = new Writer();
                   int l = 0;
                   int b = 0;
                   DirectoryInfo d = new DirectoryInfo(@"D:\Visual studio projects\Kodumunbot\Kodumunbot\bin\Debug\userData");
                   foreach (var file in d.GetFiles("*.txt"))
                   {
                       b += 1;
                   }
                   Console.WriteLine(b);
                   oyuncular = new Oyuncu[b];
                   FileInfo[] files = d.GetFiles("*.txt");
                   Console.WriteLine("Yukleniyor (1)");
                   for(int i = 0; i < oyuncular.Length; i++)
                   {
                       oyuncular[i] = new Oyuncu();
                   }
                  
                   foreach (FileInfo file in files)
                   {
                       string duzenlenmishali;
                       duzenlenmishali = file.Name.Substring(0, file.Name.Length - 4);
                       oyuncular[l].isim = duzenlenmishali;
                       _writer.Read(oyuncular[l].isim);
                       oyuncular[l].para = Int32.Parse(_writer.okunanyazi2);
                       l += 1;
                       Console.WriteLine(l.ToString());
                   }
                   Console.WriteLine("Yukleniyor (2)");
                   b = 0;
                   l = 0;
                   foreach(Oyuncu _oyuncu in oyuncular)
                   {
                       await e.Channel.SendMessage(_oyuncu.isim +" " +  _oyuncu.para.ToString());
                   }
                   await e.Channel.SendMessage("\nYükleme işlemi tamamlandı.");
               });

            commands.CreateCommand("listele")
               .Do(async (e) =>
               {
                   int k = 0;
                   foreach(Oyuncu _oyuncu in oyuncular)
                   {
                       await e.Channel.SendMessage(_oyuncu.isim);
                   }
                   
               });
            discord.ExecuteAndWait(async () =>
            {

                await discord.Connect("MzQzODk4ODY4MzU1OTU2NzM3.DGk4aA.x2ZYebb-gP1qgwCj57Aj6knvTZM", TokenType.Bot);
                

            });
            

        }
        private void log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        private async Task fonksiyon(CommandEventArgs e)
        {
           await e.Channel.SendMessage("bu bir fonksiyon");
        }
        public static async void TimerElapsedEvent(object sender, ElapsedEventArgs e)
        {

            i += 1;
            if(i >= limitstatik && temp == 0)
            {
             await yazilanchannel.SendMessage(limitstatik.ToString() + " saniye doldu!");
                gerisayimvar = false;
                temp = 1;
                
                
            }
            if (ruleticinsay)
            {
             
                
                if(temp2 == 0)
                {
                    Random rastgele = new Random();
                    int rastgeleint = rastgele.Next(6, 23);
                    ruletmesaj = await yazilanchannel.DownloadMessages(1);
                    Console.WriteLine(ruletmesaj[0].RawText);
                    i2 = i + rastgeleint;
                    temp2 = 1;
                    
                }
                
                if(i == i2)
                {
                    
                    tomp -= 1;
                    string cikansonuc = "yok";
                    if(tomp == -1)
                    {
                        cikansonuc = ":red_circle:";
                    }
                    if(tomp == 0)
                    {
                        cikansonuc = ":black_circle:";
                    }
                    if(tomp == 1)
                    {
                        cikansonuc = ":red_circle:";
                    }
                    if(tomp == 2)
                    {
                        cikansonuc = ":black_circle";

                    }
                    if(tomp == 3)
                    {
                        cikansonuc = ":red_circle:";
                    }
                    if(tomp == 4)
                    {
                        cikansonuc = ":black_circle:";
                    }
                    if(tomp == 5)
                    {
                        cikansonuc = ":red_circle:";
                    }
                    if(tomp == 6)
                    {
                        cikansonuc = ":black_circle:";
                    }
                    if(tomp == 7)
                    {
                        cikansonuc = ":red_circle:";
                    }
                    if(tomp == 8)
                    {
                        cikansonuc = ":green_heart:";
                    }
                    if(tomp == 9)
                    {
                        cikansonuc = ":black_circle:";
                    }
                    if(tomp == 10)
                    {
                        cikansonuc = ":red_circle:";
                    }
                    if(tomp == 11)
                    {
                        cikansonuc = ":black_circle:";
                    }
                    if(tomp == 12)
                    {
                        cikansonuc = ":red_circle:";
                    }
                    if(cikansonuc == ":black_circle:")
                    {
                        foreach(Ruletoyuncu _ruletoyuncu in ruletoyuncular)
                        {
                            if (_ruletoyuncu != null)
                            {
                                if (_ruletoyuncu.yatirdigirenk == ":black_circle:")
                                {
                                    await yazilanchannel.SendMessage(_ruletoyuncu.isim + " kazandınız");
                                }
                            }
                        }
                    }
                    if(cikansonuc == ":red_circle:")
                    {
                        foreach (Ruletoyuncu _ruletoyuncu in ruletoyuncular)
                        {
                            if (_ruletoyuncu != null)
                            {
                                if (_ruletoyuncu.yatirdigirenk == ":red_circle:")
                                {
                                    await yazilanchannel.SendMessage(_ruletoyuncu.isim + " kazandınız");
                                }
                            }
                        }
                    }
                    if(cikansonuc == ":green_heart:")
                    {
                        foreach (Ruletoyuncu _ruletoyuncu in ruletoyuncular)
                        {
                            if (_ruletoyuncu != null)
                            {
                                if (_ruletoyuncu.yatirdigirenk == ":green_heart:")
                                {
                                    await yazilanchannel.SendMessage(_ruletoyuncu.isim + " kazandınız");
                                }
                            }
                        }
                    }
                    await yazilanchannel.SendMessage("Çıkan sonuç: " + cikansonuc);
                //    await yazilanchannel.SendMessage("Tomp = " + tomp.ToString());
                    ruleticinsay = false;
                    temp2 = 0;
                    
                    return;
                }
                if (ruletmesaj[0].RawText != null)
                {
                    if (tomp == 0)
                    {
                        // ilk
                        await ruletmesaj[0].Edit(":black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 1)
                    {
                        await ruletmesaj[0].Edit(":red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 2)
                    {
                        await ruletmesaj[0].Edit(":black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 3)
                    {
                        await ruletmesaj[0].Edit(":red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 4)
                    {
                        await ruletmesaj[0].Edit(":black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 5)
                    {
                        await ruletmesaj[0].Edit(":red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 6)
                    {
                        await ruletmesaj[0].Edit(":black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 7)
                    {
                        await ruletmesaj[0].Edit(":red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :green_heart:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 8)
                    {
                        await ruletmesaj[0].Edit(":green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 9)
                    {
                        await ruletmesaj[0].Edit(":black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 10)
                    {
                        await ruletmesaj[0].Edit(":red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 11)
                    {
                        await ruletmesaj[0].Edit(":black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle:");
                        tomp += 1;
                        return;
                    }
                    if(tomp == 12)
                    {
                        await ruletmesaj[0].Edit(":red_circle: :black_circle: :red_circle: :black_circle: :green_heart: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle: :red_circle: :black_circle:");
                            tomp = 0;
                        
                        return;
                    }
                }
            }
            Console.WriteLine(i.ToString());
            Console.WriteLine("i2 " + i2.ToString());
            
        }
        public static void Timerbaslat(int interval, int limit)
        {
            i = 0;
            temp = 0;
            limitstatik = limit;
            
            zamanlayici.Interval = interval;
            zamanlayici.Enabled = true;
            if (a == 0)
            {
                zamanlayici.Elapsed += new ElapsedEventHandler(TimerElapsedEvent);
                a += 1;

            }
        }
        private async Task oyverildi(CommandEventArgs e)
        {
            var userRoles = e.Server.Roles;
            var rol = userRoles.Where(input => input.Name == "silenced").FirstOrDefault();
            await e.User.AddRoles(rol);
         //   await e.Channel.SendMessage("rolunuze atandiniz");
        }
        private async Task oybitti(CommandEventArgs e)
        {
            var userRoles = e.Server.Roles;
            var rol = userRoles.Where(input => input.Name == "silenced").FirstOrDefault();
            await e.User.RemoveRoles(rol);

         //   await e.Channel.SendMessage("rolunuzden alindiniz");
        }
        public async Task girisbasarili(CommandEventArgs e)
        {

        } 
     
    }
}
