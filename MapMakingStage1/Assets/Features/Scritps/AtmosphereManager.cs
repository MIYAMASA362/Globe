using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphereManager : Singleton<AtmosphereManager>
{

    public Transform atmosphere;
    public GameObject clouds;
    public float cloudsScaleSmooth = 5f;
    public float cloudsTargetScale = 5f;

    private Vector3 initScale = Vector3.zero;

    void Start()
    {
        initScale = clouds.transform.localScale;
    }

    void Update()
    {
        if (CameraManager.Instance.characterCamera.gameObject.activeInHierarchy)
        {
            clouds.transform.localScale = Vector3.Lerp(clouds.transform.localScale, new Vector3(cloudsTargetScale, cloudsTargetScale, cloudsTargetScale), Time.deltaTime * cloudsScaleSmooth);
        }
        else
        {
            clouds.transform.localScale = Vector3.Lerp(clouds.transform.localScale, initScale, Time.deltaTime * cloudsScaleSmooth);
        }
    }

    private void LateUpdate()
    {
        atmosphere.transform.position = RotationManager.Instance.planetTransform.position + CameraManager.Instance.planetCamera.transform.forward * 0.01f;
    }
}
