using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadProxy : MonoBehaviour
{
    Vector3 forceVector = Vector3.zero;
    Vector3 lastVelocity = Vector3.zero;
    Vector3 lastPosition = Vector3.zero;
    Vector3 samplingPosition = Vector3.zero;

    public float followSpeed;

    // Start is called before the first frame update
    void Start()
    {    
        lastPosition = transform.position;
        samplingPosition = lastPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = transform.position - lastPosition;
        forceVector = velocity - lastVelocity;

        samplingPosition = Vector3.MoveTowards(samplingPosition, lastPosition, followSpeed * Time.deltaTime);

        lastPosition = transform.position;
        lastVelocity = velocity;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forceVector * 50f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(samplingPosition, 0.05f);
    }
}
