using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// /// responsible to the car when the car in the path area
/// </summary>

public class CarInPathAreaState : CarState
{
    public static Action onDinamicBarrierOpened = default;
    public static Action onDinamicBarrierClosed = default;
    public static Action onPassingEndPathBorder = default;
    static List<Transform> locationsPointsOnThePath = new List<Transform>();
    Transform lastPathPointWeWas = default;
    int pathLocationsIndex = 0;
    bool carAlraedyWasOnThePath = false;
    bool goingToCollideWithAnotherCar = false;
    private void OnEnable()
    {
        currentCarAreaState = carAreaState.pathAreaState;
        base.CatchComponents();
        //adjust sound details
        driveSoundSource.loop = true;
        driveSoundSource.Play();
    }

     
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("PathPoint"))
        {
            //prevent from overlaping with the same path point twice
            if (lastPathPointWeWas != null && lastPathPointWeWas == other.transform) return;
            lastPathPointWeWas = other.transform;
            UtilitiesMethods.StopMovingCar(carRigidbody);
            if (!carAlraedyWasOnThePath)
            {
                carAlraedyWasOnThePath = true;
                pathLocationsIndex = GetTheIndexOFTheNodeWrAreOn(other.transform);
            }
            else
                //we getting the next location point index on the path
                pathLocationsIndex++;
            if (WeGotToTheEndOfThePath(pathLocationsIndex))
            {
                //deactivate the car and dont keep the process
                gameObject.SetActive(false);
                return;
            }

            StartCoroutine(DriveMovementOnPath(pathLocationsIndex, pathLocationsIndex + 1));
        }
        else if (other.CompareTag("Car"))
        {
            var carWeCollideWith = other.GetComponent<CarState>();
            //check if we need to stop the car from driving and let to the other car preiority to drive
            Vector3 vectorFromThisCarOnTheToTheOtherCarOnThePath = UtilitiesMethods.GetDirectionFromPositionAToPositionB(transform.position, carWeCollideWith.transform.position);
            float angel = Vector3.Angle(vectorFromThisCarOnTheToTheOtherCarOnThePath, transform.forward);
            if (angel < 80)//if so stop moving the car and let the other car preiority to keep drive
            {
                goingToCollideWithAnotherCar = true;
                UtilitiesMethods.StopMovingCar(carRigidbody);
            }
        }
        else if (other.CompareTag("Trigger Barrier Anim"))//so we are going to reach the end of the path
        {
            happyEmojiRenderer.transform.parent.gameObject.SetActive(true);
            onDinamicBarrierOpened?.Invoke();
        }
        else if (other.CompareTag("End Track"))//we going to the end of the path
        {
            onPassingEndPathBorder?.Invoke();
            GameManeger.GameManegerInstance.DecreasNumsOfCarsInTheSceneCounter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Car") && goingToCollideWithAnotherCar)
        {
            //we can keep the driving process
            goingToCollideWithAnotherCar = false;
            StartCoroutine(DriveMovementOnPath(pathLocationsIndex, pathLocationsIndex + 1));
        }
        else if(other.CompareTag("Trigger Barrier Anim"))
        {
            onDinamicBarrierClosed?.Invoke();
        }
    }
   
       

    /// <returns>the index of the location point we trigger with in the location point path list</returns>
    int GetTheIndexOFTheNodeWrAreOn(Transform pathLocationPointTheCarTriggerWith)
    {
        int pathPointLocationIndex = locationsPointsOnThePath.IndexOf(pathLocationPointTheCarTriggerWith);
        return pathPointLocationIndex;
    }
    /// <returns>true if we got to the end of the path</returns>
    bool WeGotToTheEndOfThePath(int pathLocationIndex) => pathLocationIndex == locationsPointsOnThePath.Count - 1;
    /// <summary>
    /// drive movement on path process
    /// </summary>
    IEnumerator DriveMovementOnPath(int currentPathLocationIndex,int nextLocationPathIndex)
    {
        //get the desire direction to the next location on the path
        Vector3 directionToMoveTheCar = UtilitiesMethods.GetDirectionFromPositionAToPositionB
        (locationsPointsOnThePath[currentPathLocationIndex].position, locationsPointsOnThePath[nextLocationPathIndex].position);
        //rotate the forward car direction to the result direction if we are not going to collide with another car 
        while (Vector3.Angle(transform.forward, directionToMoveTheCar) > 0.1 && !goingToCollideWithAnotherCar)
        {
            Quaternion rotationGoal = Quaternion.LookRotation(directionToMoveTheCar);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationGoal, carTurnSpeed*Time.deltaTime);
            yield return null;
        }
        //keep to drive when we ended the rotation
        if(!goingToCollideWithAnotherCar)
        carRigidbody.velocity = transform.forward * carSpeed;
    }
    /// <summary>
    /// inithilize the locations on path of current level
    /// </summary>
    /// <param name="pathLocationsList"> path locationsListOfCurrentLevel</param>
    public static void InithilizePathLocationsListOfCurrentLevel(List<Transform> pathLocationsList) => locationsPointsOnThePath = pathLocationsList;
}
            

        
       






   
    

 

   
    
              
            

            
            



    







