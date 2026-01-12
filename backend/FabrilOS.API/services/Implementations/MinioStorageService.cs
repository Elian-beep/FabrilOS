using FabrilOS.API.Services.Interfaces;
using Minio;
using Minio.DataModel.Args;

namespace FabrilOS.API.Services.Implementations;

public class MinioStorageService : IFileStorageService
{
  private readonly IMinioClient _minioClient;
  private readonly IMinioClient _internalClient;
  private readonly string _bucketName;
  private readonly string _accessKey;
  private readonly string _secretKey;
  private readonly bool _useSSL;

  public MinioStorageService(IConfiguration configuration)
  {
    var minioConfig = configuration.GetSection("Minio");
    _bucketName = minioConfig["BucketName"] ?? "service-order-images";

    _accessKey = minioConfig["AccessKey"];
    _secretKey = minioConfig["SecretKey"];
    _useSSL = Convert.ToBoolean(minioConfig["UseSSL"]);

    _internalClient = new MinioClient()
      .WithEndpoint(minioConfig["Endpoint"])
      .WithCredentials(_accessKey, _secretKey)
      .WithSSL(_useSSL)
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

    await _internalClient.PutObjectAsync(putObjectArgs);
    return fileName;
  }

  public async Task<string> GetPresignedUrlAsync(string fileName)
  {
    var externalClient = new MinioClient()
      .WithEndpoint("localhost", 9000)
      .WithCredentials(_accessKey, _secretKey)
      .WithSSL(_useSSL)
      .Build();

    var args = new PresignedGetObjectArgs()
      .WithBucket(_bucketName)
      .WithObject(fileName)
      .WithExpiry(60 * 60 * 24);

    return await externalClient.PresignedGetObjectAsync(args);
  }
}