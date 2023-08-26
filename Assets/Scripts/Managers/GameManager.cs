using System.Collections;
using System.Collections.Generic;
using System.Linq;
using House_Scripts;
using UnityEngine;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		private const float MAIN_MENU_CAMERA_Y = 0f;
		
		[SerializeField]
		private MainMenu _mainMenu;
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
			if (_currentLevelIndex > 0) {
				_levels[_currentLevelIndex - 1].EndLevel();
			}

			if (_currentLevelIndex >= _levels.Count) {
				StartCoroutine(EndGame());
				return;
			}
			_cameraController.MoveCamera(_levels[_currentLevelIndex].transform.position.y - 5f);
			_levels[_currentLevelIndex].StartLevel();
		}

		private IEnumerator EndGame()
		{
			// Скрыть кнопки из главного меню
			_mainMenu.HideButtons();
			
			// Подсчитывать дома, которые заполнены, поднимаясь выше на каждый уровень, и считать общее количество
			// Подсвечивать дома, которые заполнены
			int filledHousesTotal = 0;
			int housesTotal = 0;
			for (int i = _levels.Count - 1; i >= 0; i--) {
				_cameraController.MoveCamera(_levels[i].transform.position.y - 5f);
				yield return new WaitForSeconds(GlobalConstants.CAMERA_SHIFT_DURATION);
				filledHousesTotal += _levels[i].CountFilledHouses();
				housesTotal += _levels[i].CountHouses();
				yield return new WaitForSeconds(filledHousesTotal * 1.5f);
			}
			
			// Поднять камеру вверх, показать город
			_cameraController.MoveCamera(MAIN_MENU_CAMERA_Y);
			yield return new WaitForSeconds(GlobalConstants.CAMERA_SHIFT_DURATION);
			
			float houseFillPercentage = (float) filledHousesTotal / housesTotal;
			if ( houseFillPercentage >= GlobalConstants.MIN_FILLED_HOUSES_PERCENTAGE) {
				//TODO Зажечь свет города
				Debug.Log("GameManager: Зажечь свет города");
			} else {
				//TODO Погрузить город в темноту
				Debug.Log("GameManager: Погрузить город в темноту");
			}
		}
	}
}
