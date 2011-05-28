using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace LittleGeek
{
    public class Photo : Media
    {
        public Photo()
        {

        }

        public Photo(string mimeType, Stream decodedContentStream, string title, string caption, string description)
        {
            this.MimeType = mimeType;
            this.ContentStream = decodedContentStream;
            this.Title = title;
            this.Caption = caption;
            this.Description = description;
            this.HasExifData = false;

            this.FileName = NewFileName(mimeType);
            this.MediaType = Types.MediaTypes.Image;
        } 

        private Uri thumbnailUri;
        private Uri smallUri;
        private Uri mediumUri;
        private Uri largeUri;
        private Uri extraLargeUri;
        private Uri originalUri;
        private string mimeType;
        private Stream contentStream;
        private Image originalImage;
        private bool hasExifData;
        private DateTime exifOriginalDate;
        private string exifAperture;
        private string exifShutterSpeed;
        private string exifISO;
        private string exifCamera;
        private string exifExposure;
        private int index;
        private string uuid;

        public string UUID
        {
            get { return this.uuid; }
            set { this.uuid = value; }
        }

        public Uri ThumbnailUri
        {
            get { return this.thumbnailUri; }
            set { this.thumbnailUri = value; }
        }

        public Uri SmallUri
        {
            get { return this.smallUri; }
            set { this.smallUri = value; }
        }

        public Uri MediumUri
        {
            get { return this.mediumUri; }
            set { this.mediumUri = value; }
        }

        public Uri LargeUri
        {
            get { return this.largeUri; }
            set { this.largeUri = value; }
        }

        public Uri ExtraLargeUri
        {
            get { return this.extraLargeUri; }
            set { this.extraLargeUri = value; }
        }

        public Uri OriginalUri
        {
            get { return this.originalUri; }
            set { this.originalUri = value; }
        }

        public string MimeType
        {
            get { return this.mimeType; }
            set { this.mimeType = value; }
        }

        public Stream ContentStream
        {
            get { return this.contentStream; }
            set { this.contentStream = value; }
        }

        public Image OriginalImage
        {
            get { return this.originalImage; }
            set { this.originalImage = value; }
        }

        public bool HasExifData
        {
            get { return this.hasExifData; }
            set { this.hasExifData = value; }
        }

        public DateTime ExifOriginalDate
        {
            get { return this.exifOriginalDate; }
            set { this.exifOriginalDate = value; }
        }

        public string ExifAperture
        {
            get { return this.exifAperture; }
            set { this.exifAperture = value; }
        }

        public string ExifShutterSpeed
        {
            get { return this.exifShutterSpeed; }
            set { this.exifShutterSpeed = value; }
        }

        public string ExifISO
        {
            get { return this.exifISO; }
            set { this.exifISO = value; }
        }

        public string ExifCamera
        {
            get { return this.exifCamera; }
            set { this.exifCamera = value; }
        }

        public string ExifExposure
        {
            get { return this.exifExposure; }
            set { this.exifExposure = value; }
        }

        public int Index
        {
            get { return this.index; }
            set { this.index = value; }
        }

        public void Save()
        {
            try
            {
                Image original = Image.FromStream(this.ContentStream);

                // Hang onto the original image
                this.OriginalImage = original;

                // If a date exists in the original's EXIF data, use that instead; otherwise, just use whatever's there
                try
                {
                    PropertyItem pi = this.OriginalImage.GetPropertyItem((int)KnownEXIFIDCodes.DateTimeOriginal);
                    EXIFPropertyItem epi = new EXIFPropertyItem(pi);

                    if (this.OriginalImage.PropertyItems.Length > 0)
                    {
                        this.FileName = NewFileName(epi.ParsedDate, this.MimeType);
                        this.ExifOriginalDate = epi.ParsedDate;
                        this.HasExifData = true;

                        // Try camera info
                        try
                        {
                            pi = this.OriginalImage.GetPropertyItem((int)KnownEXIFIDCodes.Manufacturer);
                            epi = new EXIFPropertyItem(pi);

                            string camInfo = epi.ParsedString;

                            pi = this.OriginalImage.GetPropertyItem((int)KnownEXIFIDCodes.Model);
                            epi = new EXIFPropertyItem(pi);

                            camInfo += " " + epi.ParsedString;

                            this.ExifCamera = camInfo;
                        }
                        catch(Exception e)
                        {
                            this.ExifCamera = String.Empty;
                        }

                        // Try shutter
                        try
                        {
                            pi = this.OriginalImage.GetPropertyItem((int)KnownEXIFIDCodes.ShutterSpeedValue);
                            epi = new EXIFPropertyItem(pi);

                            this.ExifShutterSpeed = Convert.ToInt32(epi.ParsedRational).ToString();
                        }
                        catch(Exception e)
                        {
                            this.ExifShutterSpeed = String.Empty;
                        }

                        // Try aperture
                        try
                        {
                            pi = this.OriginalImage.GetPropertyItem((int)KnownEXIFIDCodes.ApertureValue);
                            epi = new EXIFPropertyItem(pi);

                            this.ExifAperture = Convert.ToInt32(epi.ParsedRational).ToString();
                        }
                        catch(Exception e)
                        {
                            this.ExifAperture = String.Empty;
                        }

                        // Try ISO
                        try
                        {
                            pi = this.OriginalImage.GetPropertyItem((int)KnownEXIFIDCodes.ISOSpeedRatings);
                            epi = new EXIFPropertyItem(pi);

                            this.ExifISO = epi.ParsedRationalArray.ToString();
                        }
                        catch (Exception e)
                        {
                            this.ExifISO = String.Empty;
                        }

                        // Try exposure
                        try
                        {
                            pi = this.OriginalImage.GetPropertyItem((int)KnownEXIFIDCodes.ExposureTime);
                            epi = new EXIFPropertyItem(pi);

                            this.ExifExposure = Convert.ToInt32(epi.ParsedRational).ToString();
                        }
                        catch (Exception e)
                        {
                            this.ExifExposure = String.Empty;
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    this.HasExifData = false;
                }             

                string pathToOriginal = ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["originalImagePath"] + this.FileName;
                string pathToThumbnail = ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["thumbnailImagePath"] + this.FileName;
                string pathToSmall = ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["smallImagePath"] + this.FileName;
                string pathToMedium = ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["mediumImagePath"] + this.FileName;
                string pathToLarge = ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["largeImagePath"] + this.FileName;
                string pathToExtraLarge = ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["extraLargeImagePath"] + this.FileName;

                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                EncoderParameters eps = new EncoderParameters();
                ImageCodecInfo ici = GetEncoderInfo(mimeType);

                eps.Param[0] = new EncoderParameter(Encoder.Quality, Convert.ToInt64(ConfigurationManager.AppSettings["thumbnailQuality"]));

                int widthThumbnail = Convert.ToInt32(ConfigurationManager.AppSettings["widthThumbnail"]);
                int widthSmall = Convert.ToInt32(ConfigurationManager.AppSettings["widthSmall"]);
                int widthMedium = Convert.ToInt32(ConfigurationManager.AppSettings["widthMedium"]);
                int widthLarge = Convert.ToInt32(ConfigurationManager.AppSettings["widthLarge"]);
                int widthExtraLarge = Convert.ToInt32(ConfigurationManager.AppSettings["widthExtraLarge"]);

                Image thumb = this.OriginalImage.GetThumbnailImage(widthThumbnail, (this.OriginalImage.Height * widthThumbnail / this.OriginalImage.Width), myCallback, IntPtr.Zero);
                Image small = this.OriginalImage.GetThumbnailImage(widthSmall, (this.OriginalImage.Height * widthSmall / this.OriginalImage.Width), myCallback, IntPtr.Zero);
                Image medium = this.OriginalImage.GetThumbnailImage(widthMedium, (this.OriginalImage.Height * widthMedium / this.OriginalImage.Width), myCallback, IntPtr.Zero);
                Image large;
                Image exlarge;

                this.OriginalImage.Save(pathToOriginal);
                thumb.Save(pathToThumbnail, ici, eps);
                small.Save(pathToSmall, ici, eps);
                medium.Save(pathToMedium, ici, eps);

                if (this.OriginalImage.Width > widthLarge)
                {
                    large = this.OriginalImage.GetThumbnailImage(widthLarge, (this.OriginalImage.Height * widthLarge / this.OriginalImage.Width), myCallback, IntPtr.Zero);
                    large.Save(pathToLarge, ici, eps);
                }
                else
                {
                    large = this.OriginalImage;
                    large.Save(pathToLarge);
                }

                if (this.OriginalImage.Width > widthExtraLarge)
                {
                    exlarge = this.OriginalImage.GetThumbnailImage(widthExtraLarge, (this.OriginalImage.Height * widthExtraLarge / this.OriginalImage.Width), myCallback, IntPtr.Zero);
                    exlarge.Save(pathToExtraLarge, ici, eps);
                }
                else
                {
                    exlarge = this.OriginalImage;
                    exlarge.Save(pathToExtraLarge);
                }

                // Create the data record(s)
                this.Create();

                if (this.HasExifData)
                {
                    // Save the date
                    this.SetMediaInfo(Types.MediaInfoTypes.ExifDateOriginal, this.ExifOriginalDate.ToLongDateString() + " " + this.ExifOriginalDate.ToLongTimeString());

                    // Save the camera
                    this.SetMediaInfo(Types.MediaInfoTypes.ExifCamera, this.ExifCamera);

                    // Save the aperture
                    this.SetMediaInfo(Types.MediaInfoTypes.ExifAperture, this.ExifAperture);

                    // Save the ISO setting
                    this.SetMediaInfo(Types.MediaInfoTypes.ExifISO, this.ExifISO);

                    // Save the shutter speed
                    this.SetMediaInfo(Types.MediaInfoTypes.ExifShutterSpeed, this.ExifShutterSpeed);

                    // Save the exposure time
                    this.SetMediaInfo(Types.MediaInfoTypes.ExifExposure, this.ExifExposure);
                }

                // Send to Flickr
                this.SubmitToFlickr();

            }
            catch (Exception e)
            {
                
            }
        }

        /// <summary>
        /// Returns a new date-based filename, in the form YYYYMMDDHHMMSS.jpg.
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static string NewFileName(string mimeType)
        {
            return DateBasedFileName(DateTime.Now, mimeType);
        }

        /// <summary>
        /// Returns a new date-based filename, in the form YYYYMMDDHHMMSS.jpg.
        /// </summary>
        /// <param name="theDate"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static string NewFileName(DateTime theDate, string mimeType)
        {
            return DateBasedFileName(theDate, mimeType);
        }

        /// <summary>
        /// Returns a new date-based filename, in the form YYYYMMDDHHMMSS.jpg.
        /// </summary>
        /// <param name="theDate">A date object representing the date and time when the image was taken or submitted.</param>
        /// <param name="mimeType">A MIME-type reflecting the image type.  (Should be "image/jpeg" in most cases.)</param>
        /// <returns></returns>
        private static string DateBasedFileName(DateTime theDate, string mimeType)
        {
            string yyyy = theDate.Year.ToString();
            string mm = "0" + theDate.Month.ToString();
            string dd = "0" + theDate.Day.ToString();
            string hh = "0" + theDate.Hour.ToString();
            string mi = "0" + theDate.Minute.ToString();
            string se = "0" + theDate.Second.ToString();

            string extension = "jpg";

            if (mimeType == "image/jpeg")
            {
                extension = "jpg";
            }

            string newFilename = yyyy + mm.Substring(mm.Length - 2, 2) + dd.Substring(dd.Length - 2, 2) + hh.Substring(hh.Length - 2, 2) + mi.Substring(mi.Length - 2, 2) + se.Substring(se.Length - 2, 2) + "." + extension;
            return newFilename;
        }

        /// <summary>
        /// Returns the default file-system path (the top level) for storing images.
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static string DefaultFilePath
        {
            get { return ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["originalImagePath"]; }
        }

        /// <summary>
        /// Returns an image cropped to the specified dimensions.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="toHeight"></param>
        /// <param name="toWidth"></param>
        /// <returns></returns>
        public Image Crop(Image img, int toHeight, int toWidth)
        {
            Bitmap sourceImage = new Bitmap(img);
            Bitmap newImage = new Bitmap(toHeight, toWidth);

            Graphics newGraphics = Graphics.FromImage(newImage);
            newGraphics.DrawImage(sourceImage, -80, 0);

            return newImage;
        }

        /// <summary>
        /// Submits an image (via SMTP connection) to the Flickr photo service, 
        /// using the e-mail address defined in the application (or Web) configuration file.
        /// </summary>
        public void SubmitToFlickr()
        {
            MailMessage m = new MailMessage(ConfigurationManager.AppSettings["mailSenderFlickr"], ConfigurationManager.AppSettings["mailRecipientFlickr"], this.Title, this.Description);
            SmtpClient s = new SmtpClient(ConfigurationManager.AppSettings["mailHost"]);
            
            m.Attachments.Add(new Attachment(ConfigurationManager.AppSettings["imagePathRoot"] + ConfigurationManager.AppSettings["originalImagePath"] + this.FileName));
            m.IsBodyHtml = false;
            s.UseDefaultCredentials = false;
            s.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailUsername"], ConfigurationManager.AppSettings["mailPassword"]);
            s.Send(m);

            m.Dispose();
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].MimeType == mimeType)
                {
                    return encoders[i];
                }
            }

            return null;
        }

        private bool ThumbnailCallback()
        {
            return false;
        }
    }
}
