using System;
using System.Collections.Generic;
using System.Text;

namespace CadFile.Domain.Entities
{
    public sealed class CadFileEntity
    {
        public CadFileEntity(string logicalFileName,
                             Guid physicalFileName,
                             long fileSize,
                             int displayOrder,
                             DateTime created,
                             DateTime updated)
        {
            LogicalFileName = logicalFileName;
            PhysicalFileName = physicalFileName;
            fileSize = FileSize;
            displayOrder = DisplayOrder;
            Created = created;
            Updated = updated;
        }

        public string LogicalFileName { get; }
        public Guid PhysicalFileName { get; }
        public long FileSize { get; }
        public int DisplayOrder { get; }
        public DateTime Created { get; }
        public DateTime Updated { get; }
    }
}
