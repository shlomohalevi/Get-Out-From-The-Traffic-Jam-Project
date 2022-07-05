using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
/// <summary>
/// responsible on the game flow of the game 
/// </summary>
public class GameManeger : MonoBehaviour
{
    public static Action<int> OnDecreasingNumsOfMoves;
    public static GameManeger GameManegerInstance;
    [SerializeField] float numOfangelAmountBetweenMouseMoveDirectionToCarForwardDirection = 90;
    mouseState currentMouseState = default;
    Vector3 initialMousePosition = default;
    CarInParkingAreaState carToMove = default;
    Plane planSurfaceForMouseDetection = default;
    int numOfCarsInTheScene = 0;
    public int numOfmoves = default;
    bool playerWin = false;
    private void Awake()
    {
        #region singelton
        if (GameManegerInstance == null)
          GameManegerInstance =  FindObjectOfType<GameManeger>();
        #endregion
    }
    void Start()
    {
        numOfmoves = GenerateLevels.generateLevelsInstance.GetNumOfAllowedMovesINCurrentLevel();
        planSurfaceForMouseDetection = UtilitiesMethods.SetPlaneSurfaceForMouseDetection(Vector3.up, Vector3.zero);
        currentMouseState = mouseState.mouseDoesNotHaveCarToMove;
    }
    

    void Update()
    {
        //prevent inraction with the game if we pressing on ui
    if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
    switch(currentMouseState)
    {
        case mouseState.mouseDoesNotHaveCarToMove:
            {
                CarInParkingAreaState carThatUserTryingtoMoveWithTheMouse;
                // check if we pressing on car
                bool wePressingOnCarInParkingArea = UserPressingOnCarWithTheMouse(out carThatUserTryingtoMoveWithTheMouse);
                //if we pressing on car and also the car not in movement now
                if (wePressingOnCarInParkingArea && !TheCarCurrentlyAlradyMoving(carThatUserTryingtoMoveWithTheMouse)) 
                {
                    if (IfWeOutOfMoves())//if so we loose
                    {
                        LoadNewScene(); return;
                    }
                           
                    initialMousePosition = UtilitiesMethods.GetMousePositionOnPlaneSuarface(planSurfaceForMouseDetection);
                    carToMove = carThatUserTryingtoMoveWithTheMouse;
                    currentMouseState = mouseState.mouseHaveCarToMove;
                }
                break;
            }
                       
        case mouseState.mouseHaveCarToMove:
            {
                if (UtilitiesMethods.UserRealeasMousButton())
                {
                    numOfmoves--;
                    OnDecreasingNumsOfMoves?.Invoke(numOfmoves);
                    Vector3 currentMousePosition = UtilitiesMethods.GetMousePositionOnPlaneSuarface(planSurfaceForMouseDetection);
                    Vector3 dirToMoveTheCar = GetdirectionTheCarNeedToDrive(initialMousePosition, currentMousePosition);
                    //if we are allowed to move to the given direction
                    if (carToMove.CarAllowedToMoveToGivenDirection(dirToMoveTheCar))
                    {
                        carToMove.MoveTheCar(dirToMoveTheCar);
                    }
                    //We are no longer trying to move this car
                    currentMouseState = mouseState.mouseDoesNotHaveCarToMove;
                    carToMove = default;
                 }
                break;
            }
    }
        
    }
    /// <summary>
    /// check if user pressing on car
    /// </summary>
    /// <returns>true if user pressing on car</returns>
    bool UserPressingOnCarWithTheMouse(out CarInParkingAreaState carThatWePressingOn)
    {
        if (Input.touchCount > 0)
        {
            Touch touchDown = Input.GetTouch(0);
            if (touchDown.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touchDown.position);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Car")))
                {
                    if (hit.transform.TryGetComponent(out CarInParkingAreaState carInParkingArea))
                    {
                        carThatWePressingOn = carInParkingArea;
                        return true;
                    }

                }
            }
        }
        carThatWePressingOn = default;
    /// <returns>true if the car is currently moving</returns>
        return false;
    }
    bool TheCarCurrentlyAlradyMoving(CarInParkingAreaState carThatWeTryingToMove) => carThatWeTryingToMove.carCurrentlyMoving;
   /// <summary>
   /// calculate the direction of the mouse move and get the direction the car needed to move back or forth according this
   /// </summary>
   /// <returns>direction the car need to move</returns>
    Vector3 GetdirectionTheCarNeedToDrive(Vector3 initialMousePosition,Vector3 currentMousePos)
    {
        Vector3 vectorFromInitialMousePosToTheCurrentMousePos = currentMousePos - initialMousePosition;
        float angelBetweenMouseMoveDirectionAndCarForwardDirection =
         Vector3.Angle(carToMove.transform.forward, vectorFromInitialMousePosToTheCurrentMousePos);
        if (angelBetweenMouseMoveDirectionAndCarForwardDirection <= numOfangelAmountBetweenMouseMoveDirectionToCarForwardDirection)
            return carToMove.transform.forward;
        else 
            return -carToMove.transform.forward;
    }
    /// <summary>
    /// count the number of cars in current level
    /// </summary>
    public void IncreasNumsOfCarsInTheSceneCounter() => numOfCarsInTheScene++;
    public void DecreasNumsOfCarsInTheSceneCounter()
    {
        numOfCarsInTheScene--;
        if (numOfCarsInTheScene <= 0)
        {
            playerWin = true;
            Invoke(nameof(LoadNewScene), 1.5f);
        }
    }
     void LoadNewScene()
    {
        int indexSceneToLoad = playerWin ? (int)SceneIndex.winSceneIndex : (int)SceneIndex.looseSceneIndex;
        SceneManager.LoadScene(indexSceneToLoad);
    }
    /// <returns>true if we out of moves</returns>
    bool IfWeOutOfMoves() => numOfmoves <= 0;
    /// <summary>
    /// enum to swith the states of the mouse
    /// </summary>
    enum mouseState
    {
        mouseDoesNotHaveCarToMove,
        mouseHaveCarToMove,
    }
}
       
        
       
                       
                
                        
                        
                        
                        
                       
                   

              

   
    
    
    
      
        
       
















