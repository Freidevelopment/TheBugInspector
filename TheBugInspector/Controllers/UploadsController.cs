using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using TheBugInspector.Data;
using TheBugInspector.Models;

namespace TheBugInspector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController(ApplicationDbContext context) : ControllerBase
    {



        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            FileUpload? image = await context.Images.FirstOrDefaultAsync(i => i.Id == id);

            return image == null ? NotFound() : File(image.Data!, image.Type!);
        }

    }
}
