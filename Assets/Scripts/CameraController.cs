using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private float _shiftDuration;

    private Vector3 _currentPosition;

    private bool _isMoving;
    
    public void MoveCameraDown(Action callback = null)
    {
        if (_isMoving) {
            Debug.LogWarning("CameraController: Попытка свдинуть камеру когда она ещё двигается");
            return;
        }
        _isMoving = true;
        
        _currentPosition = transform.position;
        Vector3 shiftPosition = new(_currentPosition.x, _currentPosition.y - GlobalConstants.LEVEL_SHIFT, _currentPosition.z);
        
        transform.DOMove(shiftPosition, _shiftDuration)
            .OnComplete(() =>
            {
                _isMoving = false;
                callback?.Invoke();
            });
    }
}
