using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDetectors.Models;
using EDetectors.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EDetectors.Pages
{
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
    public class IndexModel : PageModel
    {
        private readonly IMapService _mapService;

        [BindProperty]
        public string TileName { get; set; }

        [BindProperty]
        public MapBoxResponse MapBoxResponse { get; set; }

        public S3Credentials Data { get; set; } = new S3Credentials();

        public IndexModel(IMapService mapService)
        {
            _mapService = mapService;
        }

        public async Task OnGetAsync()
        {
        }

        public async Task OnPostAsync(IFormFile formFile)
        {
            Data = await _mapService.GetS3Credentials();

            await _mapService.UploadFileToS3Async(Data, formFile);

            MapBoxResponse = await _mapService.UploadToMapBox(Data, $"{TileName}{DateTime.Now.Ticks}");
        }
    }
}