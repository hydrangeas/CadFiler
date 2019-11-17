using CadFile.Domain.Repositories;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CadFiler.Infrastructure.Azure.BlobStorage
{
    public class CadFileStorage : ICadFileStorageRepository
    {
        public string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (_connectionString == string.Empty)
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
#if DEBUG
                        .AddJsonFile(@"applicationsettings.debug.json")
#else
                        // NEED: Create following file,
                        //       and set "copy output directory: Always"
                        .AddJsonFile(@"applicationsettings.json")
#endif
                        .Build();
                    _connectionString = configuration.GetConnectionString("Azure.BlobStorage");
                }
                return _connectionString;
            }
        }

        readonly string ContainerName = "cadfiles";

        public Task Download(string savePath, Guid guid)
        {
            return Task.Run(async () =>
            {
                var cloudStorageAcccount = CloudStorageAccount.Parse(ConnectionString);
                var cloudBlobClient = cloudStorageAcccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(guid.ToString());
                await cloudBlockBlob.DownloadToFileAsync(savePath, System.IO.FileMode.CreateNew);
            });
        }

        /// <summary>
        /// Azure Blob Storage へファイルアップロードを行う
        /// </summary>
        /// <param name="fileInfo">ファイル情報</param>
        /// <param name="guid">アップロード用ファイル名</param>
        /// <returns>なし</returns>
        public Task Upload(IFileInfo fileInfo, Guid guid)
        {
            return Task.Run(async () =>
            {
                var cloudStorageAcccount = CloudStorageAccount.Parse(ConnectionString);
                var cloudBlobClient = cloudStorageAcccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                await cloudBlobContainer.CreateIfNotExistsAsync();

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(guid.ToString());
                await cloudBlockBlob.UploadFromFileAsync(fileInfo.PhysicalPath);
            });
        }
    }
}
