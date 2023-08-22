using UnityEngine;

public class BlockFactory : MonoBehaviour
{
	[SerializeField] private Block _blockPrefab;

	public Block CreateBlock(Vector3 startPosition)
	{
		Block block = Instantiate(_blockPrefab, startPosition, Quaternion.identity);
		block.Rigidbody.isKinematic = true;
		return block;
	}
}
