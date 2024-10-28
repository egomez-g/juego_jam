using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroCtrl : MonoBehaviour
{
    public GameObject txt;
    [SerializeField] private Sprite [] frames;
    public Image image;
    int i = 0;

	private void Awake()
	{
		image.sprite = frames[i];
	}

	void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            txt.SetActive(false);
            ++i;
            if (i < frames.Length)
                image.sprite = frames[i];
            else
                SceneManager.LoadScene(1);
        }        
    }
}
