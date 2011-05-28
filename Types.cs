using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LittleGeek
{
    public class Types
    {
        public enum ContentTypes
        {
            Media = 5
        }

        public enum MediaTypes
        {
            Image = 1,
            AudioFile = 2,
            Video = 3
        }

        public enum MediaInfoTypes
        {
            Title = 1,
            Caption = 2,
            Description = 3,
            Metadata = 4,
            ExifDateOriginal = 5,
            ExifCamera = 6,
            ExifExposure = 7,
            ExifAperture = 8,
            ExifISO = 9,
            ExifShutterSpeed = 10
        }
    }
    
}
