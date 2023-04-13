using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseGazeControllor : MonoBehaviour
{
    public float rotationSpeed = 1f;
    
	void Update()
	{
		transform.eulerAngles += rotationSpeed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
	}
}
