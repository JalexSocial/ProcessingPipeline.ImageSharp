using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using System.Text.RegularExpressions;
using ProcessingPipeline.ImageSharp.Models;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace ProcessingPipeline.ImageSharp;

public static class ImageProcessor
{
    public static IImageProcessingContext SmartResize(this IImageProcessingContext ctx, AspectRatio ratio, Size size)
    {
        // TODO: Compute blurhash for the image
        // TODO: Create blurhash layer background
        // TODO: Get the current size of the image
        // TODO: Resize image so that it fits within maxSize
        // TODO: Resize image so that it matches the aspect ratio width/height
        // TODO: Composite new image on top of blurhash background

        return ctx;
    }

    // This method can be seen as an inline implementation of an `IImageProcessor`:
    // (The combination of `IImageOperations.Apply()` + this could be replaced with an `IImageProcessor`)
    public static IImageProcessingContext ApplyRoundedCorners(this IImageProcessingContext ctx, float cornerRadius)
    {
        Size size = ctx.GetCurrentSize();
        IPathCollection corners = BuildCorners(size.Width, size.Height, cornerRadius);
        
        // mutating in here as we already have a cloned original
        // use any color (not Transparent), so the corners will be clipped
        return ctx.Fill(new SolidBrush(Color.LimeGreen) , corners);
    }

    private static IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
    {
        // first create a square
        var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

        // then cut out of the square a circle so we are left with a corner
        // Changing the offsets will change how much of the shape is cut out
        IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

        // corner is now a corner shape positions top left
        //lets make 3 more positioned correctly, we can do that by translating the original around the center of the image

        float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
        float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

        // move it across the width of the image - the width of the shape
        // cut the remaining 3 corners
        IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
        IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
        IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

        return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
    }
}
