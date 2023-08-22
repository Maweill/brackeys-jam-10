using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button _newGameButton;

	private void Awake()
	{
		_newGameButton.onClick.AddListener(OnNewGameButtonClicked);
	}

	private void OnNewGameButtonClicked()
	{
		gameObject.SetActive(false);
		GameEvents.InvokeGameStarted();
	}
}
