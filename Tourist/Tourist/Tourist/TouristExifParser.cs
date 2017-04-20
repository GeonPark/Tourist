using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetadataExtractor;
using System.IO;

namespace Tourist.Tourist
{
    public static class TouristExifParser
    {
        // EXIF Format을 가진 Image의 정보를 가져오는 메소드
        public static ExifValue getImageInfo(string imagePath)
        {
            ExifValue ev = new ExifValue();
            FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            var directories = ImageMetadataReader.ReadMetadata(fs);
            fs.Close();

            // GPS 정보 
            var gpsInstance = directories.OfType<MetadataExtractor.Formats.Exif.GpsDirectory>().FirstOrDefault();
            try
            {
                ev.Latitue = gpsInstance.GetGeoLocation().Latitude;
                ev.Longitude = gpsInstance.GetGeoLocation().Longitude;
            } catch (NullReferenceException){

            }

            return ev;
        }
    }

    public class ExifValue
    {
        public ExifValue() { }

        // GPS Value
        public double Latitue { get; set; }
        public double Longitude { get; set; }

    }
}
