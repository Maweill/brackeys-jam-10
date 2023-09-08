using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using House_Scripts;
using UnityEngine;
using UnityEngine.Serialization;

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
		[SerializeField]
		private AudioClip _tubePlacementSound;
		[FormerlySerializedAs("_engineSound")] [SerializeField]
		private AudioClip _winEngineSound;
		[SerializeField]
		private AudioClip _loseEngineSound;
	
		private BlockLauncher _blockLauncher;
		private AudioSource _audioSource;

		private void Awake()
		{
			if (_isLastLevel) {
				_tubeBottom.gameObject.SetActive(false);
				_tubeSide.gameObject.SetActive(false);
			}
			gameObject.SetActive(false);
			_blockLauncher = GetComponentInChildren<BlockLauncher>();
			_audioSource = GetComponent<AudioSource>();
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
			GameManager gameManager = FindObjectOfType<GameManager>();
			Engine engine = FindObjectOfType<Engine>();

			if (_isLastLevel) {
				_tubeBottom.gameObject.SetActive(true);
				_audioSource.clip = _tubePlacementSound;
				_audioSource.Play();
				yield return new WaitForSeconds(1f);
				_tubeSide.gameObject.SetActive(true);
				_audioSource.Play();
				yield return new WaitForSeconds(1f);
				_audioSource.clip =  gameManager.IsWin ? _winEngineSound : _loseEngineSound;
				_audioSource.Play();
				engine.PlayEngineWinAnimation();
				yield break;
			}
			foreach (HouseTemplate houseTemplate in _houseTemplates.Where(_houseTemplate => _houseTemplate.IsHouseFilled)) {
				yield return StartCoroutine(houseTemplate.Lighten());
			}
		}

		public bool IsLast
		{
			get { return _isLastLevel; }
		}
	}
}
