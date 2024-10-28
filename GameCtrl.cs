using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GameCtrl : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject frase;
    GameObject[] frases;

    public GameObject hand;
    public Transform handPivot;

    public GameObject fireball;
    GameObject[] fireballs;
    
    float PhraseTimer = 0;
    float PhraseSpawnTimer = 2;
	float HandTimer = 0;
	float HandSpawnTimer = 4;
	float FireTimer = 0;
	float FireSpawnTimer = 4;
	float FireSpawningTime = 0;
	float FireDelay = .25f;
	float FireDelayTime = 0;

	Vector2 fireballPos;

	[SerializeField] private Gradient bg_colors;
    [SerializeField] private Gradient linecolors;
    [SerializeField] private GameObject bg_1;
    [SerializeField] private GameObject bg_11;
    [SerializeField] private GameObject bg_2;
    private int level;
    private int max_level = 30;

    float camHeight;
    float camWidth;

	void Awake()
	{
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 30;

		camHeight =Camera.main.orthographicSize;
		camWidth = Camera.main.aspect * camHeight;
		
		fireballPos = new Vector2(Random.Range(-camWidth + 2, camWidth - 2), -8);
		level = 0;
        frases = new GameObject[5];
        fireballs = new GameObject[20];
		for (int i = 0; i < 5; i++)
		{
			GameObject prefab = Instantiate(frase);
			frases[i] = prefab;
			frases[i].SetActive(false);
			prefab.GetComponent<PhraseCtrl>().gameCtrl = this;
		}
		for (int i = 0; i < 20; i++)
		{
			GameObject prefab = Instantiate(fireball);
			fireballs[i] = prefab;
			fireballs[i].SetActive(false);
		}
	}

	private void Update()
	{        
        SpawnPhrases();
        SpawnHand();
        SpawnFireballs();
	}

	private void SpawnPhrases()
	{
		PhraseTimer += Time.deltaTime;
		if (PhraseTimer > PhraseSpawnTimer)
		{
			PhraseSpawnTimer = Random.Range(1.0f, 2.0f);
			GameObject newfrase = getPhraseFromPool();

			PhraseTimer = 0;
			if (!newfrase)
				return;

			if (Random.Range(0, 2) == 1)
				newfrase.transform.position = new Vector2(Random.Range(-6.0f, 6.0f), 5);
			else
				newfrase.transform.position = new Vector2(Random.Range(-6.0f, 6.0f), -5);
			newfrase.GetComponent<PhraseCtrl>().SetRotation(playerTransform);

			newfrase.GetComponent<PhraseCtrl>().dir = 1;
			newfrase.GetComponent<SpriteRenderer>().sprite = newfrase.GetComponent<PhraseCtrl>().spr_bad;
			newfrase.SetActive(true);
		}
	}

	private void SpawnHand() 
    {
        if (level < 10)
            return;

		HandTimer += Time.deltaTime;
		if (HandTimer > HandSpawnTimer)
        {
            hand.GetComponentInChildren<CapsuleCollider2D>().enabled = true;
            handPivot.position = new Vector3(0, Random.Range(0, -1.5f * camHeight), 0);
            HandTimer = 0;
			hand.GetComponent<Animator>().SetInteger("dir", Random.Range(0, 2));
            hand.GetComponent<Animator>().SetTrigger("do");
		}
	}

    private void SpawnFireballs()
    {
		if (level < 15)
            return;

		FireDelayTime += Time.deltaTime;
		FireTimer += Time.deltaTime;

		if (FireTimer > FireSpawnTimer)
        {
			if (!(FireSpawningTime < 1.0f && FireSpawningTime > .5f))
			{
				if (FireDelayTime > FireDelay)
				{
					GameObject prefab1 = getFireBallFromPool();
					if (prefab1)
					{
						prefab1.transform.position = fireballPos;
						prefab1.SetActive(true);
					}
					GameObject prefab2 = getFireBallFromPool();
					if (prefab2)
					{
						prefab2.transform.position = fireballPos + new Vector2(1.5f,0);
						prefab2.SetActive(true);
					}
					FireDelayTime = 0;
	            }
			}
            FireSpawningTime += Time.deltaTime;

			if (FireSpawningTime < 0.75f && FireSpawningTime > 0.5f) 
			{
				foreach (GameObject go in fireballs)
				{
					go.GetComponent<FireBallCtrl>().currSpd = 0;
				}
			}
			else if (FireSpawningTime > 1.0f && FireSpawningTime < 1.2f)
			{
				foreach (GameObject go in fireballs)
				{
					go.GetComponent<FireBallCtrl>().currSpd = go.GetComponent<FireBallCtrl>().spd;
				}
			}

			if (FireSpawningTime > 4)
            {
				fireballPos = new Vector2(Random.Range(-camWidth + 2, camWidth - 2), -8);
				FireSpawningTime = 0;
                FireTimer = 0;
				FireDelayTime = 0;
            }
        }
    }

    public void IncreaseLv()
    {
        level++;
        bg_1.GetComponent<SpriteRenderer>().color = bg_colors.Evaluate(Mathf.Lerp(0, 1, (float)level / max_level));
        bg_11.GetComponent<SpriteRenderer>().color = bg_colors.Evaluate(Mathf.Lerp(0, 1, (float)level / max_level));
        bg_2.GetComponent<SpriteRenderer>().color = linecolors.Evaluate(Mathf.Lerp(0, 1, (float)level / max_level));
		if (level == max_level)
			SceneManager.LoadScene(2);
	}

	private GameObject getPhraseFromPool()
	{
		for (int i = 0; i < frases.Length; i++)
		{
			if (!frases[i].activeSelf)
				return frases[i];
		}
		return null;
	}

	private GameObject getFireBallFromPool()
	{
		for (int i = 0; i < fireballs.Length; i++)
		{
			if (!fireballs[i].activeSelf)
				return fireballs[i];
		}
		return null;
	}
}
