using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dex.Web.Pages.Downloads
{
    public class FluentGenerationModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public FluentGenerationModel(IWebHostEnvironment env)
        {
            _environment = env;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetDownloadExecutableAsync(string zipName)
        {
            var path = _environment.WebRootPath + @"\zips\";
            var zipFileName = $"{zipName}.zip";
            var rarFileName = $"{zipName}.rar";
            var fileName = path + zipFileName;
            if (!System.IO.File.Exists(fileName))
            {
                fileName = rarFileName;
            }
            var fileBytes = await System.IO.File.ReadAllBytesAsync(path + fileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}