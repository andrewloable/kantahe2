using Kantahe2Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kantahe2Library.Services
{
    public interface ISongService
    {
        Task<Song> GetCurrentSongAsync();
        Task<Song> GetNextSongAsync();
        Task<List<Song>> GetQueueListAsync();
        Task<List<Song>> GetSongsAsync();
        Task<PlayState> GetStatusAsync();
        Task<Song> NextSongAsync();
        Task<Song> PlaySongAsync();
        Task<Song> StopSongAsync();
        Task<List<Song>> UpdateSongListAsync();
    }
}