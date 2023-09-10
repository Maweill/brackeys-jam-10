using System;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _currentPosition;

    public bool IsMoving { get; private set; }

    public void MoveCamera(float y, Action callback = null)
    {
        if (IsMoving) {
            Debug.LogWarning("CameraController: Попытка свдинуть камеру когда она ещё двигается");
            return;
        }
        IsMoving = true;
        
        _currentPosition = transform.position;
        Vector3 shiftPosition = new(_currentPosition.x, y, _currentPosition.z);
        
        transform.DOMove(shiftPosition,GlobalConstants.CAMERA_SHIFT_DURATION)
            .OnComplete(() =>
            {
                IsMoving = false;
                callback?.Invoke();
                GameEvents.InvokeCameraMoved();
            });
    }
}
