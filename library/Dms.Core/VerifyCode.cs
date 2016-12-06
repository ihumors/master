namespace Dms.Core
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    public class VerifyCodeModel
    {
        private int length = 4;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        
        private int fontSize = 16;
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        
        private int padding = 2;
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }
        
        private bool noise = false;
        public bool Noise
        {
            get { return noise; }
            set { noise = value; }
        }
        
        private Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        private Color[] colors = {
            Color.Black,
            Color.Red,
            Color.DarkBlue,
            Color.Green,
            Color.Orange,
            Color.Brown,
            Color.DarkCyan,
            Color.Purple,
            Color.Gray
        };
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }

        private string[] fonts = { "Arial", "Verdana", "Georgia", "Comic Sans MS", "Tahoma", "宋体", "simsun", "microsoft yahei", "monospace", "sans-serif", "Menlo", "Monaco", "Consolas" };
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        
        private string codeSerial = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,0,1,2,3,4,5,6,7,8,9";
        public string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }
    }

    public class VerifyCode
    {
        private static readonly double PI2 = 6.283185307179586476925286766559 * 2;
        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="_bitmap">图片路径</param>
        /// <param name="distort">如果扭曲则选择为True</param>
        /// <param name="factor">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="phase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public static Bitmap CreateImageCode(Bitmap _bitmap, bool distort, double factor, double phase, double pi)
        {
            int width = _bitmap.Width;
            int height = _bitmap.Height;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics graph = Graphics.FromImage(bitmap);
            graph.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, width, height);
            graph.Dispose();

            double axis = distort ? (double)height : (double)width;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double xAxis = distort ? (pi * (double)y) / axis : (pi * (double)x) / axis;
                    xAxis = xAxis + phase;
                    double yAxis = Math.Sin(xAxis);

                    int nx = distort ? x : x + (int)(yAxis * factor);
                    int ny = distort ? y : y + (int)(yAxis * factor);

                    Color color = _bitmap.GetPixel(x, y);
                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        bitmap.SetPixel(nx, ny, color);
                    }
                }
            }

            return bitmap;
        }
        public static Bitmap CreateImageCode(string code, VerifyCodeModel model)
        {
            int fontWidth = model.FontSize + model.Padding;
            int imageWidth = (int)(code.Length * fontWidth) + 4 + model.Padding * 2;
            int imageHeight = model.FontSize * 2 + model.Padding;

            Bitmap image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(model.BackgroundColor);

            DrawString(ref g, code, imageWidth, imageHeight, model);
            DrawRandomNoise(ref image,ref g, 100, model);
            DrawCurve(ref g);

            return CreateImageCode(image, true, 2, 4, PI2);
        }
        private static void DrawString(ref Graphics g, string code, int imageWidth, int imageHeight, VerifyCodeModel model)
        {
            float yAxis = (imageHeight - model.FontSize - model.Padding * 2) / 4;
            Random rand = new Random();

            for (int i = 0; i < code.Length; i++)
            {
                int colorIndex = rand.Next(model.Colors.Length - 1);
                int fontIndex = rand.Next(model.Fonts.Length - 1);

                Font font = new Font(model.Fonts[fontIndex], model.FontSize, FontStyle.Bold);
                Brush brush = new SolidBrush(model.Colors[colorIndex]);

                float y = i % 2 == 1 ? yAxis : yAxis * 2;
                float x = i * (model.FontSize + model.Padding);

                g.DrawString(code.Substring(i, 1), font, brush, x, y);
            }
        }
        private static void DrawRandomNoise(ref Bitmap image,ref Graphics g, Int32 pix, VerifyCodeModel model)
        {
            if (!model.Noise) return;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            Random random = new Random();
            for (int i = 0; i < (image.Height * image.Width) / pix; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                if ((x + 1) < image.Width && (y + 1) < image.Height)
                {
                    g.DrawRectangle(new Pen(model.Colors[random.Next(model.Colors.Length - 1)]), 
                        random.Next(image.Width), 
                        random.Next(image.Height), 
                        random.Next(1,3), 
                        random.Next(1,3));
                }
            }
        }
        private static void DrawCurve(ref Graphics g)
        {
            Random rand = new Random();
            Point[] points = new Point[]{
                new Point(1,rand.Next(1,40)),
                new Point(10,rand.Next(1,40)),
                new Point(22,rand.Next(1,40)),
                new Point(30,rand.Next(1,40)),
                new Point(33,rand.Next(1,40)),
                new Point(40,rand.Next(1,40)),
                new Point(55,rand.Next(1,40)),
                new Point(60,rand.Next(1,40)),
                new Point(88,rand.Next(1,40)),
                new Point(100,rand.Next(1,40)),
                new Point(116,rand.Next(1,40))
            };
            Pen pen = new Pen(Color.Green, 2);
            g.DrawCurve(pen, points, 0.1f);
        }
        public static string RandomCode(int length, VerifyCodeModel entity)
        {
            if (length == 0)
                length = entity.Length;

            string result = string.Empty;
            string[] codes = entity.CodeSerial.Split(',');
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < length; i++)
            {
                int randValue = rand.Next(0, codes.Length - 1);
                result += codes[randValue];
            }

            return result;
        }
        public static Color GetRandomDeepColor()
        {
            Random random = new Random();
            int red = random.Next(255);
            int green = random.Next(255);
            int blue = random.Next(255);
            Color color = Color.FromArgb(red, green, blue);
            return color;
        }
    }
}
