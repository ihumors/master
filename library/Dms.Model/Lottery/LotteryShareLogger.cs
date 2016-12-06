using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Model.Lottery
{
    public class LotteryShareLogger
    {
        public long id { get; set; }
        public int lottery_catalog_id { get; set; }
        public long lottery_schedule_id { get; set; }
        public long userid { get; set; }
        public string target { get; set; }
        public DateTime create_time { get; set; }
    }
}
