namespace Backend.Domain.Images
{
    public class ImageFragment
    {
        public int Id { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }

        public ImageFragment(byte[] file, string filename, string mimeType)
        {
            File = file;
            FileName = filename;
            MimeType = mimeType;
        }

        public ImageFragment()
        {
        }
    }
}