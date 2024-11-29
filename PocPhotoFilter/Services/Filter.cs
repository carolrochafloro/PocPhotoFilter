using PocPhotoFilter.Contracts.Interfaces;
using System.Drawing;

namespace PocPhotoFilter.Services;

public class Filter : IFilter
{
    public Image ApplyFilter(Image original)
    {

        var newImage = new Bitmap(original.Width, original.Height);

        for (int y = 0; y < original.Height; y++)
        {
            for (int x = 0; x < original.Width; x++)
            {
                var originalColor = ((Bitmap)original).GetPixel(x, y);
                var grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * 0.59) + (originalColor.B * 0.11));
                var newColor = Color.FromArgb(grayScale, grayScale, grayScale);
                newImage.SetPixel(x, y, newColor);
            }
        }

        return newImage;
    }
}
