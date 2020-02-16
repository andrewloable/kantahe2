using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kantahe2API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Kantahe2Library.Models;

namespace Kantahe2API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private IConfiguration _configuration { get; }
        public SongController(IConfiguration configuration)
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
        /// GET: api/song
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<Song>> Get()
        {            
            return Ok(AppState.Songs);
        }
        /// <summary>
        /// POST: api/song/reload
        /// </summary>
        /// <returns></returns>
        [HttpPost("reload")]
        public ActionResult Reload()
        {
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
            return new OkResult();
        }
    }
}
