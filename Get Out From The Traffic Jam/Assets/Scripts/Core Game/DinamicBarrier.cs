using UnityEngine;
/// <summary>
/// responsible to the dinamic barrier
/// </summary>
public class DinamicBarrier : MonoBehaviour
{
    Animator dinamicBarrierAnimator;
    int carsThatGoingToPassTheBarrier = 0;
    void Start()
    {
        dinamicBarrierAnimator = GetComponent<Animator>();
        CarInPathAreaState.onDinamicBarrierOpened += OpenBarrier;
        CarInPathAreaState.onDinamicBarrierClosed += CloseBarrier;
    }
    private void OnDisable()
    {
        CarInPathAreaState.onDinamicBarrierOpened -= OpenBarrier;
        CarInPathAreaState.onDinamicBarrierClosed -= CloseBarrier;
    }
    void OpenBarrier()
    {
            carsThatGoingToPassTheBarrier++;
            dinamicBarrierAnimator.SetBool("RaiseTheBarrier", true);
    }
    void CloseBarrier()
    {
            carsThatGoingToPassTheBarrier--;
            //close the barrier only if there is no car that going to pass the barrier
            if (carsThatGoingToPassTheBarrier == 0)
                dinamicBarrierAnimator.SetBool("RaiseTheBarrier", false);
    }
}
        

   
   
    

    
    

   
    


   
