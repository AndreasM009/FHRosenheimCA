using System;

namespace Fhr.Contacts.Common
{
    public class ContactImageInfo
    {
        public Guid ContactId { get; set; }

        public Guid ImageId { get; } = Guid.NewGuid();

        public Uri BlobUri { get; set; }

        public string BlobName => BlobUri.Segments[BlobUri.Segments.Length - 1];
    }
}
