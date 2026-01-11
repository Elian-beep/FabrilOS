using FabrilOS.API.Services.Interfaces;
using Minio;
using Minio.DataModel.Args;

namespace FabrilOS.API.Services.Implementations;

public class MinioStorageService : IFileStorageService
{
  private readonly IMinioClient _minioClient;
  private readonly string _bucketName;

  public MinioStorageService(IConfiguration configuration)
  {
    var minioConfig = configuration.GetSection("Minio");
    _bucketName = minioConfig["BucketName"] ?? "service-order-images";

    _minioClient = new MinioClient()
      .WithEndpoint(minioConfig["Endpoint"])
      .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"])
      .WithSSL(Convert.ToBoolean(minioConfig["UseSSL"]))
      .Build();
  }

  public async Task<string> UploadFileAsync(IFormFile file)
  {
    var foundArgs = new BucketExistsArgs().WithBucket(_bucketName);
    if (!await _minioClient.BucketExistsAsync(foundArgs))
    {
      await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
    }

    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

    using var stream = file.OpenReadStream();
    var putObjectArgs = new PutObjectArgs()
      .WithBucket(_bucketName)
      .WithObject(fileName)
      .WithStreamData(stream)
      .WithObjectSize(stream.Length)
      .WithContentType(file.ContentType);

    await _minioClient.PutObjectAsync(putObjectArgs);

    return fileName;
  }

  public async Task<string> GetPresignedUrlAsync(string fileName)
  {
    var args = new PresignedGetObjectArgs()
      .WithBucket(_bucketName)
      .WithObject(fileName)
      .WithExpiry(60 * 60 * 24);

    return await _minioClient.PresignedGetObjectAsync(args);
  }
}