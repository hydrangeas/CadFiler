using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadFile.Domain.Repositories
{
    public interface ICadFileStorageRepository
    {
        void Upload(IFileInfo fileInfo);
        void Download(Guid guid);
    }
}
