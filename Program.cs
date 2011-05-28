using System;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Drawing;
using System.Drawing.Imaging;
using ADODB;
using CDO;
using System.Net;
using System.Net.Security;
using System.Text;

namespace LittleGeek
{
    class Program
    {
        static void Main(string[] args)
        {
            string emlPath = String.Empty;

            if (args.Length > 0)
            {
                emlPath = args[0].ToString();

                string emlContent = new StreamReader(emlPath).ReadToEnd();

                CDO.Message m = new CDO.Message();
                ADODB.Stream s = new ADODB.Stream();

                s = m.GetStream();
                s.Type = ADODB.StreamTypeEnum.adTypeText;
                s.LoadFromFile(emlPath);
                s.Flush();

                if (m.To.IndexOf(ConfigurationManager.AppSettings["addressPhotos"]) >= 0)
                {
                    foreach (IBodyPart bp in m.BodyPart.BodyParts)
                    {
                        if (bp.ContentMediaType == "image/jpeg")
                        {
                            string tempFilePath = ConfigurationManager.AppSettings["temporaryFilePath"] + System.Guid.NewGuid() + ".jpg";
                            bp.GetDecodedContentStream().SaveToFile(tempFilePath, SaveOptionsEnum.adSaveCreateOverWrite);

                            StreamReader sr = new StreamReader(tempFilePath);
                            Photo p = new Photo(bp.ContentMediaType, sr.BaseStream, m.Subject, m.TextBody, String.Empty);

                            // Pass up the sending and recipient addresses as well
                            p.SenderAddress = m.Sender;
                            p.RecipientAddress = m.To;

                            // Save the image
                            p.Save();

                            sr.Dispose();
                            File.Delete(tempFilePath);
                        }
                    }
                }

                s.Close();
            }
        }
    }
}
