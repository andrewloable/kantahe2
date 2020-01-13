using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantahe2API.Models
{
    public class AppState
    {        
        public static IEnumerable<Song> Songs { get; set; }
        public static IEnumerable<Song> Queue { get; set; }
        public static PlayState Status { get; set; }
        public static Song CurrentSong { get; set; }
        public static Song NextSong { get; set; }
        public static bool IsPlayingRandom { get; set; }
        public static Random RNG = new Random();
    }
    public enum PlayState
    {
        Stopped,
        Playing
    }
}
