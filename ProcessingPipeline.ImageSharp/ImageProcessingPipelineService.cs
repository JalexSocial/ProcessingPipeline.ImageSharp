using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessingPipeline.ImageSharp.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ProcessingPipeline.ImageSharp;

public class ImageProcessingPipelineService
{
    public Image SmartResize(Image image, AspectRatio ratio, int width)
    {
        // Resize size into correct aspect ratio. Might be able to throw error here in the future if size does not match
        Size size = new Size(width, 0);
        size.Height = size.Width / ratio.Width * ratio.Height;
        
        var foreground = image.Clone(fg => fg.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = size,
        }));

        Image finalImage = image;
        
        if (image.Width * size.Height >= image.Height * size.Width)
        {
            Image compositeImage = image.Clone(img => img
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Stretch,
                    Size = size,
                })
                .GaussianBlur(100f)
                .Brightness(0.5f)
                .DrawImage(foreground, new Point(0, size.Height / 2 - foreground.Height / 2), 1f)
            );
            finalImage = compositeImage;
        } else if (image.Width * size.Height < image.Height * size.Width)
        {
            Image compositeImage = image.Clone(img => img
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Stretch,
                    Size = size,
                })
                .GaussianBlur(100f)
                .Brightness(0.5f)
                .DrawImage(foreground, new Point(size.Width / 2 - foreground.Width / 2, 0), 1f)
            );
            finalImage = compositeImage;
        }

        return finalImage;
    }
}
