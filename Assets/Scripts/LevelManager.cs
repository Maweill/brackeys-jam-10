using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private CameraController _cameraController;

	private void Start()
	{
		_cameraController = FindObjectOfType<CameraController>();

		if (_cameraController == null) {
			Debug.LogWarning("LevelManager: CameraController не найден, _cameraController = null");
		}

		_cameraController.OnCameraMovedDown += HandleCameraMovedDown;
		GameEvents.GameStarted += OnGameStarted;
	}

	public void OnLevelCompleted()
	{
		GameEvents.InvokeLevelCompleted();
		
		if (_cameraController == null) {
			return;
		}

		_cameraController.MoveCameraDown();
	}
	
	private void OnGameStarted()
	{
		if (_cameraController == null) {
			return;
		}

		_cameraController.MoveCameraDown();
	}

	private void HandleCameraMovedDown()
	{
		Debug.Log("Камера переместилась вниз.");
	}

	private void OnDestroy()
	{
		if (_cameraController == null) {
			return;
		}

		_cameraController.OnCameraMovedDown -= HandleCameraMovedDown;
	}
}
