using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;   

namespace RaceDay.API
{
    public class BlobService
    {
        private readonly string _connectionString;
        public BlobService(IConfiguration config)
        {
            _connectionString = config["AzureBlobStorage"];
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("event-banners");
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
            await blobClient.UploadAsync(file.OpenReadStream());

            return blobClient.Uri.ToString(); // Returns the public URL
        }
    }
}

