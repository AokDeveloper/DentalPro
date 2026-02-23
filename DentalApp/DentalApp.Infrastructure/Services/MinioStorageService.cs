using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using DentalApp.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Infrastructure.Services
{
    public class MinioStorageService : IFileStorageService
    {
        private readonly AmazonS3Client _s3Client;
        private readonly string _bucketName;

        public MinioStorageService(IConfiguration configuration)
        {
            var serviceUrl = configuration["Minio:ServiceUrl"];
            var accessKey = configuration["Minio:AccessKey"];
            var secretKey = configuration["Minio:SecretKey"];
            _bucketName = configuration["Minio:BucketName"];

            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true // <-- MİNİO İÇİN BU AYAR ÇOK KRİTİK!
            };

            _s3Client = new AmazonS3Client(accessKey, secretKey, config);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
        {
            // --- YENİ EKLENEN KISIM: OTO-OLUŞTURMA ---
            // 1. Kova var mı kontrol et
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);

            if (!bucketExists)
            {
                // 2. Yoksa oluştur
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = _bucketName,
                    UseClientRegion = true
                };
                await _s3Client.PutBucketAsync(putBucketRequest);
            }
            // -------------------------------------------

            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(putRequest);

            return $"{_s3Client.Config.ServiceURL}/{_bucketName}/{fileName}";
        }
    }
}