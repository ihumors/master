using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dms.Model.Lottery
{
    public class LotteryBlackUser
    {
        public long userid { get; set; }
        public bool is_deleted { get; set; }
        public DateTime create_time { get; set; }
    }
    public class LotteryBlackUserDto : LotteryBlackUser
    {
        public byte action_method { get; set; }
    }
}
