using CadFile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadFile.Domain.Repositories
{
    public interface ICadFileMetadataRepository
    {
        IReadOnlyList<CadFileEntity> GetData();
    }
}
