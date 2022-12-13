using UnityEngine;

public class HoverSpin : MonoBehaviour
{
    [SerializeField]
    private float _hoverMagnitude = 1;

    [SerializeField]
    private float _hoverSpeed = 1;

    [SerializeField]
    private float _spinSpeed = 10;

    void Update()
    {
        transform.localEulerAngles = Vector3.forward * Time.time * _spinSpeed;
        transform.localPosition = Vector3.forward * Mathf.Sin(Time.time * _hoverSpeed) * _hoverMagnitude;
    }
}
