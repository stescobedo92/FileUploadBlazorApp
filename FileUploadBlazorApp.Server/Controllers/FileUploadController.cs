using Microsoft.AspNetCore.Mvc;

namespace FileUploadBlazorApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploadController(IWebHostEnvironment _webHostEnvironment) : ControllerBase
    {
        [HttpPost("uploads")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile fileToUpload)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(fileToUpload.FileName)}";
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

            if(!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await fileToUpload.CopyToAsync(fileStream);

            return Ok($"{Request.Scheme}://{Request.Host}/Images/{fileName}");
        }
    }
}
