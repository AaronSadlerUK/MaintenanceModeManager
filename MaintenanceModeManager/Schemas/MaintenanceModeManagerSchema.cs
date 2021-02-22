using System;
using System.Runtime.Serialization;
using NPoco;

namespace MaintenanceModeManager.Schemas
{
    [TableName(TableName)]
    [PrimaryKey("ParentId", AutoIncrement = false)]
    public class MaintenanceModeManagerSchema
    {
        [IgnoreDataMember]
        public const string TableName = "maintenanceModeManagerConfig";

        [Column("ParentId")]
        public Guid ParentId { get; set; }

        [Column("MaintenanceModePageId")]
        public Guid MaintenanceModePageId { get; set; }
    }
}