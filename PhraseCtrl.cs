using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PhraseCtrl : MonoBehaviour
{
    public int dir = 1;
    public int spd;
    private Rigidbody2D rb;
	public Sprite spr_bad;
	public Sprite spr_good;
    private Vector2 v2dir;
    public GameCtrl gameCtrl;

	void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
    }

    public void SetRotation(Transform pos)
	{
		v2dir = pos.position - transform.position;
    }

    void Update()
    {
        if (dir == 1) 
            rb.velocity = v2dir.normalized * spd * dir * Time.deltaTime;
        else
			rb.velocity = v2dir.normalized * spd * 3 * dir * Time.deltaTime;
        if (transform.position.y > 6 || transform.position.z < -6 ||
            transform.position.x > 7 || transform.position.x < -7)
        {
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			gameObject.SetActive(false);
        }
	}
}
