using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kantahe2Library.Models
{
    public class Song
    {
#nullable enable
        [JsonPropertyName("ID")]
        public string? ID { get; set; }
        [JsonPropertyName("FileName")]
        public string? FileName { get; set; }
        [JsonPropertyName("Title")]
        public string? Title { get; set; }
        [JsonPropertyName("Artist")]
        public string? Artist { get; set; }
    }
}
