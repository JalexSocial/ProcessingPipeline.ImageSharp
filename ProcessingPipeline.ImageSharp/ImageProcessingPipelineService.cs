using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // Forming the final image size using aspect ratio and width
        Size size = new Size(width, 0);
        size.Height = size.Width / ratio.Width * ratio.Height;
        
        // Resizing the foreground image to the requested size
        var foreground = image.Clone(fg => fg.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = size,
        }));

        // Depending on the input image's aspect ratio, set the background location accordingly
        Point backgroundLocation;
        
        // The input image is wider than requested
        if (image.Width * size.Height > image.Height * size.Width)
        {
            backgroundLocation = new Point(0, size.Height / 2 - foreground.Height / 2);
        } 
        // The input image is taller than requested
        else if (image.Width * size.Height < image.Height * size.Width)
        {
            backgroundLocation = new Point(size.Width / 2 - foreground.Width / 2, 0);
        }
        // The input image is already the correct aspect ratio
        else
        {
            backgroundLocation = new Point(0, 0);
        }
        
        // Forming the background image before drawing the foreground above it
        Image compositeImage = image.Clone(img => img
            .Resize(new ResizeOptions
            {
                Mode = ResizeMode.Stretch,
                Size = new Size(300, 300),
            })
            .GaussianBlur(50f)
            .Resize(new ResizeOptions
            {
                Mode = ResizeMode.Stretch,
                Size = size,
            })
            .Brightness(0.5f)
            .DrawImage(foreground, backgroundLocation, 1f)
        );
        
        return compositeImage;
    }
}
