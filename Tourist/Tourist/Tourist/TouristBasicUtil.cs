using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tourist.Tourist
{
    class TouristBasicUtil
    {

        // 해당파일이 정상적인 EXIF Format을 가진 파일인지 체크하는 메소드
        public bool checkExifFile(string path)
        {
            bool result = false;
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Position = 0;
                byte[] headers = br.ReadBytes(3);
                if (headers[0] == 255 && headers[1] == 216 && headers[2] == 255)
                    result = true;
                br.Close();
                fs.Close();
            }
            return result;
        }
    }
}
