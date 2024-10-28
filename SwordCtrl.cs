using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCtrl : MonoBehaviour
{
    public bool canParry;
	public GameCtrl gameCtrl;

	private void Awake()
	{
        canParry = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

        if (collision.gameObject.CompareTag("Phrase"))
        {
			if (canParry)
			{
				collision.gameObject.GetComponent<SpriteRenderer>().sprite = collision.gameObject.GetComponent<PhraseCtrl>().spr_good;
				collision.gameObject.GetComponent<PhraseCtrl>().dir = -1;
				collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
				gameCtrl.IncreaseLv();
			}
			else if (collision.gameObject.GetComponent<PhraseCtrl>().dir != -1)
				collision.gameObject.SetActive(false);
        }
	}
}
