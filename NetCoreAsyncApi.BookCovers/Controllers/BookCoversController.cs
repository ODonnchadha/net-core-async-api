namespace NetCoreAsyncApi.BookCovers.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController()]
    [Route("api/bookcovers")]
    public class BookCoversController : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> GetBookCover(string name, bool returnFault = false)
        {
            if (returnFault)
            {
                await Task.Delay(500);
                return new StatusCodeResult(500);
            }

            var random = new Random();
            var coverBytes = random.Next(8800, 888000);
            var cover = new byte[coverBytes];
            random.NextBytes(cover);

            return Ok(new { Name = name, Content = cover });
        }
    }
}
