using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _currentPosition;

    private bool _isMoving;
    
    public void MoveCamera(float y, Action callback = null)
    {
        if (_isMoving) {
            Debug.LogWarning("CameraController: Попытка свдинуть камеру когда она ещё двигается");
            return;
        }
        _isMoving = true;
        
        _currentPosition = transform.position;
        Vector3 shiftPosition = new(_currentPosition.x, y, _currentPosition.z);
        
        transform.DOMove(shiftPosition,GlobalConstants.CAMERA_SHIFT_DURATION)
            .OnComplete(() =>
            {
                _isMoving = false;
                callback?.Invoke();
                GameEvents.InvokeCameraMoved();
            });
    }
}
