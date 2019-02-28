using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Třídá pro zaměření kamery na hlavního hedinu
/// </summary>
public class CameraController : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0, 0, -10);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + offset;
	}
}
