using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthPanel : MonoBehaviour
{
	[SerializeField] private GameObject _healthPrefab;

	private Stack<GameObject> _lives = new();
	private Stack<GameObject> _savedLives = new();

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
	}
	
	private void OnDisable()
	{
		GameEvents.BlockTemplateFilled -= OnBlockTemplateFilled;		
		GameEvents.LevelInitialized -= OnLevelInitialized;
		GameEvents.LevelCompleted -= OnLevelCompleted;
	}
	
	private void OnLevelInitialized()
	{
		ShowAllLives();
	}

	private void OnLevelCompleted()
	{
		HideAllLives();
	}
	
	private void OnBlockTemplateFilled(int _, bool templateFilled)
	{
		if (templateFilled) {
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
