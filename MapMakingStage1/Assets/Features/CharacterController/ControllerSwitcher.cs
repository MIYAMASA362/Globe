using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSwitcher : MonoBehaviour {


	public bool enabled;
	public KeyCode keycode;
	public GameObject charCam;

	public Material skyMat;
	
	public float skyFloat;
	public float colorSmooth = 2;

	public GameObject clouds;
	public float cloudsScaleSmooth  = 2;

	
	


	void Update()
	{
		if(Input.GetKeyDown(keycode))
		{
			enabled = !enabled;
			if(enabled)
			{
				charCam.SetActive(true);
			}
			else
			{
				charCam.SetActive(false);
			}

		}
		skyMat.SetFloat("_sky",skyFloat);


        if (enabled){
			clouds.transform.localScale  = Vector3.Lerp(clouds.transform.localScale,new Vector3(5f,5f,5f),Time.deltaTime * cloudsScaleSmooth);
			if(skyFloat < .04f){
			skyFloat+= Time.deltaTime * colorSmooth;
			}else{
				skyFloat = 0.044f;
			}

		}else{
			clouds.transform.localScale  = Vector3.Lerp(clouds.transform.localScale,new Vector3(1f,1f,1f),Time.deltaTime * cloudsScaleSmooth);
			if(skyFloat >= .01f){
			skyFloat-= Time.deltaTime * colorSmooth;
			}else{
				skyFloat = 0;
			}

		}
		

	}

	
}
