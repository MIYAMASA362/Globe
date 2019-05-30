using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disolve : MonoBehaviour {

    public Material material;
    public float disolveAmount = 0.0f;
    public float smooth = 1.0f;
    private bool fade = false;
    private bool unFade = false;

    private void Awake()
    {
        material.SetFloat("_DissolveAmount", disolveAmount);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(fade)
        {
            disolveAmount += Time.deltaTime * smooth;
            if (disolveAmount >= 1.0f)
            {
                fade = false;
                disolveAmount = 1.0f;
            }
        }
        if (unFade)
        {
            disolveAmount -= Time.deltaTime * smooth;
            if (disolveAmount <= 0.0f)
            {
                unFade = false;
                disolveAmount = 0.0f;
            }
        }

        disolveAmount = Mathf.Clamp01(disolveAmount);

        material.SetFloat("_DissolveAmount", disolveAmount);
    }

    public void SetDisolve(float disolve)
    {
        disolveAmount = disolve;
    }

    public void FadeDsiolve()
    {
        fade = true;
    }

    public void UnFadeDsiolve()
    {
        unFade = true;
    }

    public bool isFade()
    {
        return (fade || unFade);
    }
}
