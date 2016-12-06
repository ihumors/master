using System;

namespace Dms.Model.Lottery
{
    public class LotteryStrategy
    {
        public int lottery_strategy_id { get; set; }
        public int lottery_catalog_id { get; set; }
        public int play_total_count { get; set; }
        public decimal rate { get; set; }
        public int marquee_count { get; set; }
        public DateTime create_time { get; set; }
        public string created_by { get; set; }
        public DateTime modify_time { get; set; }
        public string modified_by { get; set; }
        public byte[] row_version { get; set; }
    }
    public class LotteryStrategyDto : LotteryStrategy
    {
        public byte action_method { get; set; }
    }
    public class LotteryStrategyVo: LotteryStrategy
    {
        public string lottery_catalog_name { get; set; }
    }
}
