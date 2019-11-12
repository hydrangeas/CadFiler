using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CadFile.Domain.Repositories
{
    public interface ICadFileStorageRepository
    {
        Task Upload(IFileInfo fileInfo, Guid guid);
        void Download(Guid guid);
    }
}
