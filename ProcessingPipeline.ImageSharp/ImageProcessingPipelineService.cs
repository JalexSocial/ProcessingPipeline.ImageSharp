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
    public Image SmartResize(Image image, AspectRatio ratio, Size size)
    {
        // Resize size into correct aspect ratio. Might be able to throw error here in the future if size does not match
        size.Height = size.Width / ratio.Width * ratio.Height;
        
        using var foreground = image.Clone(fg => fg.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = size,
        }));
        
        using Image compositeImage = image.Clone(img => img
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Stretch,
                    Size = size,
                })
                .GaussianBlur(100f)
                .Brightness(0.5f)
                .DrawImage(foreground, new Point(size.Width/2-foreground.Width/2, 0), 1f )
        );

        return compositeImage;
    }
}
