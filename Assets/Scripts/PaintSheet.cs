using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PaintSheet : MonoBehaviour
{
    public float pixelsPerUnit = 100f;
    public Color32 background = Color.white;
    public int brushRadius = 4;

    public Texture2D mask;      // R = black, G = blur, B = grayscale, transparent (A < 255) = invert
    public int blurRadius = 4;
    public byte threshold = 12; // channel counts as "on" above this

    static readonly Color32[] palette =
    {
        new Color32(0, 0, 0, 255),       // 1 black
        new Color32(255, 0, 0, 255),     // 2 red
        new Color32(255, 127, 0, 255),   // 3 orange
        new Color32(255, 255, 0, 255),   // 4 yellow
        new Color32(0, 200, 0, 255),     // 5 green
        new Color32(0, 255, 255, 255),   // 6 cyan
        new Color32(0, 0, 255, 255),     // 7 blue
        new Color32(148, 0, 211, 255),   // 8 purple
        new Color32(139, 69, 19, 255),   // 9 brown
    };

    Texture2D texture;
    Color32[] pixels;
    BoxCollider2D box;
    int width;
    int height;
    Color32 color = new Color32(0, 0, 0, 255);
    Vector2Int? lastPixel;
    float holdTime;
    float radiusF;
    bool maskOn;
    Color32[] maskPixels;
    Color32[] shown;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        width = Mathf.Max(1, Mathf.RoundToInt(box.size.x * pixelsPerUnit));
        height = Mathf.Max(1, Mathf.RoundToInt(box.size.y * pixelsPerUnit));

        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        pixels = new Color32[width * height];
        Clear();

        // Pivot shifted by the collider offset so the sprite sits exactly on the box.
        Vector2 pivot = new Vector2(0.5f - box.offset.x / box.size.x, 0.5f - box.offset.y / box.size.y);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), pivot, pixelsPerUnit);
    }

    public void Clear()
    {
        for (int i = 0; i < pixels.Length; i++) pixels[i] = background;
        Upload();
    }

    public void SetMask(bool on)
    {
        maskOn = on && mask != null;
        Upload();
    }

    public void ToggleMask() { SetMask(!maskOn); }

    // Pushes the drawing to the texture, running it through the mask if it's on.
    void Upload()
    {
        if (!maskOn)
        {
            texture.SetPixels32(pixels);
            texture.Apply();
            return;
        }

        if (maskPixels == null) maskPixels = mask.GetPixels32();
        if (shown == null) shown = new Color32[pixels.Length];
        int mw = mask.width, mh = mask.height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Color32 m = maskPixels[(y * mh / height) * mw + (x * mw / width)];
                Color32 c = pixels[y * width + x];

                if (m.g > threshold) c = Blur(x, y);
                if (m.b > threshold)
                {
                    byte gray = (byte)(c.r * 0.299f + c.g * 0.587f + c.b * 0.114f);
                    c = new Color32(gray, gray, gray, c.a);
                }
                if (m.a < 255 - threshold) c = new Color32((byte)(255 - c.r), (byte)(255 - c.g), (byte)(255 - c.b), c.a);
                if (m.r > threshold) c = new Color32(0, 0, 0, 255);

                shown[y * width + x] = c;
            }
        }
        texture.SetPixels32(shown);
        texture.Apply();
    }

    Color32 Blur(int cx, int cy)
    {
        int r = 0, g = 0, b = 0, a = 0, n = 0;
        for (int y = Mathf.Max(0, cy - blurRadius); y <= Mathf.Min(height - 1, cy + blurRadius); y++)
        {
            for (int x = Mathf.Max(0, cx - blurRadius); x <= Mathf.Min(width - 1, cx + blurRadius); x++)
            {
                Color32 c = pixels[y * width + x];
                r += c.r; g += c.g; b += c.b; a += c.a; n++;
            }
        }
        return new Color32((byte)(r / n), (byte)(g / n), (byte)(b / n), (byte)(a / n));
    }

    void Update()
    {
        HandleKeys();

        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        Vector2 world = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
        if (!mouse.leftButton.isPressed || !box.OverlapPoint(world))
        {
            lastPixel = null;
            return;
        }

        Vector2Int pixel = WorldToPixel(world);
        if (lastPixel == null) Stamp(pixel);
        else Line(lastPixel.Value, pixel);
        lastPixel = pixel;

        Upload();
    }

    void HandleKeys()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        Key[] digits = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9 };
        for (int i = 0; i < digits.Length; i++)
            if (kb[digits[i]].wasPressedThisFrame) color = palette[i];
        if (kb.digit0Key.wasPressedThisFrame) color = background; // erase

        int dir = (kb.rightBracketKey.isPressed ? 1 : 0) - (kb.leftBracketKey.isPressed ? 1 : 0);
        if (dir == 0)
        {
            holdTime = 0f;
            return;
        }

        // Tap = 1 step. Hold = speeds up the longer it's held.
        if (holdTime == 0f) radiusF = brushRadius + dir;
        else if (holdTime > 0.25f) radiusF += dir * (5f + holdTime * 30f) * Time.deltaTime;
        holdTime += Time.deltaTime;
        radiusF = Mathf.Max(1f, radiusF);
        brushRadius = Mathf.RoundToInt(radiusF);
    }

    Vector2Int WorldToPixel(Vector2 world)
    {
        Vector2 local = (Vector2)transform.InverseTransformPoint(world) - (box.offset - box.size / 2f);
        int x = Mathf.FloorToInt(local.x / box.size.x * width);
        int y = Mathf.FloorToInt(local.y / box.size.y * height);
        return new Vector2Int(x, y);
    }

    void Line(Vector2Int a, Vector2Int b)
    {
        int steps = Mathf.Max(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y), 1);
        for (int i = 0; i <= steps; i++)
        {
            Vector2 p = Vector2.Lerp(a, b, i / (float)steps);
            Stamp(new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y)));
        }
    }

    void Stamp(Vector2Int c)
    {
        int r = brushRadius;
        for (int y = -r; y <= r; y++)
        {
            for (int x = -r; x <= r; x++)
            {
                if (x * x + y * y > r * r) continue;
                int px = c.x + x, py = c.y + y;
                if (px < 0 || px >= width || py < 0 || py >= height) continue;
                pixels[py * width + px] = color;
            }
        }
    }
}
