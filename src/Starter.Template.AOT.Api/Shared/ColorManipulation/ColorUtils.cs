namespace Starter.Template.AOT.Api.Shared.ColorManipulation;

public static class ColorUtils
{
    private static readonly string[] ColorPalette =
    [
        "#FF6633", "#FFB399", "#FF33FF", "#FFFF99", "#00B3E6",
        "#E6B333", "#3366E6", "#999966", "#99FF99", "#B34D4D",
        "#80B300", "#809900", "#E6B3B3", "#6680B3", "#66991A",
        "#FF99E6", "#CCFF1A", "#FF1A66", "#E6331A", "#33FFCC",
        "#66994D", "#B366CC", "#4D8000", "#B33300", "#CC80CC",
        "#66664D", "#991AFF", "#E666FF", "#4DB3FF", "#1AB399",
        "#E666B3", "#33991A", "#CC9999", "#B3B31A", "#00E680",
        "#4D8066", "#809980", "#E6FF80", "#1AFF33", "#999933",
        "#FF3380", "#CCCC00", "#66E64D", "#4D80CC"
    ];

    public static string GetColorForIndex(int index) =>
        ColorPalette[index % ColorPalette.Length];

    public static string GenerateBaseColor(string hexColor)
    {
        var (r, g, b) = ParseHex(hexColor);
        var (h, s, l) = RgbToHsl(r, g, b);
        l = Math.Min(1.0, l + 0.40);
        var (nr, ng, nb) = HslToRgb(h, s, l);
        return ToHex(nr, ng, nb);
    }

    public static string InterpolateToGrey(string hexColor, double factor)
    {
        var (r, g, b) = ParseHex(hexColor);
        const int grey = 200;
        var nr = (int)(r + (grey - r) * factor);
        var ng = (int)(g + (grey - g) * factor);
        var nb = (int)(b + (grey - b) * factor);
        return ToHex(nr, ng, nb);
    }

    public static string Saturate(string hexColor, double amount)
    {
        var (r, g, b) = ParseHex(hexColor);
        var (h, s, l) = RgbToHsl(r, g, b);
        s = Math.Min(1.0, s + amount);
        var (nr, ng, nb) = HslToRgb(h, s, l);
        return ToHex(nr, ng, nb);
    }

    public static string Desaturate(string hexColor, double amount)
    {
        var (r, g, b) = ParseHex(hexColor);
        var (h, s, l) = RgbToHsl(r, g, b);
        s = Math.Max(0.0, s - amount);
        var (nr, ng, nb) = HslToRgb(h, s, l);
        return ToHex(nr, ng, nb);
    }

    private static (int r, int g, int b) ParseHex(string hex)
    {
        hex = hex.TrimStart('#');
        var r = Convert.ToInt32(hex[0..2], 16);
        var g = Convert.ToInt32(hex[2..4], 16);
        var b = Convert.ToInt32(hex[4..6], 16);
        return (r, g, b);
    }

    private static string ToHex(int r, int g, int b) =>
        $"#{r:X2}{g:X2}{b:X2}";

    private static (double h, double s, double l) RgbToHsl(int r, int g, int b)
    {
        var rf = r / 255.0;
        var gf = g / 255.0;
        var bf = b / 255.0;

        var max = Math.Max(rf, Math.Max(gf, bf));
        var min = Math.Min(rf, Math.Min(gf, bf));
        var l = (max + min) / 2.0;

        if (max == min)
            return (0, 0, l);

        var d = max - min;
        var s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

        double h;
        if (max == rf)
            h = (gf - bf) / d + (gf < bf ? 6 : 0);
        else if (max == gf)
            h = (bf - rf) / d + 2;
        else
            h = (rf - gf) / d + 4;

        h /= 6.0;
        return (h, s, l);
    }

    private static (int r, int g, int b) HslToRgb(double h, double s, double l)
    {
        if (s == 0)
        {
            var grey = (int)(l * 255);
            return (grey, grey, grey);
        }

        var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
        var p = 2 * l - q;

        var r = (int)(HueToRgb(p, q, h + 1.0 / 3) * 255);
        var g = (int)(HueToRgb(p, q, h) * 255);
        var bv = (int)(HueToRgb(p, q, h - 1.0 / 3) * 255);

        return (r, g, bv);
    }

    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1.0 / 6) return p + (q - p) * 6 * t;
        if (t < 1.0 / 2) return q;
        if (t < 2.0 / 3) return p + (q - p) * (2.0 / 3 - t) * 6;
        return p;
    }
}
