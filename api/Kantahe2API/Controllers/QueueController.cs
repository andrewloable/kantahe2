using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Kantahe2API.Models;
using Kantahe2Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Kantahe2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private IConfiguration _configuration { get; }
        public QueueController(IConfiguration configuration)
        {
            _configuration = configuration;
            var jsonFilename = _configuration["Playlist"];
            var json = System.IO.File.ReadAllText(jsonFilename);
            var folder = Path.GetDirectoryName(jsonFilename);
            var list = JsonSerializer.Deserialize<Song[]>(json);
            var songs = new List<Song>();
            foreach (var o in list)
            {
                var exist = songs.FirstOrDefault(r => r.ID == o.ID);
                if (exist == null)
                {
                    var fn = Path.Combine(folder, o.FileName);
                    if (System.IO.File.Exists(fn))
                    {
                        songs.Add(new Song()
                        {
                            Artist = o.Artist.ToString().Trim().ToUpper(),
                            Title = o.Title.ToString().Trim().ToUpper(),
                            ID = o.ID.ToString(),
                            FileName = o.FileName.ToString()
                        });
                    }
                }
            }
            AppState.Songs = songs;
        }
        /// <summary>
        /// GET: api/queue
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Song>> Get()
        {
            return Ok(AppState.Queue);
        }
        /// <summary>
        /// GET: api/queue/current-song
        /// </summary>
        /// <returns></returns>
        [HttpGet("current-song")]
        public ActionResult<Song> GetCurrentSong()
        {
            return Ok(AppState.CurrentSong);
        }
        /// <summary>
        /// GET: api/queue/next-song
        /// </summary>
        /// <returns></returns>
        [HttpGet("next-song")]
        public ActionResult<Song> GetNextSong()
        {
            return Ok(AppState.NextSong);
        }
        /// <summary>
        /// GET: api/queue/status
        /// </summary>
        /// <returns></returns>
        [HttpGet("status")]
        public ActionResult<PlayState> GetStatus()
        {
            return Ok(AppState.Status);
        }
        /// <summary>
        /// POST: api/queue/play
        /// </summary>
        /// <returns></returns>
        [HttpPost("play")]
        public ActionResult<Song> Play()
        {
            if (AppState.Status == PlayState.Stopped)
            {
                AppState.Status = PlayState.Playing;
                if (AppState.CurrentSong == null)
                {
                    var queue = (Queue<Song>)AppState.Queue;
                    if (queue != null && queue.Count > 0)
                    {
                        AppState.CurrentSong = queue.Dequeue();
                        AppState.NextSong = queue.Peek();
                        return Ok(AppState.CurrentSong);
                    }
                    if (AppState.Songs.Count() > 0)
                    {
                        var idx = AppState.RNG.Next(0, AppState.Songs.Count());
                        AppState.CurrentSong = AppState.Songs.ElementAt(idx);
                        AppState.NextSong = null;
                        return Ok(AppState.CurrentSong);
                    }
                }
            }
            return Ok(AppState.CurrentSong);
        }
        /// <summary>
        /// POST: api/queue/stop
        /// </summary>
        /// <returns></returns>
        [HttpPost("stop")]
        public ActionResult<Song> Stop()
        {
            if (AppState.Status == PlayState.Playing)
            {
                AppState.Status = PlayState.Stopped;
            }
            return Ok(AppState.CurrentSong);
        }
        /// <summary>
        /// POST: api/queue/next
        /// </summary>
        /// <returns></returns>
        [HttpPost("next")]
        public ActionResult<Song> Next()
        {
            if (AppState.Queue == null)
            {
                AppState.Queue = new Queue<Song>();
            }
            var queue = (Queue<Song>)AppState.Queue;
            AppState.Status = PlayState.Stopped;
            if (queue.Count > 0)
            {
                AppState.CurrentSong = queue.Dequeue();
                if (queue.Count > 0)
                {
                    AppState.NextSong = queue.Peek();
                } else
                {
                    AppState.NextSong = null;
                }                
                AppState.Status = PlayState.Playing;
                return Ok(AppState.CurrentSong);
            }
            if (AppState.Songs.Count() > 0)
            {
                var idx = AppState.RNG.Next(0, AppState.Songs.Count());
                AppState.CurrentSong = AppState.Songs.ElementAt(idx);
                AppState.NextSong = null;
                AppState.Status = PlayState.Playing;
                return Ok(AppState.CurrentSong);
            }
            return NotFound();
        }
        /// <summary>
        /// POST: api/queue
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Song> AddQueue(Song song)
        {
            if (AppState.Queue == null)
            {
                AppState.Queue = new Queue<Song>();
            }
            var queue = (Queue<Song>)AppState.Queue;
            queue.Enqueue(song);
            return Ok(song);
        }
        [HttpPut]
        public ActionResult AddArtistToQueue([FromQuery] string artist)
        {
            if (AppState.Queue == null)
            {
                AppState.Queue = new Queue<Song>();
            }
            var queue = (Queue<Song>)AppState.Queue;

            var artistSongs = AppState.Songs.Where(r => r.Artist.Contains(artist, StringComparison.OrdinalIgnoreCase));
            if (artistSongs != null && artistSongs.Count() > 0)
            {
                foreach(var song in artistSongs)
                {
                    queue.Enqueue(song);
                }
            }
            return Ok();
        }
    }
}