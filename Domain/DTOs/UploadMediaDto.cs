using System;
using Azorian.Data;

namespace Azorian.Domain.DTOs
{
    public class UploadMediaDto
    {
        public MediaType MediaType { get; set; }
        public string StoragePath { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public string MimeType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
