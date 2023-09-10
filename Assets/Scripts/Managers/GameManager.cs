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
		private const float TUTORIAL_CAMERA_Y = -10.5f;
		
		[SerializeField]
		private MainMenu _mainMenu;
		[SerializeField]
		private List<LevelManager> _levels;
		[SerializeField]
		private AudioClip _winSound;
		[SerializeField]
		private AudioClip _loseSound;
		[SerializeField]
		private LoadingScreen _loadingScreen;
	
		private CameraController _cameraController;
		private AudioSource _audioSource;

		private int _filledHouses;
		private int _currentLevelIndex;
		private int _filledHousesTotal;
		private int _housesTotal;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}

		private void Start()
		{
			_currentLevelIndex = -1;
			GameEvents.LevelCompleted += OnLevelCompleted;
			_cameraController = FindObjectOfType<CameraController>();
			GameEvents.GameStarted += OnGameStarted;
			GameEvents.TutorialSkipped += OnTutorialSkipped;
		}
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.R)) {
				StartCoroutine(RestartCurrentLevel());
			}
		}

		private void OnDisable()
		{
			GameEvents.LevelCompleted -= OnLevelCompleted;
			GameEvents.GameStarted -= OnGameStarted;
		}

		private void OnGameStarted()
		{
			StartCoroutine(StartGame());
		}

		private void OnLevelCompleted()
		{
			HouseTemplate[] allHouses = FindObjectsOfType<HouseTemplate>();
			_filledHouses = allHouses.Count(house => house.IsHouseFilled);
			Debug.Log("LevelManager: Количество установленных домов:" + _filledHouses);
			StartNextLevel();
		}

		private IEnumerator StartGame()
		{
			_cameraController.MoveCamera(TUTORIAL_CAMERA_Y);
			yield return new WaitForSeconds(3f);
		}

		private void OnTutorialSkipped()
		{
			if (_cameraController.IsMoving) {
				return;
			}
			StartNextLevel();
			GameEvents.TutorialSkipped -= OnTutorialSkipped;
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

		private IEnumerator RestartCurrentLevel()
		{
			_loadingScreen.Show();
			yield return new WaitForSeconds(2f);
			_levels[_currentLevelIndex].EndLevel();
			_levels[_currentLevelIndex].StartLevel();
			_loadingScreen.Hide();
		}

		private IEnumerator EndGame()
		{
			// Скрыть кнопки из главного меню
			_mainMenu.HideButtons();
			
			// Подсчитывать дома, которые заполнены, поднимаясь выше на каждый уровень, и считать общее количество
			// Подсвечивать дома, которые заполнены
			int filledHousesTotal = 0;
			int housesTotal = 0;
			_filledHousesTotal = filledHousesTotal;
			_housesTotal = housesTotal;
			for (int i = 0; i < _levels.Count; i++) {
				_cameraController.MoveCamera(_levels[i].transform.position.y - 5f);
				yield return new WaitForSeconds(GlobalConstants.CAMERA_SHIFT_DURATION);
				_filledHousesTotal += _levels[i].CountFilledHouses();
				_housesTotal += _levels[i].CountHouses();
				yield return new WaitForSeconds(i == _levels.Count - 1 ? 5f : _filledHousesTotal * 1.5f);
			}
			
			// Поднять камеру вверх, показать город
			_cameraController.MoveCamera(MAIN_MENU_CAMERA_Y);
			yield return new WaitForSeconds(GlobalConstants.CAMERA_SHIFT_DURATION);
			
			float houseFillPercentage = (float) _filledHousesTotal / _housesTotal * 100f;
			Debug.Log("GameManager: Количество заполненных домов: " + _filledHousesTotal);
			Debug.Log("GameManager: Количество всех домов: " + _housesTotal);
			Debug.Log("GameManager: Процент заполненных домов: " + houseFillPercentage.ToString("F2") + "%");
			if ( houseFillPercentage >= GlobalConstants.MIN_FILLED_HOUSES_PERCENTAGE) {
				yield return StartCoroutine(_mainMenu.LightenBackground());
				Debug.Log("GameManager: Зажечь свет города");
				_audioSource.clip = _winSound;
				_audioSource.Play();
				//TODO Показать меню победы
			} else {
				_audioSource.clip = _loseSound;
				_audioSource.Play();
				yield return StartCoroutine(_mainMenu.DarkenBackground());
				Debug.Log("GameManager: Погрузить город в темноту");
				//TODO Показать меню проигрыша
			}
		}
		
		public bool IsWin
		{
			get { return (float) (_filledHousesTotal + 1) / (_housesTotal + 1) * 100f >= GlobalConstants.MIN_FILLED_HOUSES_PERCENTAGE; }
		}
	}
}
