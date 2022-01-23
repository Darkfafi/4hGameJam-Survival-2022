using System.Collections;
using UnityEngine;

public class PulseAnimation : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float _minScale = 0.5f;

    [SerializeField]
    private float _duration = 1f;

    [SerializeField]
    private float _delay = 0f;

    [SerializeField]
    private bool _loop = true;

    [SerializeField]
    private bool _startOnAwake = true;

    [Header("Multi Targets Options")]
    [SerializeField, Tooltip("Optional")]
    private Transform[] _targets = null;
    [SerializeField]
    private float _delayBetweenTargets = 0f;

    protected void Awake()
    {
        if (_startOnAwake)
        {
            Pulse();
        }
    }

    public void Pulse()
    {
        CancelPulse();
        StartCoroutine(StartPulseRoutine());
    }

    public void CancelPulse()
    {
        ApplyScale(Vector2.one);
        StopAllCoroutines();
    }

    private IEnumerator StartPulseRoutine()
    {
        if (_targets.Length > 0)
        {
            for (int i = 0; i < _targets.Length; i++)
            {
                StartCoroutine(PulseRoutine(_targets[i]));
                yield return new WaitForSeconds(_delayBetweenTargets);
            }
        }
        else
        {
            StartCoroutine(PulseRoutine(transform));
            yield return null;
        }
    }

    private IEnumerator PulseRoutine(Transform target)
    {
        float t = 0f;
        yield return new WaitForSeconds(_delay);
        while (t < _duration && _duration > 0f)
        {
            float progress = t / _duration;
            target.localScale = Vector2.one * (_minScale + Mathf.Cos((Mathf.PI * 2) * progress) * (1f - _minScale));
            t = Mathf.Clamp(t + Time.deltaTime, 0f, _duration);
            if (_loop && Mathf.Approximately(t, _duration))
            {
                t = 0f;
                yield return new WaitForSeconds(_delay);
            }
            yield return null;
        }
        CancelPulse();
    }

    private void ApplyScale(Vector2 scale)
    {
        if (_targets.Length > 0)
        {
            for (int i = 0; i < _targets.Length; i++)
            {
                _targets[i].localScale = scale;
            }
        }
        else
        {
            transform.localScale = scale;
        }
    }
}