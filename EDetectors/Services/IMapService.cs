using EDetectors.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDetectors.Services
{
    public interface IMapService
    {
        Task<S3Credentials> GetS3Credentials();

        Task UploadFileToS3Async(S3Credentials sc, IFormFile f);

        Task<MapBoxResponse> UploadToMapBox(S3Credentials sc, string tile_set_name);
    }
}