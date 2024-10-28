using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
	AudioCtrl instance;
    void Awake()
    {
		DontDestroyOnLoad(this.gameObject);
		//if (instance == null)
		//{
		//	instance = this;
		//	DontDestroyOnLoad(this.gameObject);
		//}
		//else
		//{
		//	Destroy(this.gameObject);
		//}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
