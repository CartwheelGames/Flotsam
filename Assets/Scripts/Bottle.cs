using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour {

    Wave wave;

	// Use this for initialization
	void Start () {
        wave = GameObject.Find("wave").GetComponent<Wave>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(0, wave.GetHeightAtXPos(0));

	}
}
