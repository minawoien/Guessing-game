namespace Backend.Domain.Images
{
    public class ImageInfo
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public string Label { get; set; }

        public ImageInfo(int imageId, string label)
        {
            ImageId = imageId;
            Label = label;
        }
    }
}