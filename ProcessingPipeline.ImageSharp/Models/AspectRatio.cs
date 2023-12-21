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
}
