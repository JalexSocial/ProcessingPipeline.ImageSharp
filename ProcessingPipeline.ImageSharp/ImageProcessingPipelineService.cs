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
	/// <summary>
	/// Determines the most appropriate aspect ratio for a given image dimension from a set of predefined aspect ratios.
	/// </summary>
	/// <param name="width">The width of the image in pixels.</param>
	/// <param name="height">The height of the image in pixels.</param>
	/// <param name="ratios">An array of AspectRatio objects to choose from.</param>
	/// <returns>
	/// The AspectRatio that most closely matches the input dimensions. 
	/// Returns AspectRatio.Undefined if the input dimensions are invalid.
	/// </returns>
	/// <remarks>
	/// This method calculates the aspect ratio of the input dimensions and compares it to each of the provided aspect ratios.
	/// It selects the AspectRatio with the smallest difference from the input ratio.
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown when the ratios array is null.</exception>
	/// <exception cref="ArgumentException">Thrown when the ratios array is empty.</exception>
    public static AspectRatio GetMostAppropriateAspectRatio(int width, int height, AspectRatio[] ratios)
	{
		if (width <= 0 || height <= 0)
		{
			return AspectRatio.Undefined;
		}

		if (ratios == null)
			throw new ArgumentNullException(nameof(ratios));

		if (ratios.Length == 0)
			throw new ArgumentException("The array of aspect ratios cannot be empty.", nameof(ratios));

        double inputRatio = (double)width / height;
		AspectRatio closestRatio = ratios[0];
		double smallestDifference = double.MaxValue;

		foreach (var ratio in ratios)
		{
			double difference = Math.Abs(inputRatio - ratio.Ratio);

			if (difference < smallestDifference)
			{
				smallestDifference = difference;
				closestRatio = ratio;
			}
		}

		return closestRatio;
	}

	/// <summary>
	/// Resizes and adjusts an image to fit a specified aspect ratio and width, applying smart cropping or padding as needed.
	/// </summary>
	/// <param name="image">The source image to be resized.</param>
	/// <param name="ratio">The target aspect ratio for the output image.</param>
	/// <param name="width">The desired width of the output image in pixels.</param>
	/// <returns>A new Image object resized and adjusted to match the specified aspect ratio and width.</returns>
	/// <remarks>
	/// This method performs the following operations:
	/// 1. If the input image's aspect ratio is close to the target ratio, it crops the image to fit.
	/// 2. If cropping is not suitable, it resizes the image and adds a blurred, darkened background to maintain the aspect ratio.
	/// 3. The background is created by stretching, blurring, and darkening a copy of the original image.
	/// 4. The resized original image is then centered on this background.
	/// 
	/// The method uses a 10% tolerance when deciding whether to crop or pad the image.
	/// </remarks>
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
        
        // Checking if aspect ratio is close enough to crop
        int cropCeiling = (int) (image.Width * size.Height * 1.10);
        int cropFloor = (int) (image.Width * size.Height * 0.90);
        int comparator = image.Height * size.Width;

        if (comparator <= cropCeiling && comparator >= cropFloor)
        {
            Image croppedImage = image.Clone(img => img
                .Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop,
                    Size = size,
                })
            );
            return croppedImage;
        }
        
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
        // The input image is already the correct aspect ratio. Should not execute!
        else
        {
            backgroundLocation = new Point(0, 0);
        }
        
        // Forming the background image before drawing the foreground above it
        Image compositeImage = image.Clone(img => img
            .Resize(new ResizeOptions
            {
                Mode = ResizeMode.Stretch,
                Size = new Size((300 / ratio.Height + 1) * ratio.Width, (300 / ratio.Height + 1) * ratio.Height),
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
