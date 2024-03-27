using FreeSql.DataAnnotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services.Tables
{
    [Table]
    public record RecordTag
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }
        public string Name { get; set; } = "";
        [Column(StringLength = -1)]
        public string Comment { get; set; } = "";

        public string Icon { get; set; } = "";
        public string IconType { get; set; } = "Text";
        public string Color { get; set; } = "";

        public long ParentId { get; set; }
        public RecordTag? Parent { get; set; }

        [Column(ServerTime = DateTimeKind.Utc, CanUpdate = false)]
        public DateTime CreateedTime { get; set; }
        [Column(ServerTime = DateTimeKind.Utc)]
        public DateTime EditedTime { get; set; }
    }
}
