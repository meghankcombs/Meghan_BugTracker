using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Models
{
    public class ImageUploadValidator
    {
        public static bool IsWebFriendlyFile(HttpPostedFileBase file)
        {           
            if (file == null)
                return false;

            try
            {           
                var fileExt = Path.GetExtension(file.FileName);
                return fileExt == ".pdf"
                    || fileExt == ".doc"
                    || fileExt == ".docx"
                    || fileExt == ".xls"
                    || fileExt == ".xlsx"
                    || fileExt == ".txt"
                    || fileExt == ".jpg"
                    || fileExt == ".png"
                    || fileExt == ".jpeg"
                    || fileExt == ".bmp"
                    || fileExt == ".gif";            
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}