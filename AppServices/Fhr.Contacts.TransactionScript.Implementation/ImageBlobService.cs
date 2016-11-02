//-------------------------------------------------------------------------------------
// Author:      amo
// Created:     8/8/2016 2:57:08 PM
// Copyright (c) white duck Gesellschaft für Softwareentwicklung mbH
//-------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Threading.Tasks;
using Fhr.Contacts.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Fhr.Contacts.TransactionScript.Implementation
{
    public class ImageBlobService : IImageBlobService
    {
        #region Fields

        private readonly CloudQueue mThumbnailQueue = null;
        private readonly CloudBlobContainer mThumbnailBlob = null;

        #endregion

        #region Construction

        public ImageBlobService()
        {
            var thumbnailQueueName = ConfigurationManager.AppSettings["app:thumbnailqueue"];
            var thumbnailBlobName = ConfigurationManager.AppSettings["app:thumbnailblob"];

            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
            var queueClient = storageAccount.CreateCloudQueueClient();
            var blobClient = storageAccount.CreateCloudBlobClient();
            mThumbnailQueue = queueClient.GetQueueReference(thumbnailQueueName);
            mThumbnailBlob = blobClient.GetContainerReference(thumbnailBlobName);
        }

        #endregion

        #region Implementation of IImageBlobService

        async Task IImageBlobService.UploadImage(Guid aUserId, Guid aContactId, byte[] aImage)
        {
            try
            {
                if (null != aImage && 0 != aImage.Length)
                {
                    var msg = new ContactImageInfo
                    {
                        ContactId = aContactId
                    };

                    var blob = await UploadToBlobAsync(msg.ImageId, aImage);
                    msg.BlobUri = blob.Uri;

                    await mThumbnailQueue.CreateIfNotExistsAsync();

                    var qMsg = new CloudQueueMessage(JsonConvert.SerializeObject(msg));
                    await mThumbnailQueue.AddMessageAsync(qMsg);
                }
            }
            catch (Exception)
            {
                // Todo: Log Exception
                return;
            }            
        }

        #endregion

        #region Implementation
        
        private async Task<CloudBlockBlob> UploadToBlobAsync(Guid aImageId, byte[] aImage)
        {
            var blobName = aImageId.ToString();

            await mThumbnailBlob.CreateIfNotExistsAsync();

            var blob = mThumbnailBlob.GetBlockBlobReference(blobName);
            await blob.UploadFromByteArrayAsync(aImage, 0, aImage.Length);
            return blob;
        }

        #endregion
    }
}
