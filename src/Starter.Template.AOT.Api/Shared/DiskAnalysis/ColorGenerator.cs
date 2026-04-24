namespace Starter.Template.AOT.Api.Shared.DiskAnalysis;

public static class ColorGenerator
{
    private static readonly List<string> PredefinedColors =
    [
        "#D32F2F", "#1976D2", "#388E3C", "#AB47BC", "#26C6DA",
        "#757575", "#BDBDBD", "#5D4037", "#2E7D32", "#1565C0",
        "#AFB42B", "#7B1FA2", "#00897B", "#BDBDBD", "#F44336",
        "#E91E63", "#9C27B0", "#673AB7", "#3F51B5", "#2196F3",
        "#03A9F4", "#00BCD4", "#009688", "#4CAF50", "#8BC34A",
        "#CDDC39", "#FFEB3B", "#FFC107", "#FF9800", "#FF5722",
        "#795548", "#9E9E9E", "#607D8B", "#E53935", "#D81B60",
        "#8E24AA", "#5E35B1", "#3949AB", "#1E88E5", "#039BE5",
        "#00ACC1", "#00897B", "#43A047", "#7CB342", "#C0CA33"
    ];

    public static void ApplyColorsToStructure(DiskItemEntity structure)
    {
        if (structure.Children is null)
            return;

        var totalDescendants = CountMaxDepth(structure.Children);

        for (var i = 0; i < structure.Children.Count; i++)
        {
            AddColorToChildren(structure.Children[i], GenerateBaseColor(i), totalDescendants);
        }
    }

    private static void AddColorToChildren(DiskItemEntity item, string color, int totalDescendants, int depth = 1)
    {
        var percentage = depth / (totalDescendants * 1.2) * 80;

        item.Color = InterpolateToGrey(color, percentage);

        if (item.Children is null || item.Children.Count == 0)
        {
            item.Color = Desaturate(item.Color, 80.0);
            return;
        }

        var childIndex = 0;

        foreach (var child in item.Children)
        {
            var saturationAmount = childIndex % 2 == 0 ? 10 : 30;

            AddColorToChildren(child, Saturate(item.Color, saturationAmount), totalDescendants, depth + 1);

            childIndex++;
        }
    }

    private static string GenerateBaseColor(int index)
    {
        const double lighterPercentage = 0.4;
        var hex = PredefinedColors[index % PredefinedColors.Count];

        ParseHexColor(hex, out var r, out var g, out var b);

        r = Math.Min(r + (int)(lighterPercentage * 255), 255);
        g = Math.Min(g + (int)(lighterPercentage * 255), 255);
        b = Math.Min(b + (int)(lighterPercentage * 255), 255);

        return $"#{r:X2}{g:X2}{b:X2}";
    }

    private static int CountMaxDepth(List<DiskItemEntity> items, int currentDepth = 0)
    {
        if (items is null || items.Count == 0)
            return currentDepth;

        return items.Max(item => CountMaxDepth(item.Children!, currentDepth + 1));
    }

    public static string InterpolateToGrey(string hexColor, double percentage)
    {
        ParseHexColor(hexColor, out var r, out var g, out var b);

        var grey = (r + g + b) / 3.0;

        var newR = (int)(r + (grey - r) * percentage / 100);
        var newG = (int)(g + (grey - g) * percentage / 100);
        var newB = (int)(b + (grey - b) * percentage / 100);

        return $"#{newR:X2}{newG:X2}{newB:X2}";
    }

    public static string Saturate(string hexColor, double percentage)
    {
        ParseHexColor(hexColor, out var r, out var g, out var b);

        RgbToHsl(r, g, b, out var h, out var s, out var l);

        s += (1 - s) * (percentage / 100.0);

        HslToRgb(h, s, l, out var newR, out var newG, out var newB);

        return $"#{(int)newR:X2}{(int)newG:X2}{(int)newB:X2}";
    }

    public static string Desaturate(string hexColor, double percentage)
    {
        ParseHexColor(hexColor, out var r, out var g, out var b);

        RgbToHsl(r, g, b, out var h, out var s, out var l);

        s -= s * (percentage / 100.0);

        HslToRgb(h, s, l, out var newR, out var newG, out var newB);

        return $"#{(int)newR:X2}{(int)newG:X2}{(int)newB:X2}";
    }

    private static void ParseHexColor(string hex, out int r, out int g, out int b)
    {
        var cleanHex = hex.TrimStart('#');
        r = Convert.ToInt32(cleanHex[..2], 16);
        g = Convert.ToInt32(cleanHex[2..4], 16);
        b = Convert.ToInt32(cleanHex[4..6], 16);
    }

    private static void RgbToHsl(int r, int g, int b, out double h, out double s, out double l)
    {
        var rNorm = r / 255.0;
        var gNorm = g / 255.0;
        var bNorm = b / 255.0;

        var max = Math.Max(rNorm, Math.Max(gNorm, bNorm));
        var min = Math.Min(rNorm, Math.Min(gNorm, bNorm));

        l = (max + min) / 2.0;

        if (max == min)
        {
            h = s = 0;
        }
        else
        {
            var d = max - min;
            s = l > 0.5 ? d / (2.0 - max - min) : d / (max + min);

            if (max == rNorm)
                h = (gNorm - bNorm) / d + (gNorm < bNorm ? 6 : 0);
            else if (max == gNorm)
                h = (bNorm - rNorm) / d + 2;
            else
                h = (rNorm - gNorm) / d + 4;

            h /= 6;
        }
    }

    private static void HslToRgb(double h, double s, double l, out double r, out double g, out double b)
    {
        if (s == 0)
        {
            r = g = b = l;
        }
        else
        {
            var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            var p = 2 * l - q;

            r = HueToRgb(p, q, h + 1 / 3.0);
            g = HueToRgb(p, q, h);
            b = HueToRgb(p, q, h - 1 / 3.0);
        }

        r = Math.Round(r * 255);
        g = Math.Round(g * 255);
        b = Math.Round(b * 255);
    }

    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1 / 6.0) return p + (q - p) * 6 * t;
        if (t < 1 / 2.0) return q;
        if (t < 2 / 3.0) return p + (q - p) * (2 / 3.0 - t) * 6;
        return p;
    }
}
