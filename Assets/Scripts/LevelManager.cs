using System.Collections.Generic;
using House_Scripts;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField]
	private BlockLauncher _blockLauncherPrefab;
	[SerializeField] 
	private GameObject _background;
	[SerializeField]
	private List<HouseTemplate> _houseTemplates;
	
	private BlockLauncher _blockLauncher;
	
	public void StartLevel()
	{
		GameEvents.BlocksEnded += OnBlocksEnded;
		_background.SetActive(true);
		_blockLauncher = Instantiate(_blockLauncherPrefab, transform.position, Quaternion.identity, transform);
	}
	
	public void EndLevel()
	{
		GameEvents.BlocksEnded -= OnBlocksEnded;
		Destroy(_blockLauncher.gameObject);
		_houseTemplates.ForEach(houseTemplate => houseTemplate.gameObject.SetActive(false));
	}

	private void OnBlocksEnded()
	{
		GameEvents.InvokeLevelCompleted();
	}
}
