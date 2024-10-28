using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{
	private Rigidbody2D rb;
	public SwordCtrl swordCtrl;
	private Animator anim;
	
	[SerializeField] private float spd;
	[SerializeField] GameObject sprite;
	[SerializeField] GameObject trail;
	[SerializeField] private Transform sword;
	[SerializeField] private Transform shake_pivot;

	private Vector2 v2_movement;
	private float attack_charge;
	private bool charging;
	private bool canCharge;

	private float chargeTime = 1;
	private float chargeSpeed = 0.25f;

	int vidas;
	float recover_time;
	Vector3 camPos;
	
	float camHeight;
	float camWidth;

	private void Awake()
	{
		camHeight = Camera.main.orthographicSize;
		camWidth = Camera.main.aspect * camHeight;

		camPos = Camera.main.transform.position;
		vidas = 2;
		trail.SetActive(false);
		attack_charge = 0;
		canCharge = true;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	void Update()
    {
		Movement();
		Attack();
		LifeCtrl();
		swordCtrl.transform.localPosition = shake_pivot.position;
		swordCtrl.transform.up = transform.position - swordCtrl.transform.position;
		if (!canCharge)
		{
			trail.SetActive(true);
			if (attack_charge > 0)
			{
				attack_charge -= Time.deltaTime;

				if (transform.localScale == new Vector3(1, 1, 1))
					sword.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, attack_charge / chargeSpeed));
				else
					sword.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(360, 0, attack_charge / chargeSpeed));
			}
			else
			{
				swordCtrl.GetComponent<SwordCtrl>().canParry = false;
				canCharge = true;
			}
			return;
		}
		else if (attack_charge > chargeTime)
				swordCtrl.transform.position = shake_pivot.transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
		trail.SetActive(false);
	}

	private void LifeCtrl()
	{
		if (vidas > 1)
			return;

		Camera.main.transform.position = camPos + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);

		if (recover_time > 0)
			recover_time -= Time.deltaTime;
		else
		{
			Camera.main.transform.position = camPos;
			vidas++;
		}

		if (vidas < 1)
			SceneManager.LoadScene(1);
	}

	private void Movement()
	{
		if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
			v2_movement.x = -1;
		else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
			v2_movement.x = 1;
		else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			v2_movement.x = -1;
		else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
			v2_movement.x = 1;
		else
			v2_movement.x = 0;

		if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
			v2_movement.y = 1;
		else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.UpArrow))
			v2_movement.y = -1;
		else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
			v2_movement.y = 1;
		else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
			v2_movement.y = -1;
		else
			v2_movement.y = 0;

		if (v2_movement != Vector2.zero)
			anim.SetBool("walking", true);
		else
			anim.SetBool("walking", false);

		if (v2_movement.x < 0)
		{
			swordCtrl.transform.localScale = new Vector3(1, 1, 1);
			transform.localScale = new Vector3(1, 1, 1);
		}
		else if (v2_movement.x > 0)
		{
			swordCtrl.transform.localScale = new Vector3(-1, 1, 1);
			transform.localScale = new Vector3(-1, 1, 1);
		}

		rb.velocity = v2_movement.normalized * spd * Time.deltaTime;

		if (transform.position.x > camWidth - 1)
			transform.position = new Vector2(camWidth - 1, transform.position.y);
		if (transform.position.x < -camWidth + 1)
			transform.position = new Vector2(-camWidth + 1, transform.position.y);

		if (transform.position.y > camHeight - 1)
			transform.position = new Vector2(transform.position.x, camHeight - 1);
		if (transform.position.y < -camHeight + 1)
			transform.position = new Vector2(transform.position.x, -camHeight + 1);
	}

	private void Attack()
	{
		if (!canCharge)
			return;

		if (Input.GetKey(KeyCode.Space))
			charging = true;

		if (Input.GetKeyUp(KeyCode.Space))
			DoAttack();

		if (charging)
		{
			if (attack_charge > chargeTime)
				attack_charge = chargeTime;

			attack_charge += Time.deltaTime;

			if (transform.localScale == new Vector3(1, 1, 1))
				sword.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, attack_charge / chargeTime));
			else
				sword.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(360, 0, attack_charge / chargeTime));
		}
	}

	private void DoAttack()
	{
		swordCtrl.GetComponent<SwordCtrl>().canParry = true;
		charging = false;
		canCharge = false;

		attack_charge = Mathf.Lerp(0, chargeSpeed, attack_charge / chargeTime);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Sword")
			return ;
		if (collision.gameObject.tag == "Phrase")
			collision.gameObject.SetActive(false);
		if (collision.gameObject.tag == "Hand")
			collision.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

		vidas--;
		recover_time = 5;
	}
}