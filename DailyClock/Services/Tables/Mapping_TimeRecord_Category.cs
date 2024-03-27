using FreeSql.DataAnnotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services.Tables
{
    [Table]
    public record Mapping_TimeRecord_Tag
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        public long RecordId { get; set; }
        public TimeRecord? Record { get; set; }

        public long TagId { get; set; }
        public RecordTag? Tag { get; set; }
    }
}
