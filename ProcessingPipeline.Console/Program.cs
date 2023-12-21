// See https://aka.ms/new-console-template for more information

using ProcessingPipeline.ImageSharp;
using ProcessingPipeline.ImageSharp.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

using (var img = Image.Load("bulldog.webp"))
{
    // clone the original image, since we need this for future runs of the application
    using (Image destRound = img.Clone(x => x.SmartResize(AspectRatio.Landscape16x9, new Size(1280, 720))))
    {
        destRound.Save("output.webp", new WebpEncoder());
    }
}