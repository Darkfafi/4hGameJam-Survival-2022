using UnityEngine;

public class SimplePulseAnimation : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float _minScale = 0.5f;

    [SerializeField]
    private float _speed = 1f;

    protected void Update()
    {
        transform.localScale = Vector2.one * (_minScale + Mathf.Sin(Time.time * _speed) * (1f - _minScale));
    }
}