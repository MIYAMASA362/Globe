using UnityEngine;
using System.Collections;

public class ScannerEffect : MonoBehaviour
{
	public Material mat;
    public float velocity = 5;
    private bool isScanning;
	public float dis;


    void Start()
	{
		Camera.main.depthTextureMode = DepthTextureMode.Depth;
	}

	void Update()
	{
		if (this.isScanning)
		{
			this.dis += Time.deltaTime * this.velocity;
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			this.isScanning = true;
			this.dis = 0.002f;
		}
	}


	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		mat.SetFloat("_ScanDistance", dis);
        Graphics.Blit(src, dst, mat);
	}
}
