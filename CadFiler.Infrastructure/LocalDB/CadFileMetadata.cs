using CadFile.Domain.Entities;
using CadFile.Domain.Repositories;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CadFiler.Infrastructure.LocalDB
{
    public class CadFileMetadata : ICadFileMetadataRepository
    {
        readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ower\source\repos\CadFiler\CadFiler.Infrastructure\LocalDB\SampleData\CadFile.mdf;Integrated Security=True";

        public IReadOnlyList<CadFileEntity> GetData()
        {
            string sql = @"
SELECT [id]
      ,[logical_name]
      ,[physical_name]
      ,[file_size]
      ,[display_order]
      ,[created]
      ,[updated]
  FROM [metadata]
";

            var result = new List<CadFileEntity>();
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(
                            new CadFileEntity(
                                Convert.ToString(reader["logical_name"]),
                                new Guid(Convert.ToString(reader["physical_name"])),
                                Convert.ToInt32(reader["file_size"]),
                                Convert.ToInt32(reader["display_order"]),
                                Convert.ToDateTime(reader["created"]),
                                Convert.ToDateTime(reader["updated"])));
                    }
                }
            }
            return result;
        }

        public void Save(CadFileEntity cadFileEntity)
        {
            string insert = @"
INSERT INTO metadata
(logical_name, physical_name, file_size, display_order, created, updated)
VALUES
(@logical_name, @physical_name, @file_size, @display_order, @created, @updated)
";
            string update = @"
UPDATE metadata
SET logical_name = @logical_name
   ,file_size = @file_size
   ,display_order = @display_order
   ,updated = @updated
WHERE physical_name = @physical_name
";


            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(update, connection))
            {
                connection.Open();
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@logical_name", cadFileEntity.LogicalFileName),
                    new SqlParameter("@physical_name", cadFileEntity.PhysicalFileName),
                    new SqlParameter("@file_size", cadFileEntity.FileSize),
                    new SqlParameter("@display_order", cadFileEntity.DisplayOrder),
                    new SqlParameter("@created", cadFileEntity.Created),
                    new SqlParameter("@updated", cadFileEntity.Updated)
                };
                command.Parameters.AddRange(parameters.ToArray());

                if (command.ExecuteNonQuery() < 1)
                {
                    command.CommandText = insert;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Guid physicalFileName)
        {
            throw new NotImplementedException();
        }
    }
}
