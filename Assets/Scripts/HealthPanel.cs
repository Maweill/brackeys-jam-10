using System.Collections.Generic;
using UnityEngine;

public class HealthPanel : MonoBehaviour
{
	[SerializeField] private GameObject _healthPrefab;

	private Stack<GameObject> _lives = new();
	private Stack<GameObject> _savedLives = new();
	private bool _levelFailed;

	private void Awake()
	{
		for (int i = 0; i < GlobalConstants.LEVEL_LIVES; i++) {
			_lives.Push(Instantiate(_healthPrefab, transform));
		}
		_savedLives = new Stack<GameObject>(_lives);
		HideAllLives();
	}

	private void OnEnable()
	{
		GameEvents.BlockTemplateFilled += OnBlockTemplateFilled;
		GameEvents.LevelInitialized += OnLevelInitialized;
		GameEvents.LevelCompleted += OnLevelCompleted;
		GameEvents.LevelFailed += OnLevelFailed;
	}
	
	private void OnDisable()
	{
		GameEvents.BlockTemplateFilled -= OnBlockTemplateFilled;		
		GameEvents.LevelInitialized -= OnLevelInitialized;
		GameEvents.LevelCompleted -= OnLevelCompleted;
		GameEvents.LevelFailed -= OnLevelFailed;
	}
	
	private void OnLevelInitialized()
	{
		ShowAllLives();
		_levelFailed = false;
	}

	private void OnLevelCompleted()
	{
		HideAllLives();
	}
	
	private void OnLevelFailed()
	{
		_levelFailed = true;
	}
	
	private void OnBlockTemplateFilled(int _, bool templateFilled)
	{
		if (templateFilled || _levelFailed) {
			return;
		}
		_lives.Pop().SetActive(false);
	}

	private void ShowAllLives()
	{
		_lives = new Stack<GameObject>(_savedLives);
		foreach (GameObject life in _lives) {
			life.SetActive(true);
		}
	}

	private void HideAllLives()
	{
		foreach (GameObject life in _lives) {
			life.SetActive(false);
		}
	}
}
