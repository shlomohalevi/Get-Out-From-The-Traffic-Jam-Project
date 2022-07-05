using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GenerateLevels : MonoBehaviour
{
    public static readonly string LastLevelTheUserPlayedKey = "lastLevelTheUserPlayed";
    public static Action OnGenerateLevel = default;
    [SerializeField] LevelsData levelsData = default;
    [SerializeField] bool GenerateLevel = true;
    [SerializeField] string PathName = default;
    public static GenerateLevels generateLevelsInstance = default;
    public List<Transform> pathPointsOfCurrentLevel = new List<Transform>();
    int numberOfCurrentLevel = default;

    private void Awake()
    {
        #region singelton
        if (generateLevelsInstance == null)
            generateLevelsInstance = FindObjectOfType<GenerateLevels>();
        #endregion
       // if (!GenerateLevel) return;
            numberOfCurrentLevel = PlayerPrefs.GetInt(LastLevelTheUserPlayedKey,0);
            GenerateLevelProcess(numberOfCurrentLevel);
            CarInPathAreaState.InithilizePathLocationsListOfCurrentLevel(pathPointsOfCurrentLevel);
    }
   
    void GenerateLevelProcess(int levelIndexToInstantiate)
    {
        GameObject currentLevel = Instantiate(levelsData.levels[levelIndexToInstantiate], Vector3.zero, Quaternion.identity);
        Transform pathPointsParentTransform = currentLevel.transform.Find(PathName).transform;
        pathPointsOfCurrentLevel = pathPointsParentTransform.GetComponentsInChildren<Transform>().ToList();
    }
    /// <returns>number of allowed moves in current level</returns>
    public int GetNumOfAllowedMovesINCurrentLevel()
    {
      //  return 100;
        int currentlevelIndex = PlayerPrefs.GetInt(LastLevelTheUserPlayedKey);
        int allowedMovesInCurrentLevel = levelsData.numOfAllowedMovesInEachLevel[currentlevelIndex];
        return allowedMovesInCurrentLevel;
    }
    public string GetLevelText()
    {
       //return default;
        int currentlevelIndex = PlayerPrefs.GetInt(LastLevelTheUserPlayedKey);
        return $"Level {currentlevelIndex+1}";
    }
}
   
    
  
      
        
       
   

          
            


           



