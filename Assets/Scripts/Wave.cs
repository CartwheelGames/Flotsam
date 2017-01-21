using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {

    float scrollPos;
    Renderer rend;
    float scrollVel;

	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        scrollVel = 5;
    }

    public float GetHeightAtXPos(float xPos)
    {
        return Mathf.Sin((((xPos + (scrollPos - 5)) / 5) * Mathf.PI) + Mathf.PI) * 2.5f;
    }

    public float GetSlopeAtXPos(float xPos)
    {
        return 0;
    }
	
	// Update is called once per frame
	void Update () {
        scrollPos += Time.deltaTime * scrollVel;
        scrollPos = scrollPos % 10;
        transform.position = new Vector2(scrollPos, 0);

        if (Input.GetMouseButtonDown(0))
        {
            scrollVel = -scrollVel;
        }
	}
}
