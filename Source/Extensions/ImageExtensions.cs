using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLWrapper
{
    public static class ImageExtensions
    {
        public static byte[] GetPixelsBytesFromImage(this Image<Rgba32> image)
        {            
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            var pixels = new List<byte>();           
            for (int i = 0; i < image.Width; ++i){
                for (int j = 0; j < image.Height; ++j)
                {
                    pixels.Add(image[i, j].R);
                    pixels.Add(image[i, j].G);
                    pixels.Add(image[i, j].B);
                    pixels.Add(image[i, j].A);
                }
            }
            return pixels.ToArray();
        }
        public static byte[] GetPixelsBytesFromImagePath(this string filepath)
        {
            if(!(filepath.EndsWith("jpg") || filepath.EndsWith("png")))
            {
                throw new UnknownImageFormatException(string.Format("the {0} has a extension that is not supported by the software. Use either jpg or png files", filepath));
            }
            return GetPixelsBytesFromImage(Image.Load<Rgba32>(filepath));
        }
    }
}
