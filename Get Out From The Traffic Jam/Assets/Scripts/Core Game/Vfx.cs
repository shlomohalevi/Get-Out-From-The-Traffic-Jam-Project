
using UnityEngine;

public class Vfx : MonoBehaviour
{
    [SerializeField] AudioClip collisionSound = default;
    [SerializeField] ParticleSystem collisionPartical = default;
    [SerializeField] ParticleSystem confetyEffect = default;
    [SerializeField] AudioClip ShoutHappySound = default;
    [SerializeField] AudioClip ConffetySound = default;
    [SerializeField] float volume = 1f;

    void Start()
    {
        CarInParkingAreaState.onCollisionCar += PlayCollisionCarVfx;
        CarInPathAreaState.onPassingEndPathBorder += PlayVfxWhenPassingTheBarrier;

    }
    private void OnDisable()
    {
        CarInParkingAreaState.onCollisionCar -= PlayCollisionCarVfx;
        CarInPathAreaState.onPassingEndPathBorder -= PlayVfxWhenPassingTheBarrier;

    }

    void PlayVfxWhenPassingTheBarrier()
    {
        AudioSource.PlayClipAtPoint(ConffetySound, Camera.main.transform.position, volume);
        confetyEffect.Play();
        AudioSource.PlayClipAtPoint(ShoutHappySound, Camera.main.transform.position, volume);
    }

    void PlayCollisionCarVfx(Vector3 collisionPos)
    {
        AudioSource.PlayClipAtPoint(collisionSound, Camera.main.transform.position);
        UtilitiesMethods.makeParticalEffect(collisionPartical, collisionPos);
    }
  

   
    

}
