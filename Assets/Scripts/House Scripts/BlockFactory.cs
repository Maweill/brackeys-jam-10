using ScriptableObjects;
using UnityEngine;

namespace House_Scripts
{
	public class BlockFactory : MonoBehaviour
	{
		[SerializeField] private Blocks _blocksData;
		
		private int _currentIndex;

		public Block CreateBlock(Vector3 startPosition)
		{
			if (_currentIndex >= _blocksData._blockPrefabs.Length) {
				GameEvents.InvokeBlocksEnded();
				return null;
			}

			GameObject blockPrefab = _blocksData._blockPrefabs[_currentIndex];

			Block block = Instantiate(blockPrefab, startPosition, Quaternion.identity).GetComponent<Block>();
			block.Rigidbody.isKinematic = true;
			block.Initialize(_currentIndex);
			_currentIndex++;
			GameEvents.InvokeBlockSpawned(block);
			return block;
		}
		
		public void ResetIndex()
		{
			_currentIndex = 0;
		}

		public void ResetToPreviousIndex()
		{
			_currentIndex--;
		}
	}
}
