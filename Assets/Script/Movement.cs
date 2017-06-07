using UnityEngine;

public class Movement : MonoBehaviour {

    Vector3 pos;
    public float speed;
    float camLength = 100f;


    void Start () {
        pos = transform.position;
	}
	
	void FixedUpdate () {
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;

        GetComponent<Rigidbody>().velocity = new Vector3(h, 0.0f, v);
	}
}
