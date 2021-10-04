using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{

    public List<WheelCollider> leftColliders;
    public List<WheelCollider> rightColliders;
    public float motorTorque;
    public float brakeTorque;
    public float maxWheelSpeed;
    public Rigidbody rigidbody;
    WheelFrictionCurve slowFriction;
    WheelFrictionCurve fastFriction;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        slowFriction = leftColliders[0].sidewaysFriction;
        slowFriction.extremumValue = 0.5f;
        slowFriction.asymptoteValue = 0.2f;
        fastFriction = leftColliders[0].sidewaysFriction;
        fastFriction.extremumValue = 1f;
        fastFriction.asymptoteValue = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        float throttle = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");
        foreach (WheelCollider wc in leftColliders) {
            float overSpeed = Mathf.Abs(wc.rpm) / maxWheelSpeed;
            overSpeed = 0f;
            float maxTorque = Mathf.Lerp(motorTorque, 0f, overSpeed);
            float brake = Mathf.Lerp(0, brakeTorque, (Mathf.Abs(wc.rpm) - maxWheelSpeed) / maxWheelSpeed);
            if (!wc.isGrounded) {
                maxTorque = 0f;
            }
            wc.motorTorque = (throttle + Mathf.Sign(throttle) * steering) * maxTorque;

            if (rigidbody.velocity.magnitude > 1f) {
                wc.sidewaysFriction = fastFriction;
            }
            else  {
                wc.sidewaysFriction = slowFriction;
            }

            if (Mathf.Abs(throttle) < 0.05f && Mathf.Abs(steering) < 0.05f) {
                wc.brakeTorque = brakeTorque;
            }
            else {
                wc.brakeTorque = 0f;
            }
        }
        foreach (WheelCollider wc in rightColliders) {
            float overSpeed = Mathf.Abs(wc.rpm) / maxWheelSpeed;
            overSpeed = 0f;
            float maxTorque = Mathf.Lerp(motorTorque, 0f, overSpeed);
            float brake = Mathf.Lerp(0, brakeTorque, (Mathf.Abs(wc.rpm) - maxWheelSpeed) / maxWheelSpeed);
            if (!wc.isGrounded) {
                maxTorque = 0f;
            }
            wc.motorTorque = (throttle - Mathf.Sign(throttle) * steering) * maxTorque;
            //wc.brakeTorque = brake;
            if (rigidbody.velocity.magnitude > 1f) {
                wc.sidewaysFriction = fastFriction;
            }
            else  {
                wc.sidewaysFriction = slowFriction;
            }

            if (Mathf.Abs(throttle) < 0.05f && Mathf.Abs(steering) < 0.05f) {
                wc.brakeTorque = brakeTorque;
            }
            else {
                wc.brakeTorque = 0f;
            }
        }
    }

    void OnDrawGizmos()
    {   
        Gizmos.color = Color.red;
        foreach (WheelCollider wc in leftColliders) {
            Gizmos.DrawLine(wc.transform.position, wc.transform.position + wc.transform.forward * (wc.motorTorque / motorTorque));
        }
        foreach (WheelCollider wc in rightColliders) {
            Gizmos.DrawLine(wc.transform.position, wc.transform.position + wc.transform.forward * (wc.motorTorque / motorTorque));
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(rigidbody.transform.position, rigidbody.transform.position + rigidbody.transform.forward * rigidbody.velocity.magnitude);
    }
}
