using CadFile.Domain.Repositories;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CadFiler.Infrastructure.LocalFile
{
    public class CadFiles : ICadFileRepository
    {
        public IFileInfo GetFileInfo(string path)
        {
            var physicalFileProvider = new PhysicalFileProvider(Path.GetDirectoryName(path));
            return physicalFileProvider.GetFileInfo(Path.GetFileName(path));
        }
    }
}
