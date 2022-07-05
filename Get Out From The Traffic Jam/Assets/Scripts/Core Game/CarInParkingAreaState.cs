using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// responsible to the car when the car in the parking area
/// </summary>

public class CarInParkingAreaState : CarState
{
    public static Action<Vector3> onCollisionCar = default;
    [SerializeField] List<Transform> frontLeftAndFrontRightPositionsOfTheCar = new List<Transform>(2);
    [SerializeField] List<Transform> backLeftAndBackRightPositionsOfTheCar = new List<Transform>(2);
    [SerializeField] LayerMask objectsWeCantPassWithTheCar = default;
    [SerializeField] float distanceToCheckWithRaycastFromThePositionsOnTheCar = default;
    [SerializeField] Sprite[] angryAndSadEmojies = new Sprite[2];
    [SerializeField] SpriteRenderer angryAndSadrendererComponent = default;
    List<Transform> positionsToCheckFromIfOtherObjectBlockAs = new List<Transform>(2);
    public bool carCurrentlyMoving = false;
    Vector3 collisionPointBetweenTwoObjects = default;
    Vector3 lastDirectionBefourThisCarEnteredToThePathArea = default;
    static CarInParkingAreaState lastCarTheUserMove = default;
    void Start()
    {
        base.CatchComponents();
        AdjustAngryAndSadEmojiRotation();
        currentCarAreaState = carAreaState.parkingAreaState;
        GameManeger.GameManegerInstance.IncreasNumsOfCarsInTheSceneCounter();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("StaticBarrier"))
        {
            CarCollisionProcesss(other, true);
            onCollisionCar?.Invoke(collisionPointBetweenTwoObjects);
            //play the static barrier animation 
            var staticbarrierAnimator = other.GetComponent<Animator>();
            HitBarrierProcess(staticbarrierAnimator);
           
        }
        else if(other.CompareTag("Car"))
        {
            var carWeCollideWith = other.GetComponent<CarState>();
            // make the hit effect and atc only if the car we collide with  in the parking area
            if (CarWeCollideWithStillInTheParkingArea(carWeCollideWith.currentCarAreaState))
            {
                if (ThisInstanceCarAcussedInTheCollision(this, carWeCollideWith))
                {
                    CarCollisionProcesss(carWeCollideWith.GetComponent<Collider>(), true);
                    onCollisionCar?.Invoke(collisionPointBetweenTwoObjects);
                }
                else
                    CarCollisionProcesss(carWeCollideWith.GetComponent<Collider>(), false);
            }
            //if the other car already in the path area (we are going to enter to the path)
            else
            {
                Vector3 VectorFromTheCarOnThePathToThisCar = UtilitiesMethods.GetDirectionFromPositionAToPositionB(other.transform.position, transform.position);
                float angel = Vector3.Angle(VectorFromTheCarOnThePathToThisCar,other.transform.forward);
                //if so we need to let stop the move and let to the car in path to keep drive
                if (angel >=80 )
                    //get the desired direction to move the car after the path will clears befour we stop the car from driving 
                    if (carRigidbody.velocity != Vector3.zero)
                    {
                        lastDirectionBefourThisCarEnteredToThePathArea = carRigidbody.velocity.normalized;
                        UtilitiesMethods.StopMovingCar(carRigidbody);
                    }
            }
        }
        else if(other.CompareTag("PathPoint"))
        {
            Destroy(this);
            gameObject.AddComponent<CarInPathAreaState>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            var carWeCollidWith = other.GetComponent<CarState>();
            //if true the path cleared and we can keep drive to the path
            if (ThisInstanceCarInTheParkingAreaAndTheOtherCarInThePathArea(this, carWeCollidWith))
                carRigidbody.velocity = lastDirectionBefourThisCarEnteredToThePathArea * carSpeed;
        }
    }
   
    /// <summary>
    /// move the car and update fields values acorrding to this new move state
    /// </summary>
   public void MoveTheCar(Vector3 normelizedDirToMoveTheCar)
    {
        carCurrentlyMoving = true;
        lastCarTheUserMove = this;
        carRigidbody.velocity = normelizedDirToMoveTheCar * carSpeed;
    }
    /// <summary>
    /// calculate collision details ,where we get the hit and which vfx and animation to play according those details
    /// </summary>
   public void CarCollisionProcesss(Collider colliderOfObjectWeCollidWith,bool isThisCarAccusedInTheCollision)
    {
        UtilitiesMethods.StopMovingCar(carRigidbody);
        //get the collision point 
        collisionPointBetweenTwoObjects = colliderOfObjectWeCollidWith.ClosestPoint(transform.position);
        Vector3 vectorFromTheCarToTheObjectWeCollideWith = UtilitiesMethods.GetDirectionFromPositionAToPositionB(transform.position, colliderOfObjectWeCollidWith.transform.position);
        if (isThisCarAccusedInTheCollision)
        {
            string collisionAnimationCarToPlay = UtilitiesMethods.DotProductBiggerThanZiro(vectorFromTheCarToTheObjectWeCollideWith, transform.forward)
            ? "FrontHitAnimation" : "BackHitAnimation";
            carAnimator.SetTrigger(collisionAnimationCarToPlay);
        }
        else
        {
            bool carGetHitFromSide = Vector3.Angle(transform.forward, colliderOfObjectWeCollidWith.transform.forward) == Mathf.Round(90);
            if (carGetHitFromSide)
            {
                string collisionAnimationCarToPlay = UtilitiesMethods.DotProductBiggerThanZiro(vectorFromTheCarToTheObjectWeCollideWith, transform.right)
                ? "RightHitAnimation" : "LeftHitAnimation";
                carAnimator.SetTrigger(collisionAnimationCarToPlay);
            }
            else
            {
                string collisionAnimationCarToPlay = UtilitiesMethods.DotProductBiggerThanZiro(vectorFromTheCarToTheObjectWeCollideWith, transform.forward)
                ? "FrontHitAnimation" : "BackHitAnimation";
                carAnimator.SetTrigger(collisionAnimationCarToPlay);
            }
                ChooseRandomAngryOrSadEmoji();
                carAnimator.SetTrigger("AngryAnimation");
        }
    }
   /// <returns>true if the car we collided with in parking area</returns>
    bool CarWeCollideWithStillInTheParkingArea(carAreaState carWeCollideWithState) =>carWeCollideWithState == carAreaState.parkingAreaState;
    
    /// <returns>true if this instance car acussed in the collision </returns>
    bool ThisInstanceCarAcussedInTheCollision(CarState thisInstanceCar,CarState otherInstanceCar)
    {
        //its not possible to be the last car we move and not be accused in the collision
        if (thisInstanceCar == lastCarTheUserMove) return true;
        bool IsAcussed = Vector3.Dot(thisInstanceCar.transform.forward, otherInstanceCar.transform.forward) == Mathf.Round(0)
        && carCurrentlyMoving; return IsAcussed;
    }
    /// <summary>
    /// check if the user allowed to move the car to given direction 
    /// </summary>
    /// <returns>true if the user allowed to move the car to the given direction</returns>
    public bool CarAllowedToMoveToGivenDirection(Vector3 directionToMoveTheCar)
    {
        // calculate from where we need to check if we can move the car back or forth according to the dir we trying to move the car
        positionsToCheckFromIfOtherObjectBlockAs = directionToMoveTheCar == transform.forward
        ? frontLeftAndFrontRightPositionsOfTheCar : backLeftAndBackRightPositionsOfTheCar;
        
            foreach(Transform transformPoint in positionsToCheckFromIfOtherObjectBlockAs)
            {
                Ray ray = new Ray(transformPoint.position, directionToMoveTheCar);
                if (Physics.Raycast(ray,out RaycastHit hitInfo, distanceToCheckWithRaycastFromThePositionsOnTheCar, objectsWeCantPassWithTheCar))
                {
                //even we are not allowed to move the car we are still trying to move (not with velocity but with hit animation )
                    carCurrentlyMoving = true;
                    lastCarTheUserMove = this;
                    CarCollisionProcesss(hitInfo.collider, true);
                    if (hitInfo.collider.CompareTag("Car"))
                        hitInfo.transform.GetComponent<CarInParkingAreaState>().CarCollisionProcesss(carCollider, false);
                    else
                    //if we hit the barrier
                    {
                        var staticbarrierAnimator = hitInfo.transform.GetComponent<Animator>();
                        HitBarrierProcess(staticbarrierAnimator);
                    }
                    onCollisionCar?.Invoke(collisionPointBetweenTwoObjects); 

                return false;
                }
            }
        return true;
    }
    /// <returns>true if the conditions are met</returns>
     bool ThisInstanceCarInTheParkingAreaAndTheOtherCarInThePathArea(CarState thisInstanceCar,CarState otherInstanceCar)
    {
        if (thisInstanceCar.currentCarAreaState == carAreaState.parkingAreaState &&
            otherInstanceCar.currentCarAreaState == carAreaState.pathAreaState) return true;
        return false;
    }
    /// <summary>
    /// invoked in the event in hit animation car when the hit car animation ended
    /// </summary>
    public void ReturnCarMoveFlagToFalse() => carCurrentlyMoving = false;
    public void ChooseRandomAngryOrSadEmoji()
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        angryAndSadrendererComponent.sprite = angryAndSadEmojies[rnd];
    }
    /// <summary>
    /// always align emoji rotation to the camera regardless to the car rotation
    /// </summary>
    void AdjustAngryAndSadEmojiRotation() => angryAndSadrendererComponent.transform.eulerAngles = new Vector3(90, 0, 0);

   /// <summary>
   /// calculate from where the barrier get hit and play animation according this
   /// </summary>
    void HitBarrierProcess(Animator staticBarrierAnimator)
    {
        var dirFromBarrierToTheCar = UtilitiesMethods.GetDirectionFromPositionAToPositionB(staticBarrierAnimator.transform.position, transform.position);
        string barrierAnimToPlay = UtilitiesMethods.DotProductBiggerThanZiro(dirFromBarrierToTheCar, staticBarrierAnimator.transform.right)
        ? "BarrierHitRightAnim" : "BarrierHitLeftAnim";
        staticBarrierAnimator.SetTrigger(barrierAnimToPlay);
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Ray ray = new Ray(backLeftAndBackRightPositionsOfTheCar[0].position, -transform.forward);
    //    Debug.DrawRay(ray.origin, ray.direction * distanceToCheckWithRaycastFromThePositionsOnTheCar, Color.red);
    //}

}
       

                
                
            
   

  
  
        
       
   
        
      
    
          
            
            
       
         
           


           
                

                
                 
                




            
                
            

            
    
    
    




    

    



    
    
