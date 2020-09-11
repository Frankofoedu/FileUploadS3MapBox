using EDetectors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using Microsoft.AspNetCore.Http;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System.Text;

namespace EDetectors.Services
{
    public class MapService : IMapService
    {
        private const string ACCESS_TOKEN = "sk.eyJ1IjoiZ2Vvc21hcnQiLCJhIjoiY2tleHlydWtyMDV1bjJ6cGh0OHR5MWJvZiJ9.6CsLA0F0xiSTNygsPYKcOA";
        private const string MAPBOXUSERNAME = "geosmart";
        private const string BASEURL = "https://api.mapbox.com/uploads/v1/";
        private readonly IHttpClientFactory _clientFactory;

        public MapService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<S3Credentials> GetS3Credentials()
        {
            var client = _clientFactory.CreateClient();

            var resposne = await client.PostAsync($"{BASEURL}{MAPBOXUSERNAME}/credentials?access_token={ACCESS_TOKEN}", null);

            if (resposne.IsSuccessStatusCode)
            {
                using var data = await resposne.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<S3Credentials>(data);
            }
            else
            {
                return null;
            }
        }

        public async Task UploadFileToS3Async(S3Credentials sc, IFormFile f)
        {
            RegionEndpoint bucketRegion = RegionEndpoint.USEast1;

            var sessionCredentials =
               new SessionAWSCredentials(sc.AccessKeyId,
                                         sc.SecretAccessKey,
                                         sc.SessionToken);

            IAmazonS3 s3Client = new AmazonS3Client(sessionCredentials, bucketRegion);

            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(f.OpenReadStream(),
                                              sc.Bucket, sc.Key);

                Console.WriteLine("Upload 3 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public async Task<MapBoxResponse> UploadToMapBox(S3Credentials sc, string tile_set_name)
        {
            var client = _clientFactory.CreateClient();

            var dt = new SendData { url = $"{sc.Url}", tileset = $"{MAPBOXUSERNAME}.{tile_set_name}" };

            var resposne = await client.PostAsync($"https://api.mapbox.com/uploads/v1/geosmart?access_token=sk.eyJ1IjoiZ2Vvc21hcnQiLCJhIjoiY2tleHlydWtyMDV1bjJ6cGh0OHR5MWJvZiJ9.6CsLA0F0xiSTNygsPYKcOA", new StringContent(JsonSerializer.Serialize(dt), Encoding.UTF8, "application/json"));

            if (resposne.IsSuccessStatusCode)
            {
                using var stream = await resposne.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<MapBoxResponse>(stream);

                return data;
            }
            else
            {
                return null;
            }
        }
    }

    public class SendData
    {
        public string url { get; set; }
        public string tileset { get; set; }
    }
}