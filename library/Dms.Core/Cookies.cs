namespace Dms.Core
{
    using System;
    using System.Web;
    public class Cookies : IDisposable
    {
        private string name = "Net.Auth";
        public string Name
        {
            set { this.name = value; }
            get { return this.name; }
        }
        private HttpCookie mycookie;
        public Cookies()
        {
            this.SetDefaultCookie();
        }
        public Cookies(string name)
        {
            this.Name = name;
            this.SetDefaultCookie();
        }
        ~Cookies()
        {
            this.Dispose(false);
        }
        public bool Set(string key, string value,bool isNeverExpire=false)
        {
            value = HttpContext.Current.Server.UrlEncode(value);
            if (!string.IsNullOrEmpty(this.Get(key)))
            {
                this.mycookie.Values.Remove(key);
            }

            this.mycookie.Values.Add(key, value);

            if (isNeverExpire)
            {
                this.mycookie.Expires = DateTime.MaxValue;
            }

            HttpContext.Current.Response.AppendCookie(this.mycookie);
            return true;
        }
        public string Get(string key)
        {
            if (!this.Exists(this.Name)) return string.Empty;

            string value = this.mycookie.Values[key] + "";
            return HttpContext.Current.Server.UrlDecode(value.Trim());
        }
        public bool Exists()
        {
            return this.Exists(this.Name);
        }
        public bool Exists(string name)
        {
            var cookies = HttpContext.Current.Request.Cookies[name];
            if (cookies != null)  return true;

            return false;
        }
        public bool Clear()
        {
            if (!this.Exists(this.Name)) return true;

            this.mycookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.AppendCookie(this.mycookie);

            return true;
        }
        public bool Remove(string key)
        {
            if (!this.Exists(this.Name)) return true;

            this.mycookie.Values.Remove(key);
            HttpContext.Current.Response.AppendCookie(this.mycookie);

            return true;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (mycookie == null) return;

            mycookie = null;
            GC.SuppressFinalize(this);
        }
        private void SetDefaultCookie()
        {
            this.mycookie = this.Exists(this.Name) ? HttpContext.Current.Request.Cookies[this.Name] : new HttpCookie(this.Name);
        }
        public void Dispose()
        {
            Dispose(true);
        }

    }
}
