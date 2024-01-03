// See https://aka.ms/new-console-template for more information

using ProcessingPipeline.ImageSharp;
using ProcessingPipeline.ImageSharp.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;



string filename = "Landscape-Color.jpg";
// string filename = "15542038745ca344e267fb80.webp";

using var img = Image.Load(filename);

ImageProcessingPipelineService a = new ImageProcessingPipelineService();

a.SmartResize(img, AspectRatio.Landscape16x9, 1080).Save("output.jpg", new JpegEncoder()
{
	Quality = 100
});


/*
using var foreground = img.Clone(fg => fg.Resize(new ResizeOptions
{
	Mode = ResizeMode.Max,
	Size = new Size(1280, 720),
}));

// clone the original image, since we need this for future runs of the application
using Image background = img.Clone(x => x
	.Resize(new ResizeOptions
	{
		Mode = ResizeMode.Stretch,
		Size = new Size(1280, 720),
	})
	.GaussianBlur(100f)
	.Brightness(0.5f)
	.DrawImage(foreground, new Point(1280/2-foreground.Width/2, 0), 1f ))
	;

background.Save("output.jpg", new JpegEncoder()
{
	Quality = 100
});
*/
