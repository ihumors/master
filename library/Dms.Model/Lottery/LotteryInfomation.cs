using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Model.Lottery
{
    public class LotteryInfomation
    {
        public long id { get; set; }
        public int lottery_catalog_id { get; set; }
        public long lottery_schedule_id { get; set; }
        public long userid { get; set; }
        public int shared { get; set; }
        public int played { get; set; }
        public int prize_count { get; set; }
        public int lucky_count { get; set; }
        public DateTime create_time { get; set; }
        public DateTime modify_time { get; set; }
        public byte[] row_version { get; set; }
    }
    public class LotteryInfomationDto : LotteryInfomation
    {
        public byte action_method { get; set; }
    }
}
