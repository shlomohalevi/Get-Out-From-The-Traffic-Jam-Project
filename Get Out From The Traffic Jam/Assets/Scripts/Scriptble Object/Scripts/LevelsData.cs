using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Levels")]
public class LevelsData : ScriptableObject
{
  public List<GameObject> levels = new List<GameObject>();
  public List<int> numOfAllowedMovesInEachLevel = new List<int>();
}
