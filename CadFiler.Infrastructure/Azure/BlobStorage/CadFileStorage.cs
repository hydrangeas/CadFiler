using CadFile.Domain.Repositories;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CadFiler.Infrastructure.Azure.BlobStorage
{
    public class CadFileStorage : ICadFileStorageRepository
    {
        readonly string ConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

        readonly string ContainerName = "cadfiles";

        public void Download(Guid guid)
        {
            throw new NotImplementedException();
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
