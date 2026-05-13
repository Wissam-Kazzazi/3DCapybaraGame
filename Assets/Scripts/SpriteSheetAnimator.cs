using UnityEngine;
using UnityEngine.UI;

// animates a sprite sheet on a UI RawImage
public class SpriteSheetAnimator : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] int columns = 5;
    [SerializeField] int rows = 21;
    [SerializeField] int totalFrames = 101;
    [SerializeField] float fps = 24f;

    float _timer;
    int _currentFrame;

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1f / fps)
        {
            _timer = 0f;
            _currentFrame = (_currentFrame + 1) % totalFrames;
            UpdateFrame();
        }
    }

    void UpdateFrame()
    {
        int col = _currentFrame % columns;
        int row = _currentFrame / columns;

        float frameW = 1f / columns;
        float frameH = 1f / rows;

        rawImage.uvRect = new Rect(col * frameW, 1f - (row + 1) * frameH, frameW, frameH);
    }
}