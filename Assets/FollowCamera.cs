using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Vector3 targetPosition;
    Vector3 currentPosition;
    public Vector3 cameraOffset;

    public float followSmoothTime;
    public float lookSmoothTime;
    public Transform target;
    public Transform camera;

    GameObject hiddenWall;

    // Start is called before the first frame update
    void Start()
    {    
        targetPosition = target.position;
        currentPosition = target.position + target.rotation * cameraOffset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = Vector3.zero;
        targetPosition = Vector3.SmoothDamp(targetPosition, target.position, ref velocity, lookSmoothTime);
        currentPosition = Vector3.SmoothDamp(currentPosition, target.position + target.rotation * cameraOffset, ref velocity, followSmoothTime);
        camera.position = currentPosition;
        camera.LookAt(targetPosition, Vector3.up);
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        
        Vector3 dir = target.position - camera.position;
        if (Physics.Raycast(camera.position, dir, out hit, dir.magnitude, 1 << 6))
        {
            if (hiddenWall && hit.collider.gameObject != hiddenWall) {
                hiddenWall.GetComponent<MeshRenderer>().enabled = true;
                hiddenWall = null;
            }
            else if (!hiddenWall) {
                hiddenWall = hit.collider.gameObject;
                hiddenWall.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else if (hiddenWall) {
            hiddenWall.GetComponent<MeshRenderer>().enabled = true;
            hiddenWall = null;
        }
    }

}
