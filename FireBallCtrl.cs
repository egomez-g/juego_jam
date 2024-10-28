using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCtrl : MonoBehaviour
{
	public int dir = 1;
	public int spd;
	public int currSpd;
	private Rigidbody2D rb;

	void Awake()
	{
		currSpd = spd;
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetRotation(Transform pos)
	{
		transform.up = pos.position - transform.position;
	}

	void Update()
	{
		if (dir == 1)
			rb.velocity = transform.up * currSpd * dir * Time.deltaTime;
		else
			rb.velocity = transform.up * currSpd * 3 * dir * Time.deltaTime;

		if (transform.position.y > 6 || transform.position.z < -6 ||
			transform.position.x > 7 || transform.position.x < -7)
			gameObject.SetActive(false);
	}
}
