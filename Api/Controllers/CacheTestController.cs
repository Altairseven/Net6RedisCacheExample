using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CacheTestController : ControllerBase {

    private readonly ICacheService _cache;

    public CacheTestController(ICacheService cache) {
        _cache = cache;
    }

    const string CACHE_KEY = "GRUPO_CACHE";


    [HttpGet]
    [Route("test1")]
    public async Task<IActionResult> GetCache1() {
        var subkey = $"{nameof(GetCache1)}";
        var list = await _cache.GetAsync<List<string>>(CACHE_KEY, subkey);
        if (list == null) {
            list = new List<string>();
            for (long i = 0; i < 1_000; i++) {
                list.Add("asdasdasd" + i);
                Console.WriteLine(i);
            }
            await _cache.WriteAsync(CACHE_KEY, subkey, list);
        
        }
        return Ok(list);
    }

    [HttpGet]
    [Route("test2/{clientId}")]
    public async Task<IActionResult> GetCache2(long clientId) {
        var subkey = $"{nameof(GetCache2)}-clientId";
        var str = await _cache.GetAsync<string>(CACHE_KEY, subkey);
        if (str == null) {
            str = "test " + clientId;
            await _cache.WriteAsync(CACHE_KEY, subkey, str);
        }
        return Ok(str);
    }

    [HttpGet]
    [Route("clearAll")]
    public async Task<IActionResult> ClearAll() {
        await _cache.RemoveFromCacheAsync(CACHE_KEY); //optional: cachesubkey to delete anything in particular.
        return Ok();
    }
}
