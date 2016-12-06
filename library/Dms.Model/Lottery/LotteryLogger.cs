using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Model.Lottery
{
    public class LotteryLogger
    {
        public long id { get; set; }
        public int lottery_catalog_id { get; set; }
        public long lottery_schedule_id { get; set; }
        public string deviceid { get; set; }
        public long userid { get; set; }
        public int user_level { get; set; }
        public string ip { get; set; }
        public long lottery_prize_id { get; set; }
        public DateTime prize_send_time { get; set; }
        public string access_token { get; set; }
        public string remark { get; set; }
        public DateTime create_time { get; set; }
    }
    public class LotteryLoggerDto : LotteryLogger
    {
        public byte action_method { get; set; }
    }
}
