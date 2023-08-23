using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private CameraController _cameraController;

	private int _filledHouses;

	private void Start()
	{
		GameEvents.LevelCompleted += OnLevelCompleted;
		_cameraController = FindObjectOfType<CameraController>();

		if (_cameraController == null) {
			Debug.LogWarning("LevelManager: CameraController не найден, _cameraController = null");
		}

		_cameraController.OnCameraMovedDown += HandleCameraMovedDown;
		GameEvents.GameStarted += OnGameStarted;
	}

	private void OnLevelCompleted()
	{
		HouseTemplate[] allHouses = FindObjectsOfType<HouseTemplate>();

		_filledHouses = allHouses.Count(house => house.IsHouseFilled);
		
		Debug.Log("LevelManager: Количество установленных домов:" + _filledHouses);

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
