using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kantahe2Library.Models;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Kantahe2Library.Services
{
    public class SongService : ISongService
    {
        private readonly HttpClient client;
        private readonly IConfiguration configuration;
        public SongService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            client = httpClientFactory.CreateClient("kantahe");
            this.configuration = configuration;
        }
        public int GetRefreshWait()
        {
            return int.Parse(configuration["RefreshMilliSeconds"]);
        }
        /// <summary>
        /// Get the list of songs
        /// </summary>
        /// <returns></returns>
        public async Task<List<Song>> GetSongsAsync()
        {
            var response = await client.GetAsync("/api/song");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song[]>(json);
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
            var response = await client.PostAsync("/api/song/reload", null);
            if (response.IsSuccessStatusCode)
            {
                response = await client.GetAsync("/api/song");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(json))
                    {
                        return null;
                    }
                    var retval = JsonSerializer.Deserialize<Song[]>(json);
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
            var response = await client.GetAsync("/api/queue");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song[]>(json);
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
            var response = await client.GetAsync("/api/queue/current-song");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song>(json);
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
            var response = await client.GetAsync("/api/queue/next-song");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song>(json);
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
            var response = await client.GetAsync("/api/queue/status");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return PlayState.None;
                }
                var retval = JsonSerializer.Deserialize<PlayState>(json);
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
            var response = await client.PostAsync("/api/queue/play", null);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song>(json);
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
            var response = await client.PostAsync("/api/queue/stop", null);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song>(json);
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
            var response = await client.PostAsync("/api/queue/next", null);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song>(json);
                return retval;
            }
            return null;
        }
        /// <summary>
        /// Add song to queue
        /// </summary>
        /// <param name="song"></param>
        /// <returns></returns>
        public async Task<Song> AddQueue(Song song)
        {
            var payload = new StringContent(JsonSerializer.Serialize(song), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/queue", payload);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }
                var retval = JsonSerializer.Deserialize<Song>(json);
                return retval;
            }
            return null;
        }
        /// <summary>
        /// Add artist songs to queue
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        public async Task AddArtistsToQueue(string artist)
        {
            var response = await client.PutAsync($"/api/queue?artist={artist}", null);            
            return;
        }
    }
}
