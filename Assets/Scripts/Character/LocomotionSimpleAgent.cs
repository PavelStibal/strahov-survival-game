﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionSimpleAgent : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Don’t update position automatically
        agent.updatePosition = false;
    }

    void Update()
    {
		Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

		// Map 'worldDeltaPosition' to local space
		float dx = Vector3.Dot(transform.right, worldDeltaPosition);
		float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
		Vector2 deltaPosition = new Vector2(dx, dy);
		
		// Low-pass filter the deltaMove
		float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
		smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
		
		// Update velocity if time advances
		if (Time.deltaTime > 1e-5f)
			velocity = smoothDeltaPosition / Time.deltaTime;
		
		bool shouldMove = velocity.magnitude > 0.5f && (agent.remainingDistance > agent.radius);
		
		// Update animation parameters
		anim.SetBool("isWalking", shouldMove);

    }

	void OnAnimatorMove() 
	{
		// Move the transform
		transform.position = agent.nextPosition;
	}

 }