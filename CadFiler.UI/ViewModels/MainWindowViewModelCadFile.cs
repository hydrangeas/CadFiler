using CadFile.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CadFiler.UI.ViewModels
{
    public class MainWindowViewModelCadFile
    {
        private CadFileEntity _entity;
        public MainWindowViewModelCadFile(CadFileEntity entity)
        {
            _entity = entity;
        }

        public string LogicalFileName => _entity.LogicalFileName;
        public Guid PhysicalFileName => _entity.PhysicalFileName;
        public long FileSize => _entity.FileSize;
        public int DisplayOrder => _entity.DisplayOrder;
        public DateTime Created => _entity.Created;
        public DateTime Updated => _entity.Updated;
    }
}
