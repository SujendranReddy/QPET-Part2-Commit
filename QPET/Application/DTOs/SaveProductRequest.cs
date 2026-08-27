namespace QPET.Application.DTOs
{
    public class SaveProductRequest
    {
        public int? ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string NeckFinish { get; set; } = string.Empty;

        public string CapacityVolume { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IEnumerable<UploadedFile> Images { get; set; }
            = Enumerable.Empty<UploadedFile>();

        public bool IsActive { get; set; }
    }
}