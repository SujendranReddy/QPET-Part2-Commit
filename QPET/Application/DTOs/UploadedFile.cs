namespace QPET.Application.DTOs
{
    public class UploadedFile
    {
        public string OriginalFileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long Length { get; set; }

        public Stream Content { get; set; } = Stream.Null;
    }
}