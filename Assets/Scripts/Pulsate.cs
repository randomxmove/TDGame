using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsate : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] _renderers = null;

    [SerializeField]
    private string _propertyName = "";

    [SerializeField]
    private float _frequency = 3;

    [SerializeField]
    private float _valueScale = 50;

    [SerializeField]
    private float _startValue = 10;

    private void Update()
    {
        foreach (var renderer in _renderers)
        {
            var mat = renderer.material;
            mat.SetFloat(_propertyName, (Mathf.Sin(Time.time * _frequency) + 1) * 0.5f * _valueScale + _startValue);
            renderer.material = mat;
        }
    }
}
