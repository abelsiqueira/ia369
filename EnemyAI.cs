using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class EnemyAI : MonoBehaviour 
{

	public enum States
	{
		Patrol,
		Chase
	}
	
	private States state;
	public GameObject player;
	public float minDistance = 3.0f;
	private Vector3 direction;
	public float speed = 1.0f;
	CharacterController controller;
	
	void Start () 
	{
		controller = GetComponent<CharacterController>();
		state = States.Patrol;
		direction = transform.TransformDirection(Vector3.forward);
		player = GameObject.FindGameObjectWithTag("Player");
		StartCoroutine(UpdateMEF());
	}
	
	void Update () 
	{
		controller.SimpleMove(direction * speed);
	}
	
	IEnumerator UpdateMEF ()
	{
		while (true)
		{
			switch (state)
			{
				case States.Patrol:
					if (isPlayerNear()) state = States.Chase;
					else Patrol();
					break;
				case States.Chase:
					if (isPlayerFar()) state = States.Patrol;
					else Chase();
					break;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	bool isPlayerFar()
	{
		Vector3 distance = transform.position - player.transform.position;
		if (distance.magnitude >= minDistance * 4) return true;
		else return false;
	}
	
	void Patrol()
	{
		if(Physics.Raycast(transform.position, direction, 1.0f)) direction *= -1;
	}
	
	bool isPlayerNear()
	{
		Vector3 distance = transform.position - player.transform.position;
		if (distance.magnitude <= minDistance) return true;
		else return false;
	}
	
	void Chase()
	{
		direction = (player.transform.position - transform.position).normalized;
	}
}
