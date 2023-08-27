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
		[SerializeField]
		private bool _isLastLevel;
		[SerializeField] 
		private SpriteRenderer _tubeBottom;
		[SerializeField]
		private SpriteRenderer _tubeSide;
	
		private BlockLauncher _blockLauncher;

		private void Awake()
		{
			if (_isLastLevel) {
				_tubeBottom.gameObject.SetActive(false);
				_tubeSide.gameObject.SetActive(false);
			}
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
			if (_isLastLevel) {
				_tubeBottom.gameObject.SetActive(true);
				yield return new WaitForSeconds(1f);
				_tubeSide.gameObject.SetActive(true);
				yield return new WaitForSeconds(1f);
				yield break;
			}
			foreach (HouseTemplate houseTemplate in _houseTemplates.Where(_houseTemplate => _houseTemplate.IsHouseFilled)) {
				yield return StartCoroutine(houseTemplate.Lighten());
			}
		}
	}
}
