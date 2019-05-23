using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager :Singleton<StageManager>
{
    [System.Serializable]
    public class Galaxy
    {
        public string GalaxyName = "";
        public string[] planets = new string[5];
    }

    [SerializeField] public Galaxy[] galaxies = new Galaxy[4];

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
