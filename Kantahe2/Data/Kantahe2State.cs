using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
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
        public static string songListPath = "/home/kantahe/kantahe2/videoke/data.json";
        public static string songFolder = string.Empty;
        public delegate void Kantahe2EventHandler(object sender, Kantahe2EventArgs e);
        public event Kantahe2EventHandler OnUpdateStatus;
        private static Queue<Song> Queue = new Queue<Song>();
        private Song CurrentSong;
        public bool isPlaying = false;

        public Kantahe2State()
        {
            try
            {   
                songFolder = Path.GetDirectoryName(songListPath);
                var songlistfile = File.ReadAllText(songListPath);
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
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.StackTrace}");
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

        public async Task SongStream(Song song, Stream outputstream)
        {
            var path = Path.Combine(songFolder, song.FileName);
            
            try
            {
                var buffer = new byte[65536];
                using (var video = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;
                    while(length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputstream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch
            {
                return;
            }
            finally
            {
                outputstream.Close();
            }
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

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork 
                    && !ip.ToString().Contains("127") 
                    && !ip.ToString().Contains("local"))
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
