using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Tourist.Tourist
{
    public static class TouristFileInfo
    {

        // 해당파일의 기본정보를 가져오는 메소드
        public static PropertyFileInfo getFileInfo(string filePath)
        {
            PropertyFileInfo pf = new PropertyFileInfo();

            FileInfo fi = new FileInfo(filePath);
            pf.Filesize = fi.Length;
            pf.CreateTime = fi.CreationTime;
            pf.LastAccessTime = fi.LastAccessTime;
            pf.LastWriteTime = fi.LastWriteTime;
            pf.Filename = Path.GetFileName(filePath);

            return pf;
        }
        
    }

    public class PropertyFileInfo
    {
        public string Filename { get; set; }
        public long Filesize { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastWriteTime { get; set; }


    }
}
