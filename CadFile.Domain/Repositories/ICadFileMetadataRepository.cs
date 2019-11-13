using CadFile.Domain.Entities;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadFile.Domain.Repositories
{
    public interface ICadFileMetadataRepository
    {
        IReadOnlyList<CadFileEntity> GetData();
        void Save(CadFileEntity cadFile);
        void Delete(Guid physicalFileName);
    }
}
