using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OutroCtrl : MonoBehaviour
{
    [SerializeField] private Sprite [] frames;
    public Image image;
    int i = 0;

    float timer;
	private void Awake()
	{
        timer = 2;
		image.sprite = frames[i];
	}

	void Update()
    {
        timer -= Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Space) && timer < 0)
        {
            ++i;
            if (i < frames.Length)
                image.sprite = frames[i];
        }        
    }
}
