using System.Collections.Generic;
using System.Linq;
using House_Scripts;
using UnityEngine;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private List<LevelManager> _levels;
	
		private CameraController _cameraController;

		private int _filledHouses;
		private int _currentLevelIndex;

		private void Start()
		{
			_currentLevelIndex = -1;
			GameEvents.LevelCompleted += OnLevelCompleted;
			_cameraController = FindObjectOfType<CameraController>();
			GameEvents.GameStarted += OnGameStarted;
		}
	
		private void OnGameStarted()
		{
			StartNextLevel();
		}

		private void OnLevelCompleted()
		{
			HouseTemplate[] allHouses = FindObjectsOfType<HouseTemplate>();
			_filledHouses = allHouses.Count(house => house.IsHouseFilled);
			Debug.Log("LevelManager: Количество установленных домов:" + _filledHouses);
			StartNextLevel();
		}
	
		private void StartNextLevel()
		{
			_currentLevelIndex++;
			if (_currentLevelIndex >= _levels.Count) {
				GameEvents.InvokeGameCompleted();
				return;
			}
			if (_currentLevelIndex > 0) {
				_levels[_currentLevelIndex - 1].EndLevel();
			}
			_cameraController.MoveCameraDown();
			_levels[_currentLevelIndex].StartLevel();
		}
	}
}
