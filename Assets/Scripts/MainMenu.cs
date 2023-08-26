using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject _newGameButton;
	[SerializeField] private GameObject _optionsButton;

	public void HideButtons()
	{
		_newGameButton.gameObject.SetActive(false);
		_optionsButton.gameObject.SetActive(false);
	}
}
