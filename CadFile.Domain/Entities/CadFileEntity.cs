using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CadFile.Domain.Entities
{
    public sealed class CadFileEntity
    {
        public CadFileEntity(FileInfo fileInfo,
                             Guid physicalFileName,
                             int displayOrder,
                             DateTime created)
            :this(
                 fileInfo.Name,
                 physicalFileName,
                 fileInfo.Length,
                 displayOrder,
                 created,
                 created
                 )
        {
        }

        public CadFileEntity(string logicalFileName,
                             Guid physicalFileName,
                             long fileSize,
                             int displayOrder,
                             DateTime created,
                             DateTime updated)
        {
            LogicalFileName = logicalFileName;
            PhysicalFileName = physicalFileName;
            FileSize = fileSize;
            DisplayOrder = displayOrder;
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
