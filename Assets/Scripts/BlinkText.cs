using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkSpeed = 1f;
    TMP_Text _text;

    void Awake() => _text = GetComponent<TMP_Text>();

    void Update()
    {
        float alpha = Mathf.Round(Mathf.PingPong(Time.time * blinkSpeed, 1f));
        Color c = _text.color;
        c.a = alpha;
        _text.color = c;
    }
}