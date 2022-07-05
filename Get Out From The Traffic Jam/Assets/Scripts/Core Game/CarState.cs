using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CarState : MonoBehaviour
{
    [SerializeReference]protected int carSpeed = 25;
    [SerializeReference]protected int carTurnSpeed = 500;
    public carAreaState currentCarAreaState = carAreaState.parkingAreaState;
    protected Rigidbody carRigidbody = default;
    protected Collider carCollider = default;
    protected Animator carAnimator= default;
    protected AudioSource driveSoundSource = default;
    protected SpriteRenderer happyEmojiRenderer;
    protected void CatchComponents()
    {
        carRigidbody = GetComponent<Rigidbody>();
        driveSoundSource = GetComponent<AudioSource>();
        carAnimator = GetComponent<Animator>();
        carCollider = GetComponent<Collider>();
        happyEmojiRenderer = gameObject.transform.Find("happy emoji").GetChild(0).GetComponent<SpriteRenderer>();
    }
    public enum carAreaState
    {
        parkingAreaState,
        pathAreaState,
    }
}


 
   
   

    





