using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace SpiralFound
{
    public class GetImage
    {
        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            return new WSThumb(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight).GetWSThumb();
        }

        private class WSThumb
        {
            public WSThumb(string Url, int BW, int BH, int TW, int TH)
            {
                tmb_Url = Url;
                tmb_BrowserWidth = BW;
                tmb_BrowserHeight = BH;
                tmb_ThumbnailWidth = TW;
                tmb_ThumbnailHeight = TH;
            }

            private Bitmap tmb_Bitmap = null;
            private string tmb_Url = null;
            private int tmb_ThumbnailWidth;
            private int tmb_ThumbnailHeight;
            private int tmb_BrowserWidth;
            private int tmb_BrowserHeight;

            public string Url
            {
                get { return tmb_Url; }
                set { tmb_Url = value; }
            }

            public Bitmap ThumbnailImage
            {
                get { return tmb_Bitmap; }
            }

            public int ThumbnailWidth
            {
                get { return tmb_ThumbnailWidth; }
                set { tmb_ThumbnailWidth = value; }
            }

            public int ThumbnailHeight
            {
                get { return tmb_ThumbnailHeight; }
                set { tmb_ThumbnailHeight = value; }
            }

            public int BrowserWidth
            {
                get { return tmb_BrowserWidth; }
                set { tmb_BrowserWidth = value; }
            }

            public int BrowserHeight
            {
                get { return tmb_BrowserHeight; }
                set { tmb_BrowserHeight = value; }
            }

            public Bitmap GetWSThumb()
            {
                ThreadStart tmb_threadStart = new ThreadStart(_GenerateWSThumb);
                Thread tmb_thread = new Thread(tmb_threadStart);

                tmb_thread.SetApartmentState(ApartmentState.STA);
                tmb_thread.Start();
                tmb_thread.Join();
                return tmb_Bitmap;
            }

            private void _GenerateWSThumb()
            {
                WebBrowser tmb_WebBrowser = new WebBrowser();
                tmb_WebBrowser.ScrollBarsEnabled = false;
                tmb_WebBrowser.ScriptErrorsSuppressed = true;
                tmb_WebBrowser.Navigate(tmb_Url);
                tmb_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                while (tmb_WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                    Application.DoEvents();
                tmb_WebBrowser.Dispose();
            }

            private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                WebBrowser tmb_WebBrowser = (WebBrowser)sender;
                tmb_WebBrowser.ClientSize = new Size(this.tmb_BrowserWidth, this.tmb_BrowserHeight);
                tmb_WebBrowser.ScrollBarsEnabled = false;
                tmb_WebBrowser.ScriptErrorsSuppressed = true;
                tmb_Bitmap = new Bitmap(tmb_WebBrowser.Bounds.Width, tmb_WebBrowser.Bounds.Height);
                tmb_WebBrowser.BringToFront();
                tmb_WebBrowser.DrawToBitmap(tmb_Bitmap, tmb_WebBrowser.Bounds);

                // if (tmb_ThumbnailHeight != 0 && tmb_ThumbnailWidth != 0)
                //     tmb_Bitmap = (Bitmap)tmb_Bitmap.GetThumbnailImage(tmb_ThumbnailWidth, tmb_ThumbnailHeight, null, IntPtr.Zero);
            }
        }
    }
}
