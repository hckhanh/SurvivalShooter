﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float speed = 6f;
	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;

	void Awake ()
	{
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate ()
	{
		float h = Input.GetAxisRaw ("Horizontal"); // GetAxisRaw ONLY returns -1, 0, 1.
		float v = Input.GetAxisRaw ("Vertical");

		Move (h, v); // Move the charater by using keyboards (WASD)

		Turning (); // Turn the charrater follow the mouse position

		Animating (h, v); // Animating Idle or Walking animate clip
	}

	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning ()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRigidbody.MoveRotation (newRotation);
		}

	}

	void Animating (float h, float v)
	{
		bool isWalking = h != 0 || v != 0;
		anim.SetBool ("IsWalking", isWalking);
	}

}
