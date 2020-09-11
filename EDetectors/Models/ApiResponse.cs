using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDetectors.Models
{
    public class S3Credentials
    {
        [JsonPropertyName("bucket")]
        public string Bucket { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("accessKeyId")]
        public string AccessKeyId { get; set; }

        [JsonPropertyName("secretAccessKey")]
        public string SecretAccessKey { get; set; }

        [JsonPropertyName("sessionToken")]
        public string SessionToken { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class MapBoxResponse
    {
        [JsonPropertyName("complete")]
        public bool Complete { get; set; }

        [JsonPropertyName("tileset")]
        public string Tileset { get; set; }

        [JsonPropertyName("error")]
        public object Error { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("modified")]
        public string Modified { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("progress")]
        public int Progress { get; set; }
    }
}