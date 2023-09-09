using DG.Tweening;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField]
    Transform _engineSpriteContainer;
    
    private float _shakeDuration = 4f;
    private float _shakeAmplitude = 0.15f;

    public void PlayEngineWinAnimation()
    {
        _engineSpriteContainer.transform.DOShakePosition(_shakeDuration, _shakeAmplitude);
    }
}
