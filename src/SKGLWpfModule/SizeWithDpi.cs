using System;
using System.Diagnostics;

namespace SKGLWpfControl;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public struct SizeWithDpi
{
	public static SizeWithDpi Empty = new(0, 0);

	public int Width;
	public int Height;
	public double DpiX;
	public double DpiY;

	public SizeWithDpi(int width, int height, double dpiX = 96.0, double dpiY = 96.0)
	{
		Width = width;
		Height = height;
		DpiX = dpiX;
		DpiY = dpiY;
	}

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        return obj is SizeWithDpi dpi &&
               Width == dpi.Width &&
               Height == dpi.Height &&
               DpiX == dpi.DpiX &&
               DpiY == dpi.DpiY;
    }

    public bool Equals(SizeWithDpi dpi)
    {
	        return Width == dpi.Width &&
	               Height == dpi.Height &&
	               DpiX == dpi.DpiX &&
	               DpiY == dpi.DpiY;
    }

    public override int GetHashCode()
    {
		return HashCode.Combine(Width, Height, DpiX, DpiY);
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
