using DeWalt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace DeWaltPOS.Code
{
    public class StandingMethods
    {

        public static WebImage ResizeImage(HttpPostedFileBase pfile, int size)
        {
            WebImage img = new WebImage(pfile.InputStream);
            double imgwidth = img.Width;
            double imgheight = img.Height;
            double valratio = imgwidth > imgheight ? imgwidth : imgheight;
            imgwidth = (imgwidth / valratio) * size;
            imgheight = (imgheight / valratio) * size;
            img.Resize((int)Math.Round(imgwidth), (int)Math.Round(imgheight), true, true);
            return img;

        }

        public static WebImage ResizeImage(WebImage pfile, int size)
        {
            double imgwidth = pfile.Width;
            double imgheight = pfile.Height;
            double valratio = imgwidth > imgheight ? imgwidth : imgheight;
            imgwidth = (imgwidth / valratio) * size;
            imgheight = (imgheight / valratio) * size;
            pfile.Resize((int)Math.Round(imgwidth), (int)Math.Round(imgheight), true, true);
            return pfile;

        }

        public static void DeleteTempImages()
        {
            var tempfilesdir = HostingEnvironment.MapPath("~" + Globals.ImageTempSavePath);
            string[] picList = Directory.GetFiles(tempfilesdir);
            foreach (string fileName in picList)
                System.IO.File.Delete(fileName);

        }
    }
}