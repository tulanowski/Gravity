using UnityEngine;
using System.Collections;

public class LookCamera : MonoBehaviour 
{
    public float moveSpeed = 10f;
	public float rotationSpeed = 0.5f;
    public float mouseSensitivity = 5f;
    
	
	void Start()
	{

	}

	void Update()
	{
		float xRot = 0f;
		float yRot = 0f;
		float zRot = Input.GetAxis("Rotation") * rotationSpeed;

		float xMove = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		float zMove = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
		
		if (Input.GetMouseButton(1)) 
        {
			xRot = -Input.GetAxis("Mouse Y") * mouseSensitivity;
			yRot = Input.GetAxis("Mouse X") * mouseSensitivity;
        }
		transform.Rotate(xRot, yRot, zRot);
		transform.Translate(xMove, 0, zMove);
	}
}
