using UnityEngine;
using UnityEngine.UI;

public class GameLevelUi : MonoBehaviour
{
    [SerializeField] Text numOfMovesText = default;
    [SerializeField] Text levelNumText = default;
    void Start()
    {
        DisplayNumOfMovesAndNumOfCurrentLevelTextWhenNewLevelStart();
        GameManeger.OnDecreasingNumsOfMoves += UpdateNumOfMovesText;
    }
    private void OnDisable()
    {
        GameManeger.OnDecreasingNumsOfMoves -= UpdateNumOfMovesText;

    }
    void UpdateNumOfMovesText(int remainingNumOfMoves)
    {
        if (numOfMovesText != null)
            numOfMovesText.text = remainingNumOfMoves.ToString();
    }
    void DisplayNumOfMovesAndNumOfCurrentLevelTextWhenNewLevelStart()
    {
        levelNumText.text = GenerateLevels.generateLevelsInstance.GetLevelText();
        numOfMovesText.text = GenerateLevels.generateLevelsInstance.GetNumOfAllowedMovesINCurrentLevel().ToString();
    }
}
