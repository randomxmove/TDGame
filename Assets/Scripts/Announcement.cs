using UnityEngine;
using UnityEngine.UI;

public class Announcement : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    [SerializeField]
    private AnimationCurve _alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private float _remainingShowTime = 0;
    private float _totalShowTime = 0;

    public void Show(string text, float duration)
    {
        _text.text = text;
        _remainingShowTime = duration;
        _totalShowTime = duration;
        gameObject.SetActive(true);
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }

    private void Update()
    {
        _remainingShowTime -= Time.deltaTime;
        _canvasGroup.alpha = _alphaCurve.Evaluate(1 - (_remainingShowTime / _totalShowTime)); 
        if (_remainingShowTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
