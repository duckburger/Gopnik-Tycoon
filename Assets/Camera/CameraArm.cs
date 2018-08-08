using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArm : MonoBehaviour
{

    public float cameraSpeed;
    public float zoomSenstitivity;
    public bool cameraMovementOn = true;

    private float minCameraZoom = 4.45f;
    private float maxCameraZoom = 9f;


    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.M))
        {
            cameraMovementOn = !cameraMovementOn;
        }
        if (cameraMovementOn)
        {
            MoveCamera();
        }

    }

    void MoveCamera()
    {
        Vector3 camPos = transform.position;



        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            camPos.x += cameraSpeed * Time.deltaTime * 2;
            camPos.x = Mathf.Clamp(camPos.x, -10, 10);
            transform.position = camPos;

        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            camPos.x -= cameraSpeed * Time.deltaTime * 2;
            camPos.x = Mathf.Clamp(camPos.x, -10, 10);
            transform.position = camPos;
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            camPos.y += cameraSpeed * Time.deltaTime * 2;
            camPos.y = Mathf.Clamp(camPos.y, -10, 10);
            transform.position = camPos;

        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            camPos.y -= cameraSpeed * Time.deltaTime * 2;
            camPos.y = Mathf.Clamp(camPos.y, -10, 10);
            transform.position = camPos;
        }



        /* //TODO reenable the mouse camera movement
		if (Input.mousePosition.x > Screen.width - 5)
		{
			camPos.x += cameraSpeed * Time.deltaTime;
			camPos.x = Mathf.Clamp(camPos.x, -10, 10);
			transform.position = camPos;
		}
		else if (Input.mousePosition.x < 5)
		{
		
			camPos.x -= cameraSpeed * Time.deltaTime;
			camPos.x = Mathf.Clamp(camPos.x, -10, 10);

			transform.position = camPos;

		}

		else if (Input.mousePosition.y > Screen.height - 5)
		{
			
			camPos.y += cameraSpeed * Time.deltaTime;
			camPos.y = Mathf.Clamp(camPos.y, -10f, 10f);
			transform.position = camPos;

		}
		else if (Input.mousePosition.y < 5)
		{
			
			camPos.y -= cameraSpeed * Time.deltaTime;
			camPos.y = Mathf.Clamp(camPos.y, -10f, 10f);
			transform.position = camPos;

		}
		*/
        float camZoom = Camera.main.orthographicSize;


        camZoom += zoomSenstitivity * -Input.GetAxis("Mouse ScrollWheel");
        camZoom = Mathf.Clamp(camZoom, minCameraZoom, maxCameraZoom);



        Camera.main.orthographicSize = camZoom;



    }

}
