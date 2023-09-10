using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		[SerializeField]
		private BlockLauncher _blockLauncher;
		
		private AudioSource _audioSource;
		
		private List<Block> _placedBlocks;

		private void Awake()
		{
			if (_isLastLevel) {
				_tubeBottom.gameObject.SetActive(false);
				_tubeSide.gameObject.SetActive(false);
			}
			gameObject.SetActive(false);
			_audioSource = GetComponent<AudioSource>();
			_placedBlocks = new List<Block>();
		}

		public void StartLevel()
		{
			foreach (Block placedBlock in _placedBlocks.Where(block => block != null)) {
				Destroy(placedBlock.gameObject);
			}
			_placedBlocks = new List<Block>();
			gameObject.SetActive(true);
			_houseTemplates.ForEach(houseTemplate => houseTemplate.Show());
			_blockLauncher.gameObject.SetActive(true);
			GameEvents.BlockPlaced += OnBlockPlaced;
			GameEvents.BlocksEnded += OnBlocksEnded;
			GameEvents.InvokeLevelInitialized();
		}

		public void EndLevel()
		{
			GameEvents.BlockPlaced -= OnBlockPlaced;
			GameEvents.BlocksEnded -= OnBlocksEnded;
			_blockLauncher.gameObject.SetActive(false);
			_houseTemplates.ForEach(houseTemplate => houseTemplate.Hide());
		}

		private void OnBlockPlaced(Block block)
		{
			_placedBlocks.Add(block);
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
