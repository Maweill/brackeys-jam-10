using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private float _shiftDuration;

    private Vector3 _currentPosition;

    private bool _isMoving;
    
    public event Action OnCameraMovedDown;
    
    public void MoveCameraDown()
    {
        if (_isMoving)
            Debug.LogWarning("CameraController: Попытка свдинуть камеру когда она ещё двигается, _isMoving = true");
        
        _isMoving = true;
        
        _currentPosition = transform.position;
        Vector3 shiftPosition = new Vector3(_currentPosition.x, _currentPosition.y - GlobalConstants.LEVEL_SHIFT, _currentPosition.z);
        
        transform.DOMove(shiftPosition, _shiftDuration, false)
            .OnComplete(() =>
            {
                _isMoving = false;
                OnCameraMovedDown?.Invoke();
            });
    }
}
