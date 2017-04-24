using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetadataExtractor;
using System.IO;
using System.Windows.Forms;
using MetadataExtractor.Formats.Exif;
namespace Tourist.Tourist
{
    public static class TouristExifParser
    {
        // EXIF Format을 가진 Image의 정보를 가져오는 메소드
        public static ExifValue getImageInfo(string imagePath)
        {
            ExifValue ev = new ExifValue();

            ev.Filename = Path.GetFileName(imagePath);

            FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            var directories = ImageMetadataReader.ReadMetadata(fs);
            fs.Close();

            // GPS 정보 
            var gpsInstance = directories.OfType<GpsDirectory>().FirstOrDefault();
            try
            {
                ev.Latitue = gpsInstance.GetGeoLocation().Latitude;
                ev.Longitude = gpsInstance.GetGeoLocation().Longitude;
            } catch (NullReferenceException){
                ev.existsGPS = false;

            }
            
            // SubIFD 정보
            var SubIFDinstance = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            try
            {
                /*
                ev.OriginalDate = DateTime.ParseExact(SubIFDinstance.GetDescription(ExifDirectoryBase.TagDateTimeOriginal), 
                    "yyyy;MM;dd HH:mm:ss",System.Globalization.CultureInfo.InvariantCulture);
                ev.DigitDate = DateTime.ParseExact(SubIFDinstance.GetDescription(ExifDirectoryBase.TagDateTimeDigitized),
                    "yyyy;MM;dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                 */
                ev.OriginalDate = SubIFDinstance.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
                ev.DigitDate = SubIFDinstance.GetDescription(ExifDirectoryBase.TagDateTimeDigitized);

            }
            catch (NullReferenceException)
            {
                ev.existsDate = false;
            }

            return ev;
        }
    }

    public class ExifValue
    {
        public ExifValue() { }

        private bool chkGPS = true;
        private bool chkDate = true;

        public string Filename { get; set; }

        // GPS Value
        public bool existsGPS { get { return chkGPS; } set { chkGPS = value; } }
        public double Latitue { get; set; }
        public double Longitude { get; set; }
        
        // EXIF SubIFD Value
        public bool existsDate { get { return chkDate; } set { chkDate = value; } }
        public string OriginalDate { get; set; }
        public string DigitDate { get; set; }


    }
}
