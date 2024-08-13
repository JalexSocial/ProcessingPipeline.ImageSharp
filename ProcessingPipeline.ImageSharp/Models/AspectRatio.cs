using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingPipeline.ImageSharp.Models;

public class AspectRatio
{
    public int Width { get; }
    public int Height { get; }

    public AspectRatio(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public static readonly AspectRatio Undefined = new AspectRatio(0, 0);
    public static readonly AspectRatio Square = new AspectRatio(1, 1);
    public static readonly AspectRatio Landscape16x9 = new AspectRatio(16, 9);
    public static readonly AspectRatio Portrait4x5 = new AspectRatio(4, 5);
    public static readonly AspectRatio Portrait5x4 = new AspectRatio(5, 4);
    public static readonly AspectRatio Portrait3x4 = new AspectRatio(3, 4);
    public static readonly AspectRatio Landscape4x3 = new AspectRatio(4, 3);
    public static readonly AspectRatio Landscape3x2 = new AspectRatio(3, 2);
    public static readonly AspectRatio Banner16x4 = new AspectRatio(16, 4);
    public static readonly AspectRatio Landscape191x100 = new AspectRatio(191, 100);
    public static readonly AspectRatio Vertical9x16 = new AspectRatio(9, 16);
    public static readonly AspectRatio Panorama2x1 = new AspectRatio(2, 1);
    public static readonly AspectRatio WidePanorama3x1 = new AspectRatio(3, 1);

    public static readonly AspectRatio[] AllAspectRatios = new AspectRatio[]
    {
	    Square,
	    Landscape16x9,
	    Portrait4x5,
	    Portrait5x4,
	    Portrait3x4,
	    Landscape4x3,
	    Landscape3x2,
	    Banner16x4,
	    Landscape191x100,
	    Vertical9x16,
	    Panorama2x1,
	    WidePanorama3x1
    };
    
    public double Ratio => (double)Width / Height;
}
