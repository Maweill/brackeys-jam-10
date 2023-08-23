using UnityEngine;

public class BlockFactory : MonoBehaviour
{
	[SerializeField] private Blocks _blocksData;
	private int _currentIndex = 0;

	public Block CreateBlock(Vector3 startPosition)
	{
		if (_currentIndex >= _blocksData._blockPrefabs.Length)
		{
			// Если список блоков закончился, не создавайте новые блоки
			GameEvents.InvokeLevelCompleted();
			return null;
		}

		GameObject blockPrefab = _blocksData._blockPrefabs[_currentIndex];

		Block block = Instantiate(blockPrefab, startPosition, Quaternion.identity).GetComponent<Block>();
		block.Rigidbody.isKinematic = true;
		block.Initialize(_currentIndex);
		_currentIndex++;

		return block;
	}
}
