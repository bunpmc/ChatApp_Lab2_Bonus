using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ChatApp_With_RazorSignalR.Pages
{
    [IgnoreAntiforgeryToken] // For simpler upload via fetch without token handling complexity, or we handle token in JS. Let's use Token in JS.
    [RequestSizeLimit(2147483648)] // 2GB
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            var uploads = new List<object>();
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    uploads.Add(new { 
                        url = $"/uploads/{fileName}", 
                        name = file.FileName,
                        type = file.ContentType,
                        size = file.Length
                    });
                }
            }
            return new JsonResult(uploads);
        }
    }
}
