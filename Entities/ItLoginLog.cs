using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_login_Log")]
    public partial class ItLoginLog
    {
        public int Id { get; set; }
        public int UserItId { get; set; }
        [StringLength(1000)]
        public string IpAddress { get; set; }
        [StringLength(1000)]
        public string BrowserType { get; set; }
        [StringLength(1000)]
        public string DeviceType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogInTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogOutTime { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(12, 9)")]
        public decimal? Longitude { get; set; }

        [ForeignKey("UserItId")]
        [InverseProperty("ItLoginLogs")]
        public virtual ItUserMaster UserIt { get; set; }
    }
}