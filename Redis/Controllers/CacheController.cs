using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Redis.Model;
using Redis.Services;

namespace Redis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetCacheValue([FromRoute] string key)
        {
            var value = await _cacheService.GetCacheValueAsync(key);
            return string.IsNullOrEmpty(value) ? (IActionResult)NotFound() : Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> SetCacheValue([FromBody] JsonElement body)
        {
            string json = System.Text.Json.JsonSerializer.Serialize(body);
            var data = System.Text.Json.JsonSerializer.Deserialize<DataTest>(json);
            await _cacheService.SetCacheValueAsync(data.key, data.value);
            return Ok();
        }

    }
}
