using System.Collections.Generic;
using Backend.SharedKernel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Backend.Domain.Images
{
    public class Image : BaseEntity
    {
        public int Id { get; set; }
        public List<ImageFragment> Fragments { get; set; }
        public Label Label { get; set; }

        public string ResolveLayer(int x, int y)
        {
            for (int i = 0; i < Fragments.Count; i++)
            {
                var image = Image<Rgba32>.Load(Fragments[i].File);
                if (image[x, y].A == 255)
                {
                    return Fragments[i].FileName;
                }
            }

            return "";
        }

        public Image(string label)
        {
            Fragments = new List<ImageFragment>();
            Label = new Label(label);
        }

        public Image()
        {
            Fragments = new List<ImageFragment>();
        }
    }
}