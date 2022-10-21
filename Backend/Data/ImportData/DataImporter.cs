using System;
using System.IO;
using System.Linq;
using Backend.Domain.Images;
using Microsoft.AspNetCore.StaticFiles;

namespace Backend.Data.ImportData
{
    public class DataImporter
    {
        private GameContext _db { get; set; }

        public string Import()
        {
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            if (_db.Images.ToArray().Length > 0)
            {
                return "Nothing to import";
            }

            LabelImporter labelImporter = new();
            labelImporter.Import();
            var imgFolder = Directory.GetDirectories("Assets");
            var imgFolders = Directory.GetDirectories(imgFolder[0]);

            foreach (var folder in imgFolders)
            {
                //retrieves the imagename from the folderpath
                var imageName = string.Join('_', folder.Split(Path.DirectorySeparatorChar)[2].Split('_')[..3]);
                Console.WriteLine(imageName);


                //retrieves the label for the image
                var imageLabel = labelImporter.ImageLabel
                    .FirstOrDefault(l => l.ImagePath == imageName).LabelId;
                var labelValue = labelImporter.LabelList
                    .FirstOrDefault(l => l.Id == imageLabel).Value;

                var newImage = new Image(labelValue);

                var filesPaths = Directory.GetFiles(folder, "*.png"); // Only want images

                foreach (var filepath in filesPaths)
                {
                    contentTypeProvider.TryGetContentType(filepath, out var contentType);
                    var fragmentName = filepath.Split(Path.DirectorySeparatorChar).Last();
                    var fileName = $"{imageName}_{fragmentName}";
                    using FileStream fs = File.OpenRead(filepath);
                    byte[] imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, (int) fs.Length);
                    ImageFragment imageFragment = new(imageBytes, fileName, contentType);
                    newImage.Fragments.Add(imageFragment);
                }

                _db.Images.Add(newImage);
            }

            _db.SaveChanges();
            return "Finished importing images";
        }

        public DataImporter(GameContext db)
        {
            _db = db;
        }
    }
}