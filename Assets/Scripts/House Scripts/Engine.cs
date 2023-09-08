using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Engine : MonoBehaviour
{
    private float _shakeDuration = 4f;
    private float _shakeAmplitude = 0.15f;

    public void PlayEngineWinAnimation()
    {
        transform.DOShakePosition(_shakeDuration, _shakeAmplitude);
    }

}
