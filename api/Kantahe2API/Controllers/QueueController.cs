using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantahe2API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kantahe2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
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
            var queue = (Queue<Song>)AppState.Queue;
            AppState.Status = PlayState.Stopped;
            if (queue.Count > 0)
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
            return NotFound();
        }
    }
}