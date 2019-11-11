using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadFile.Domain.Repositories
{
    public interface ICadFileRepository
    {
        IFileInfo Save(string path);
    }
}
