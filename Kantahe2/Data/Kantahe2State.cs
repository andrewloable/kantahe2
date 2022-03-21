using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Kantahe2.Data
{
    public class Kantahe2State
    {
        private static List<Song> SongList = new List<Song>();
        private string filelistPath = "songlistpath.config";
        private string songFolder = string.Empty;
        public delegate void Kantahe2EventHandler(object sender, Kantahe2EventArgs e);
        public event Kantahe2EventHandler OnUpdateStatus;
        private static Queue<Song> Queue = new Queue<Song>();
        private Song CurrentSong;
        public bool isPlaying = false;

        public Kantahe2State()
        {
            try
            {
                var songlistpath = File.ReadAllText(filelistPath);
                songFolder = Path.GetDirectoryName(songlistpath);
                var songlistfile = File.ReadAllText(songlistpath);
                var songs = JArray.Parse(songlistfile);
                SongList.Clear();
                foreach (var song in songs)
                {
                    var s = song.ToObject<Song>();
                    if (File.Exists(Path.Combine(songFolder, s.FileName)))
                    {
                        SongList.Add(song.ToObject<Song>());
                    }
                }
            }
            catch
            {
            }
        }
        public void AddQueue(Song song)
        {
            Queue.Enqueue(song);
        }
        public Queue<Song> GetSongQueue()
        {
            return Queue;
        }
        public List<Song> GetSongList()
        {
            return SongList;
        }
        public Song GetCurrentSong()
        {
            return CurrentSong;
        }
        public Song GetNextSong()
        {
            if (Queue.Count > 0)
            {
                return Queue.Dequeue();
            }

            var random = new Random();
            CurrentSong = Enumerable.Repeat(SongList, 1).Select(r => r[random.Next(SongList.Count)]).First();
            return CurrentSong;
        }
        public string SongBase64(Song song)
        {
            var bytes = File.ReadAllBytes(Path.Combine(songFolder, song.FileName));
            return Convert.ToBase64String(bytes);
        }
        public byte[] SongBytes(Song song)
        {
            return File.ReadAllBytes(Path.Combine(songFolder, song.FileName));
        }

        public void UserIsConnected()
        {
            EmitEvent(new Kantahe2EventArgs
            {
                Status = "UserConnected",
                URL = "/player"
            });
        }

        public void EmitEvent(Kantahe2EventArgs args)
        {
            if (OnUpdateStatus == null) return;

            OnUpdateStatus(this, args);
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "not found";
        }

        public string GenerateQRCode(string data)
        {
            var qrgen = new QRCodeGenerator();
            var qrcodedata = qrgen.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qr = new QRCode(qrcodedata);
            var qrbmp = qr.GetGraphic(20);
            var ms = new MemoryStream();
            qrbmp.Save(ms, ImageFormat.Jpeg);
            var img = ms.ToArray();
            return Convert.ToBase64String(img);
        }
    }
}
