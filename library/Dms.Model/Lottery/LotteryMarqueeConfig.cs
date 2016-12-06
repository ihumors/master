using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Model.Lottery
{
    public class LotteryMarqueeConfig
    {
        public long id { get; set; }
        public int lottery_catalog_id { get; set; }
        public long lottery_schedule_id { get; set; }
        public long lottery_prize_id { get; set; }
        public decimal probability { get; set; }
        public bool is_deleted { get; set; }
        public DateTime create_time { get; set; }
        public string created_by { get; set; }
        public DateTime modify_time { get; set; }
        public string modified_by { get; set; }
        public byte[] row_version { get; set; }
    }
    public class LotteryMarqueeConfigDto : LotteryMarqueeConfig
    {
        public byte action_method { get; set; }
    }
}
