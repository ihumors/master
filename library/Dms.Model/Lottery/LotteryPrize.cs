using System;

namespace Dms.Model.Lottery
{
    public class LotteryPrize
    {
        public long lottery_prize_id { get; set; }
        public int lottery_catalog_id { get; set; }
        public long lottery_schedule_id { get; set; }
        public string prize_code { get; set; }
        public string prize_name { get; set; }
        public string prize_image_url { get; set; }
        public string prize_path_url { get; set; }
        public int prize_type { get; set; }
        public decimal rate { get; set; }
        public long stock { get; set; }
        public long total { get; set; }
        public int user_level { get; set; }
        public bool is_deleted { get; set; }
        public DateTime create_time { get; set; }
        public string created_by { get; set; }
        public DateTime modify_time { get; set; }
        public string modified_by { get; set; }
        public byte[] row_version { get; set; }
    }
    public class LotteryPrizeDto : LotteryPrize
    {
        public byte action_method { get; set; }
    }
}
