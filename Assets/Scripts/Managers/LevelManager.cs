using System.Collections;
using System.Collections.Generic;
using System.Linq;
using House_Scripts;
using UnityEngine;

namespace Managers
{
	public class LevelManager : MonoBehaviour
	{
		[SerializeField]
		private List<HouseTemplate> _houseTemplates;
	
		private BlockLauncher _blockLauncher;

		private void Awake()
		{
			gameObject.SetActive(false);
			_blockLauncher = GetComponentInChildren<BlockLauncher>();
		}

		public void StartLevel()
		{
			gameObject.SetActive(true);
			GameEvents.BlocksEnded += OnBlocksEnded;
		}
	
		public void EndLevel()
		{
			GameEvents.BlocksEnded -= OnBlocksEnded;
			Destroy(_blockLauncher.gameObject);
			_houseTemplates.ForEach(houseTemplate => houseTemplate.gameObject.SetActive(false));
		}
		
		public int CountFilledHouses()
		{
			StartCoroutine(LightenHouses());
			int filledHouses = _houseTemplates.Count(house => house.IsHouseFilled);
			Debug.Log("LevelManager: Количество установленных домов = " + filledHouses);
			return filledHouses;
		}
		
		public int CountHouses()
		{
			return _houseTemplates.Count;
		}

		private void OnBlocksEnded()
		{
			GameEvents.InvokeLevelCompleted();
		}
		
		private IEnumerator LightenHouses()
		{
			foreach (HouseTemplate houseTemplate in _houseTemplates) {
				yield return StartCoroutine(houseTemplate.Lighten());
			}
		}
	}
}
