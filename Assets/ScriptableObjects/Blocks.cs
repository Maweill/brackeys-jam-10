using UnityEngine;

namespace ScriptableObjects
{
	[CreateAssetMenu(fileName = "Blocks", menuName = "ScriptableObjects/Blocks")]
	public class Blocks : ScriptableObject
	{
		public GameObject[] _blockPrefabs;
	}
}
