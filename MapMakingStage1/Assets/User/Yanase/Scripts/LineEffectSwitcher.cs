using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffectSwitcher : MonoBehaviour {

    [SerializeField] private Material lineMaterial;
    public bool isPlay = false;
    public float playSpeed = 1.0f;
    public float playTime = 0.0f;
    public float maxPlayTime = 10.0f;

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(isPlay)
        {
            lineMaterial.SetFloat("_LineTime", playTime);
            playTime += Time.deltaTime * playSpeed;
            if(playTime >= maxPlayTime)
            {
                playTime = 0.0f;
                isPlay = false;
                GetComponent<Renderer>().enabled = false;
            }
        }

        Vector3 mainCameraPos = CameraManager.Instance.mainCamera.transform.position;

        transform.localPosition = (mainCameraPos - transform.localPosition).normalized * 0.001f;
	}

    public void SetEffect(Vector3 centerPosition, Color lineColor)
    {
        lineMaterial.SetVector("_CenterPosition", centerPosition);
        lineMaterial.SetColor("_LineColor", lineColor);
        lineMaterial.SetColor("_LineEmissionColor", lineColor);
        lineMaterial.SetFloat("_LineEmisionValue", 5.0f);
        playTime = 0.0f;
        isPlay = true;
        GetComponent<Renderer>().enabled = true;
    }
}
