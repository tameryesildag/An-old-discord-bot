﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json;
namespace SmileBot
{
    public class league
    {
        public string summonerid;
        public string summonerid2;
        public string summonername;
        public string summonerlevel;
        public string macbuloutput;
        public string macinfooutput;
        public string mac1id;
        public string mac2id;
        public string mac3id;
        public string mac4id;
        public string mac5id;
        public string apikey = "RGAPI-5ba3ea1d-6f74-4961-861b-818a8109a926";
        public string macoyunmodu;
        public string macsure;
        public string macsonuc;
        public string kill = "";
        public string death = "";
        public string assist = "";

        bool bitti = false;
        int data2count = 0;
        int data5count = 0;
        public void summonerbul(string isim) {
            //https://tr1.api.riotgames.com/lol/summoner/v3/summoners/by-name/gg%20izi?api_key=RGAPI-386f0485-8fb0-4440-ad18-0e7986b6d51b
            string url = "https://tr1.api.riotgames.com/lol/summoner/v3/summoners/by-name/" + isim + "?api_key=" + apikey;
            var jss = new JavaScriptSerializer();
            var jsunn = new WebClient().DownloadString(url);
            JsonConvert.DeserializeObject(jsunn);
            dynamic dynObj = JsonConvert.DeserializeObject(jsunn);
            foreach(var data in dynObj)
            {
                foreach(var data2 in data)
                {
                    data2count += 1;
                    if(data2count == 1)
                    {
                        Console.WriteLine("Hesap ID2: " + Convert.ToString(data2));
                        summonerid2 = Convert.ToString(data2);
                    }
                    if(data2count == 2)
                    {
                        Console.WriteLine("Hesap ID: " + Convert.ToString(data2));
                        summonerid = Convert.ToString(data2);
                    }                    
                    if(data2count == 3)
                    {
                        Console.WriteLine("Hesap adı: " + Convert.ToString(data2));
                        summonername = Convert.ToString(data2);
                    }
                    if(data2count == 6)
                    {
                        Console.WriteLine("Hesap leveli: " + Convert.ToString(data2));
                        summonerlevel = Convert.ToString(data2);
                    }
                }
            }
            macbul(summonerid);

           // summoneroutput = jsunn;
           // Console.WriteLine(jsunn);

        }
        public void macbul(string id)
        {
            sa:
            Console.WriteLine("girilen id: " + id);
            //https://tr1.api.riotgames.com/lol/match/v3/matchlists/by-account/208290931/recent?api_key=RGAPI-386f0485-8fb0-4440-ad18-0e7986b6d51b
            string url = "https://tr1.api.riotgames.com/lol/match/v3/matchlists/by-account/" + id + "/recent?api_key=" + apikey;
            var jss = new JavaScriptSerializer();
            var jsunn = new WebClient().DownloadString(url);
            dynamic dynObj = JsonConvert.DeserializeObject(jsunn);
            if (bitti == false)
            {
                foreach (var data in dynObj)
                {
                    //  Console.WriteLine(data);
                    foreach (var data2 in data) //2. al
                    {
                        //    Console.WriteLine(data2);
                        foreach (var data3 in data2)
                        {
                            //   Console.WriteLine(data3);
                            foreach (var data4 in data3)
                            {
                                foreach (var data5 in data4)
                                {
                                    data5count += 1;
                                    if (data5count == 2)
                                    {
                                        Console.WriteLine(data5);
                                        mac1id = Convert.ToString(data5);

                                        //   Console.WriteLine("deneme" + mac1id);
                                    }
                                    if (data5count == 10)
                                    {
                                        Console.WriteLine(data5);
                                        mac2id = Convert.ToString(data5);

                                    }
                                    if (data5count == 18)
                                    {
                                        Console.WriteLine(data5);
                                        mac3id = Convert.ToString(data5);

                                    }
                                    if (data5count == 26)
                                    {
                                        Console.WriteLine(data5);
                                        mac4id = Convert.ToString(data5);

                                    }
                                    if (data5count == 34)
                                    {
                                        Console.WriteLine(data5);
                                        mac5id = Convert.ToString(data5);
                                        bitti = true;
                                        goto sa;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            
            //  macbuloutput = jsunn.Substring(32,20);
            //  Console.WriteLine(jsunn);
        }
        public void macbilgi(string uid, string macid)
        {
            //            https://tr1.api.riotgames.com/lol/match/v3/matches/595147021?forAccountId=201950307&api_key=RGAPI-5ba3ea1d-6f74-4961-861b-818a8109a926
            string url = "https://tr1.api.riotgames.com/lol/match/v3/matches/" + macid + "?forAccountId=" + uid + "&api_key=" + apikey;
            string parid = "";
            string teamid = "";
            Console.WriteLine(url);
            var jss = new JavaScriptSerializer();
            var jsunn = new WebClient().DownloadString(url);
            dynamic dynObj = JsonConvert.DeserializeObject(jsunn);
            macoyunmodu = Convert.ToString(dynObj.gameMode);
            macsure = Convert.ToString(dynObj.gameDuration);
            Console.WriteLine(dynObj.gameMode);
            Console.WriteLine(dynObj.gameDuration);
            foreach(var veri in dynObj.participantIdentities)
            {
                if(veri.player != null)
                {
                    Console.WriteLine(veri.participantId);
                    parid = veri.participantId;
                }
            } 
            foreach(var veri in dynObj.participants)
            {
                if(veri.participantId == parid)
                {
                    Console.WriteLine(veri.teamId);
                    teamid = veri.teamId;
                    kill = veri.stats.kills;
                    death = veri.stats.deaths;
                    assist = veri.stats.assists;
                }
            }
            foreach(var veri in dynObj.teams)
            {
                if (veri.teamId == teamid)
                {
                    Console.WriteLine(veri.win);
                    if(veri.win == "Fail")
                    {
                        macsonuc = "Bozgun";
                    }
                    if(veri.win == "Win")
                    {
                        macsonuc = "Zafer";
                    }
                }
            }

            
        }
        public void rankbul(string id)
        {

        }
    }
}
