namespace Dms.Master.Models
{
    public class PopWinModel
    {
        private string id = "dms-popwin-container";
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 是否有底部操作按钮
        /// </summary>
        private bool hasBottomWrapper = true;
        public bool HasBottomWrapper
        {
            get { return hasBottomWrapper; }
            set { hasBottomWrapper = value; }
        }
    }
}