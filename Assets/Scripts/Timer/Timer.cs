using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public FloatVariable timer;
    public GameObject objectToRotate;

    private int _currentTime;
    private bool _resetTimer;
    private float _initialValue;
    private float _angle;
    private Vector3 _basePostion;

    private bool _playedEndSFX;
    
    // Start is called before the first frame update
    void Start()
    {
        _initialValue = timer.InitialValue;
        _currentTime = Mathf.RoundToInt(_initialValue);
        _basePostion = transform.position;

        _angle = 360.0f /  timer.InitialValue;

        _playedEndSFX = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_resetTimer)
        {
            timer.Value -= Time.deltaTime;

            int roundValue = Mathf.RoundToInt(timer.Value);
            if (roundValue != _currentTime)
            {
                _currentTime = roundValue;
                objectToRotate.transform
                    .DORotate(new Vector3(0, 0, -_angle * (Mathf.RoundToInt(_initialValue) - _currentTime)), 0.5f)
                    .SetEase(Ease.OutBounce);
            }

            if (timer.Value < .0f)
            {
                _resetTimer = true;
                int angleCount = 30;
                float resetAngle = 360.0f / angleCount;
                float resetTime = 1.0f / angleCount;

                Sequence rewindSequence = DOTween.Sequence();

                for (int i = 0; i <= angleCount; i++)
                {
                    rewindSequence.Append(objectToRotate.transform.DORotate(new Vector3(0,0, resetAngle * i), resetTime).SetEase(Ease.InOutBounce));
                    rewindSequence.Join(transform.DOShakePosition(resetTime, 2.0f));
                }
                rewindSequence.AppendCallback(ResetTimer);
            }

            if (timer.Value < 5 && !_playedEndSFX)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clock/5secBeforeEnd");
                _playedEndSFX = true;
            }
                
        }
    }

    void ResetTimer()
    {
        timer.Value = timer.InitialValue;
        _resetTimer = false;
       // transform.position = _basePostion;
    }

    void ShakeObject()
    {
        transform.DOShakePosition(0.2f, 1.0f);
    }

}
