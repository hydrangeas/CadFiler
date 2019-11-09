using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadFiler.Infrastructure.LocalDB
{
    public class CadFiles : ICadFileRepository
    {
        public IReadOnlyList<CadFileEntity> GetData()
        {
            throw new NotImplementedException();
        }
    }
}
