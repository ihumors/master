using System;

namespace Dms.Model.Lottery
{
    public class LotterySchedule
    {
        public long lottery_schedule_id { get; set; }
        public int lottery_catalog_id { get; set; }
        public int play_limit { get; set; }
        public int share_limit { get; set; }
        public int prize_limit { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public bool is_deleted { get; set; }
        public DateTime create_time { get; set; }
        public string created_by { get; set; }
        public DateTime modify_time { get; set; }
        public string modified_by { get; set; }
        public byte[] row_version { get; set; }
    }
    public class LotteryScheduleDto: LotterySchedule
    {
        public byte action_method { get; set; }
    }
}
