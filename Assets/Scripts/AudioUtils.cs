using UnityEngine;

public static class AudioUtils
{
    public static AudioClip BuildDingClip()
    {
        int sampleRate = 44100;
        int count = (int)(sampleRate * 0.4f);
        float[] samples = new float[count];
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = Mathf.Exp(-t * 8f);
            samples[i] = envelope * (
                0.7f * Mathf.Sin(2f * Mathf.PI * 880f  * t) +
                0.3f * Mathf.Sin(2f * Mathf.PI * 1760f * t)
            );
        }
        var clip = AudioClip.Create("Ding", count, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    public static Texture2D BuildCheckmarkTexture()
    {
        int size = 64;
        var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point;

        // Fill transparent
        Color clear = new Color(0f, 0f, 0f, 0f);
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                tex.SetPixel(x, y, clear);

        // Bright green with yellow noise for "deep-fried" look
        Color green  = new Color(0.1f, 1f, 0.1f, 1f);
        Color yellow = new Color(1f, 1f, 0f, 1f);
        Color white  = new Color(1f, 1f, 1f, 1f);

        // Draw thick checkmark: left leg goes down-right from (10,35) to (25,18),
        // right leg goes up-right from (25,18) to (54,52). Thickness 5px via offsets.
        void DrawThickLine(int x0, int y0, int x1, int y1, int thickness)
        {
            int steps = Mathf.Max(Mathf.Abs(x1 - x0), Mathf.Abs(y1 - y0)) * 3;
            for (int s = 0; s <= steps; s++)
            {
                float f = (float)s / steps;
                int cx = Mathf.RoundToInt(Mathf.Lerp(x0, x1, f));
                int cy = Mathf.RoundToInt(Mathf.Lerp(y0, y1, f));
                for (int dy = -thickness; dy <= thickness; dy++)
                    for (int dx = -thickness; dx <= thickness; dx++)
                        if (dx * dx + dy * dy <= thickness * thickness)
                        {
                            int px = cx + dx;
                            int py = cy + dy;
                            if (px >= 0 && px < size && py >= 0 && py < size)
                            {
                                // Random noise pixel
                                float r = Random.value;
                                Color c = r < 0.75f ? green : (r < 0.88f ? yellow : white);
                                tex.SetPixel(px, py, c);
                            }
                        }
            }
        }

        DrawThickLine(10, 35, 25, 18, 3); // left downward stroke
        DrawThickLine(25, 18, 54, 52, 3); // right upward stroke

        // Scatter a few noise dots around the mark for deep-fried compression artifact feel
        for (int i = 0; i < 80; i++)
        {
            int nx = Random.Range(0, size);
            int ny = Random.Range(0, size);
            if (tex.GetPixel(nx, ny).a > 0f)
            {
                float r = Random.value;
                tex.SetPixel(nx, ny, r < 0.5f ? yellow : white);
            }
        }

        tex.Apply();
        return tex;
    }
}
