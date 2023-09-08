using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	private Image _image;

	private void Awake()
	{
		_image = GetComponent<Image>();
	}
	
	public void Show()
	{
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
		_image.DOFade(1, 1f).SetEase(Ease.Linear);
	}
	
	public void Hide()
	{
		_image.DOFade(0, 1f).SetEase(Ease.Linear);
	}
}
