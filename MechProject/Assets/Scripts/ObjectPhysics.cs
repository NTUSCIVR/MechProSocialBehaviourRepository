﻿using UnityEngine;

// Add onto "Pusher" GameObject
// Used to Push objects in the direction of "Pusher"
// Note: Both "Pusher" and "Being Pushed" GameObjects require Rigidbody Component and Collider Component
public class ObjectPhysics : MonoBehaviour
{
    [Tooltip("Force of pushing.(Higher means object pushed further;Lower means object pushed closer) Default: 3.0f")]
    [SerializeField]
    private float PushPower = 3.0f;

    [SerializeField]
    private GameObject LastDebris;
    [SerializeField]
    private Animator AmbulanceAnimator;
    [SerializeField]
    private AudioSource AmbulanceSiren;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == LastDebris)
        {
            if (!AmbulanceAnimator.GetCurrentAnimatorStateInfo(0).IsName("Drive"))
            {
                AmbulanceAnimator.Play("Drive");
                AmbulanceSiren.Play();
            }
        }

        Rigidbody CollidedBody = collision.rigidbody;
        Rigidbody MyBody = GetComponent<Rigidbody>();

        if (!CollidedBody || CollidedBody.isKinematic)
            return;

        Vector3 PushDirection = new Vector3(MyBody.velocity.x, MyBody.velocity.y, MyBody.velocity.z);

        CollidedBody.AddForce(PushDirection * PushPower);
    }
}
