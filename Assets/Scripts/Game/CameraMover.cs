using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {

    public float distance = 30;
    public float height = 10;
    public Transform target;
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

        this.transform.position = target.position - target.forward * distance;
        this.transform.position = this.transform.position + Vector3.up * height;
        this.transform.LookAt(target);

	}
}
