using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blocks", menuName = "ScriptableObjects/Blocks")]
public class Blocks : ScriptableObject
{
    public GameObject[] _blockPrefabs; // Массив с префабами блоков
}