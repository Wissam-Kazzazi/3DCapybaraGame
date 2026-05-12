using UnityEngine;
using System.Collections;

/// <summary>
/// Stand-in for VideoPlayer until real video clips arrive.
/// When Play() is called, animates the screen material through a color cycle
/// and plays a sine-wave tone via the AudioSource. Stop() reverses it.
/// Drop this on the Screen GameObject (sibling of VideoPlayer + AudioSource).
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class MockVideoScreen : MonoBehaviour
{
    [Header("Look")]
    [Tooltip("Color this screen pulses to when 'playing'. Different per room to tell them apart.")]
    public Color tintColor = Color.white;

    [Tooltip("Cycles per second of the color pulse.")]
    public float pulseSpeed = 0.7f;

    [Header("Sound")]
    [Tooltip("Frequency of the sine-wave tone (Hz). Different per room.")]
    public float toneFrequency = 440f;

    [Tooltip("Tone volume 0-1.")]
    [Range(0f, 1f)] public float toneVolume = 0.4f;

    [Header("Runtime (read-only)")]
    [SerializeField] bool _isPlaying;

    MeshRenderer _mr;
    Material _matInstance;
    AudioSource _audio;
    Color _restColor = new Color(0.05f, 0.05f, 0.05f, 1f); // dim when off
    Coroutine _pulseRoutine;

    void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        // get a unique material instance so multiple screens don't share color
        _matInstance = _mr.material;
        _audio = GetComponent<AudioSource>();

        // build a 1-second looping sine tone matching toneFrequency
        if (_audio != null)
        {
            _audio.clip = BuildSineClip(toneFrequency, 1f);
            _audio.loop = true;
            _audio.playOnAwake = false;
            _audio.volume = toneVolume;
        }

        ApplyColor(_restColor);
    }

    public void Play()
    {
        if (_isPlaying) return;
        _isPlaying = true;
        if (_audio != null) _audio.Play();
        if (_pulseRoutine != null) StopCoroutine(_pulseRoutine);
        _pulseRoutine = StartCoroutine(PulseLoop());
    }

    public void Stop()
    {
        if (!_isPlaying) return;
        _isPlaying = false;
        if (_audio != null) _audio.Stop();
        if (_pulseRoutine != null) { StopCoroutine(_pulseRoutine); _pulseRoutine = null; }
        ApplyColor(_restColor);
    }

    IEnumerator PulseLoop()
    {
        float t = 0f;
        while (_isPlaying)
        {
            t += Time.deltaTime * pulseSpeed * Mathf.PI * 2f;
            float k = (Mathf.Sin(t) + 1f) * 0.5f; // 0..1
            ApplyColor(Color.Lerp(tintColor * 0.3f, tintColor, k));
            yield return null;
        }
    }

    void ApplyColor(Color c)
    {
        if (_matInstance == null) return;
        if (_matInstance.HasProperty("_BaseColor")) _matInstance.SetColor("_BaseColor", c);
        else _matInstance.color = c;
    }

    /// <summary>Generate a short looping sine wave AudioClip.</summary>
    static AudioClip BuildSineClip(float frequency, float seconds)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(seconds * sampleRate);
        // Round sampleCount to whole cycles so the loop has no click.
        float samplesPerCycle = sampleRate / frequency;
        sampleCount = Mathf.RoundToInt(sampleCount / samplesPerCycle) * (int)samplesPerCycle;
        if (sampleCount < 1) sampleCount = sampleRate;

        var samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate);

        var clip = AudioClip.Create("MockTone_" + frequency.ToString("F0") + "Hz", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
