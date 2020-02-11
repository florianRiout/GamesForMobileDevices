using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    Controllable selectedObject;
    Camera my_camera;
    float timer;
    bool isTap;
    float distance;
    float distanceFingers;
    Quaternion currentRotationCube;
    float angleFingers;
    float rotationSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        my_camera = Camera.main;
        selectedObject = null;
        timer = 0f;
        isTap = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.1f)
        {
            isTap = false;
        }
        if (Input.touchCount == 1)
        {
            List<Touch> touches = new List<Touch>(Input.touches);
            foreach (Touch touch in touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    isTap = true;
                    timer = 0f;
                }
                Ray my_ray = my_camera.ScreenPointToRay(touch.position);
                Debug.DrawRay(my_ray.origin, 20 * my_ray.direction);
                if (Physics.Raycast(my_ray, out RaycastHit info_on_hit))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector3 positionCube = info_on_hit.collider.gameObject.transform.position;
                        Vector3 positionCamera = my_camera.transform.position;
                        distance = Mathf.Sqrt(Mathf.Pow(positionCube.x - positionCamera.x, 2)
                                                + Mathf.Pow(positionCube.y - positionCamera.y, 2)
                                                + Mathf.Pow(positionCube.z - positionCamera.z, 2));
                    }
                    if (touch.phase == TouchPhase.Ended && isTap)
                    {
                        if (selectedObject == null)
                        {
                            selectedObject = info_on_hit.transform.GetComponent<Controllable>();
                            selectedObject.Select();
                        }
                        else if (selectedObject != info_on_hit.transform.GetComponent<Controllable>())
                        {
                            selectedObject.Unselect();
                            selectedObject = info_on_hit.transform.GetComponent<Controllable>();
                            selectedObject.Select();
                        }
                        else
                        {
                            selectedObject.Unselect();
                            selectedObject = null;
                        }
                    }
                }
                else if (selectedObject != null && isTap && touch.phase == TouchPhase.Ended)
                {
                    selectedObject.Unselect();
                    selectedObject = null;
                }
                if (touch.phase == TouchPhase.Moved && selectedObject != null && !isTap)
                {
                    selectedObject.MoveTo(my_ray.GetPoint(distance));
                }
                else if(selectedObject == null)
                {

                }
            }
        }
        else if (Input.touchCount == 2)
        {
            if(selectedObject != null)
            {
                Touch finger1 = Input.GetTouch(0);
                Touch finger2 = Input.GetTouch(1);

                float newDistanceFingers = Vector2.Distance(finger1.position, finger2.position);

                float rotateAngle = 0;
                float newAngleFingers = Mathf.Abs(Mathf.Atan2(finger2.position.y - finger1.position.y, finger2.position.x - finger1.position.x));
                if(newAngleFingers > angleFingers)
                {
                    rotateAngle = newAngleFingers - angleFingers;
                }
                else if(angleFingers > newAngleFingers)
                {
                    rotateAngle = -(angleFingers - newAngleFingers);
                }
                    angleFingers = newAngleFingers;


                if (distanceFingers > 0 && newDistanceFingers > distanceFingers)
                    selectedObject.ScaleUp((newDistanceFingers - distanceFingers) / 100);
                else if (distanceFingers > 0 && newDistanceFingers < distanceFingers)
                    selectedObject.ScaleDown((distanceFingers - newDistanceFingers) / 100);

                distanceFingers = newDistanceFingers;

                if(finger1.phase == TouchPhase.Ended || finger2.phase == TouchPhase.Ended)
                {
                    distanceFingers = 0;
                    angleFingers = 0;
                }

                if (finger1.phase == TouchPhase.Began || finger2.phase == TouchPhase.Began)
                {
                    currentRotationCube = selectedObject.transform.rotation;
                    angleFingers = Mathf.Abs(Mathf.Atan2(finger2.position.y - finger1.position.y, finger2.position.x - finger1.position.x));
                }

                if (finger1.phase == TouchPhase.Moved || finger2.phase == TouchPhase.Moved)
                {
                    Quaternion newRotationCube = Quaternion.AngleAxis(rotateAngle * rotationSpeed, my_camera.transform.forward);
                    selectedObject.transform.rotation = currentRotationCube * newRotationCube;
                    currentRotationCube = selectedObject.transform.rotation;
                }
            }
            else
            {

            }
        }
    }
}
