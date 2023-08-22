using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelManager : MonoBehaviour
{
    private CameraController _cameraController;

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();

        if (_cameraController == null) 
            Debug.LogWarning("LevelManager: CameraController не найден, _cameraController = null");
        
        _cameraController.OnCameraMovedDown += HandleCameraMovedDown;
    }
    
    public void OnLevelCompleted()
    {
        if (_cameraController == null) return;
        _cameraController.MoveCameraDown();
    }
    
    private void HandleCameraMovedDown()
    {
        Debug.Log("Камера переместилась вниз.");
    }

    private void OnDestroy()
    {
        if (_cameraController == null) return;
        _cameraController.OnCameraMovedDown -= HandleCameraMovedDown;
    }
}
