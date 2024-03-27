using FreeSql.DataAnnotations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services.Tables
{
    [Table]
    public record TimeRecord
    {
        public TimeRecord()
        {
            
        }

        public TimeRecord(string title, string message, DateTime beginTime, DateTime endTime)
        {
            Title = title;
            Message = message;
            BeginTime = beginTime;
            EndTime = endTime;
        }

        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }
        public string Title { get; set; } = "";
        [Column(StringLength = -1)]
        public string Message { get; set; } = "";
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        [Column(ServerTime = DateTimeKind.Utc)]
        public DateTime UpdateTime { get; set; }
    }
}
