using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantahe2API.Models;
using Kantahe2Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kantahe2API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Song>> Get()
        {            
            return Ok(AppState.Songs);
        }
    }
}
