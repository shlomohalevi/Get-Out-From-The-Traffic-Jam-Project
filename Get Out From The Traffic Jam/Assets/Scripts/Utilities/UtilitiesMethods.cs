
using UnityEngine;
/// <summary>
/// helper class for general common methods
/// </summary>
public static class UtilitiesMethods 
{
  
    /// <returns>mouse position on plane surface</returns>
   public static Vector3 GetMousePositionOnPlaneSuarface(Plane planeSuarface)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float mouseEnterPoint;
        if (planeSuarface.Raycast(ray, out mouseEnterPoint))
        {
            Vector3 mousePosition = ray.GetPoint(mouseEnterPoint);
            return mousePosition;
        }
        return default;
    }
    /// <returns>true if user realeas left mouse button</returns>
    public static bool UserRealeasMousButton()
    {
      if(Input.touchCount>0)
          return  Input.GetTouch(0).phase == TouchPhase.Ended;
        return false;
        
    } 
   /// <returns>distance between two positions</returns>
  public static float DistanceBetweenTwoPositions(Vector3 positionA, Vector3 positionB) => Vector3.Distance(positionA, positionB);
    /// <returns>surface plane for mouse detection</returns>
  public static Plane SetPlaneSurfaceForMouseDetection(Vector3 planeNormal,Vector3 planePosition)
    {
        Plane planeSurfaceForMouseDetection = new Plane(planeNormal, planePosition);
        return planeSurfaceForMouseDetection;
    }
    /// <returns>true if bigger than ziro</returns>
  public static bool DotProductBiggerThanZiro(Vector3 vectorA, Vector3 vectorB) => Vector3.Dot(vectorA, vectorB) > 0;
    /// <returns>direction between to positions</returns>
  public static Vector3 GetDirectionFromPositionAToPositionB(Vector3 positionA,Vector3 PositionB)
    {
        Vector3 direction = PositionB - positionA;
        return direction;
    }
    /// <summary>
    /// make partical effect to in given position
    /// </summary>
    public static void makeParticalEffect(ParticleSystem particalToPlay,Vector3 positionToPlayThePartical)
    {
        particalToPlay.transform.position = positionToPlayThePartical;
        particalToPlay.Play();
    }
   /// <summary>
   /// stoping car movement
   /// </summary>
    public static void StopMovingCar(Rigidbody rigidbodyOfTheCar)
    {
        rigidbodyOfTheCar.velocity = Vector3.zero;
        rigidbodyOfTheCar.angularVelocity = Vector3.zero;
    }
}
    
    

   
   

   
    
