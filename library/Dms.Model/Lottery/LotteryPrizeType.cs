namespace Dms.Model.Lottery
{
    public class LotteryPrizeType
    {
        public int id { get; set; }
        public string key { get; set; }
        public int value { get; set; }
        public string path_url { get; set; }
        public string remark { get; set; }
    }
    public class LotteryPrizeTypeDto : LotteryPrizeType
    {
        public byte action_method { get; set; }
    }
}
