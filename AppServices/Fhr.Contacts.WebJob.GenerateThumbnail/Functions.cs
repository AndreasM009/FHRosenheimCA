using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Fhr.Contacts.Common;
using Fhr.Contacts.Models;
using Fhr.Contacts.Repository.Implementation;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Fhr.Contacts.WebJob.GenerateThumbnail
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage(
            [QueueTrigger("%thumbnailqueue%")] ContactImageInfo aImageInfo, 
            [Blob("%thumbnailblob%/{BlobName}", FileAccess.Read)]Stream aInput,
            [Blob("%thumbnailblob%/{BlobName}_thumbnail.jpg")]CloudBlockBlob aOutput)
        {            
            using (var output = aOutput.OpenWrite())
            {
                ConvertImageToThumbnailJpg(aInput, output);
                aOutput.Properties.ContentType = "image/jpeg";
            }

            using (var db = new ContactContext())
            {
                var contact = db.ContactsImages.FirstOrDefault(c => c.ContactId == aImageInfo.ContactId);
                if (null == contact)
                {
                    db.ContactsImages.Add(new ContactImage
                    {
                        ContactId = aImageInfo.ContactId,
                        ImageUrl = aOutput.Uri.ToString()
                    });

                    db.SaveChanges();
                }
                else
                {
                    db.Entry(contact).State = EntityState.Modified;
                    contact.ImageUrl = aOutput.Uri.ToString();
                    db.SaveChanges();
                }
            }
        }

        public static void ConvertImageToThumbnailJpg(Stream input, Stream output)
        {
            int thumbnailsize = 60;
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = thumbnailsize;
                height = thumbnailsize * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = thumbnailsize;
                width = thumbnailsize * originalImage.Width / originalImage.Height;
            }

            Bitmap thumbnailImage = null;
            try
            {
                thumbnailImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                thumbnailImage.Save(output, ImageFormat.Jpeg);
            }
            finally
            {
                if (thumbnailImage != null)
                {
                    thumbnailImage.Dispose();
                }
            }
        }
    }
}
