using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kantahe2Library.Models;
using Newtonsoft.Json;

namespace Kantahe2Library.Services
{
    public class SongService : ISongService
    {
        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// Get the list of songs
        /// </summary>
        /// <returns></returns>
        public async Task<List<Song>> GetSongsAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/song");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<Song[]>(json);
                return new List<Song>(retval);
            }
            return null;
        }
        /// <summary>
        /// Update song list then get the list of songs
        /// </summary>
        /// <returns></returns>
        public async Task<List<Song>> UpdateSongListAsync()
        {
            var response = await client.PostAsync($"{Constants.APIHostSetting}/api/song/reload", null);
            if (response.IsSuccessStatusCode)
            {
                response = await client.GetAsync("/api/song");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var retval = JsonConvert.DeserializeObject<Song[]>(json);
                    return new List<Song>(retval);
                }
            }
            return null;
        }
        /// <summary>
        /// Get a list of songs in the queue
        /// </summary>
        /// <returns></returns>
        public async Task<List<Song>> GetQueueListAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/queue");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<Song[]>(json);
                return new List<Song>(retval);
            }
            return null;
        }
        /// <summary>
        /// Get the current song info
        /// </summary>
        /// <returns></returns>
        public async Task<Song> GetCurrentSongAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/queue/current-song");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<Song>(json);
                return retval;
            }
            return null;
        }
        /// <summary>
        /// Get the next song info
        /// </summary>
        /// <returns></returns>
        public async Task<Song> GetNextSongAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/queue/next-song");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<Song>(json);
                return retval;
            }
            return null;
        }
        /// <summary>
        /// Get player status
        /// </summary>
        /// <returns></returns>
        public async Task<PlayState> GetStatusAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/queue/status");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<PlayState>(json);
                return retval;
            }
            return PlayState.None;
        }
        /// <summary>
        /// Start playing current song
        /// </summary>
        /// <returns></returns>
        public async Task<Song> PlaySongAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/queue/play");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<Song>(json);
                return retval;
            }
            return null;
        }
        /// <summary>
        /// Stop playing current song
        /// </summary>
        /// <returns></returns>
        public async Task<Song> StopSongAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/queue/stop");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<Song>(json);
                return retval;
            }
            return null;
        }
        /// <summary>
        /// Go to next song
        /// </summary>
        /// <returns></returns>
        public async Task<Song> NextSongAsync()
        {
            var response = await client.GetAsync($"{Constants.APIHostSetting}/api/queue/next");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var retval = JsonConvert.DeserializeObject<Song>(json);
                return retval;
            }
            return null;
        }
    }
}
