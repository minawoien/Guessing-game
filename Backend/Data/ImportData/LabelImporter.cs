using System.Collections.Generic;
using System.IO;

namespace Backend.Data.ImportData
{
    public class LabelImporter
    {
        public List<(int Id, string Value)> LabelList { get; set; }
        public List<(string ImagePath, int LabelId)> ImageLabel { get; set; }

        public LabelImporter()
        {
            LabelList = new();
            ImageLabel = new();
        }

        public void Import()
        {
            var labels = File.ReadAllLines(Path.Combine("Assets", "label_mapping.csv"));
            foreach (var label in labels)
            {
                var i = label.ToLower().Split();
                LabelList.Add((int.Parse(i[0]), string.Join(' ', i[1..])));
            }

            var images = File.ReadAllLines(Path.Combine("Assets", "image_mapping.csv"));
            foreach (var img in images)
            {
                var i = img.Split();
                ImageLabel.Add((i[0], int.Parse(i[1])));
            }
        }
    }
}