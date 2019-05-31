using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : Singleton<AtmosphereManager>
{

    public Transform planetAtmosphere;
    public GameObject clouds = null;
    public float cloudsScaleSmooth = 5f;
    public float cloudsTargetScale = 5f;

    private Vector3 targetScale;

    private Vector3 initScale = Vector3.zero;

    void Start()
    {
        if(clouds != null)
            initScale = clouds.transform.localScale;
    }

    void Update()
    {
        if (CameraManager.Instance.characterCamera.gameObject.activeInHierarchy)
        {
            if(clouds != null)
                targetScale = new Vector3(cloudsTargetScale, cloudsTargetScale, cloudsTargetScale);
        }
        else
        {
            targetScale = initScale;
        }

        if(clouds != null)
            clouds.transform.localScale = Vector3.Lerp(clouds.transform.localScale, initScale, Time.deltaTime * cloudsScaleSmooth);
    }

    private void LateUpdate()
    {
        planetAtmosphere.transform.position = RotationManager.Instance.planetTransform.position + Camera.main.transform.forward * 0.01f;
    }
}
