using System;

namespace Kantahe2.Data
{
    public class Kantahe2EventArgs: EventArgs
    {
        public string Status { get; set; }
        public Song Song { get; set; }
        public string URL { get; set; }
    }
}
