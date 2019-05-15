using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : Singleton<AtmosphereManager> {

    public Material skyMaterial;
    public float skyColorSmooth = 0.1f;
    public float skyFloat;
    public GameObject clouds;
    public float cloudsScaleSmooth = 5f;

    private Vector4 starsTileing1 = new Vector4(6f, 6f, 0f, 0f);
    private Vector4 starsTileing2 = new Vector4(3f, 3f, 0f, 0f);

    // Use this for initialization
    void Start () {
       // circleScale = supportCircle.localScale;
    }
	
	// Update is called once per frame
	void Update ()
    {

        skyMaterial.SetFloat("_sky", skyFloat);
        
        if (CameraManager.Instance.characterCamera.gameObject.activeInHierarchy)
        {
            skyMaterial.SetVector("_StarsTiling", starsTileing1);
            clouds.transform.localScale = Vector3.Lerp(clouds.transform.localScale, new Vector3(5f, 5f, 5f), Time.deltaTime * cloudsScaleSmooth);
            if (skyFloat < .04f)
            {
                skyFloat += Time.deltaTime * skyColorSmooth;
            }
            else
            {
                skyFloat = 0.044f;
            }
        }
        else
        {
            skyMaterial.SetVector("_StarsTiling", starsTileing2);
            clouds.transform.localScale = Vector3.Lerp(clouds.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), Time.deltaTime * cloudsScaleSmooth);
            if (skyFloat >= .01f)
            {
                skyFloat -= Time.deltaTime * skyColorSmooth;
            }
            else
            {
                skyFloat = 0;
            }
        }
    }
}
