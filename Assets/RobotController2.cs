using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController2 : MonoBehaviour
{

    IActivatable targetActivatable = null;
    float rotation = 0f;
    Quaternion qrot;
    float speed = 0f;
    float newSpeed = 0f;
    float releaseStopped = 0f;
    public float maxSpeed;
    public float acceleration;
    public float breaking;
    public float rotationSpeed;
    public float drag;
    public float stoppedTime;
    public float gravity;
    public AudioSource rollingAudio;
    public AudioSource chemicalAudio;

    Vector3 forceVector;
    Rigidbody rigidbody;

    float updateTime;
    GameObject carryObject = null;
    bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>(); 
        qrot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float velocityDiff = acceleration;
        newSpeed = 0f;
        if (releaseStopped > 0f) {
            releaseStopped -= Time.deltaTime; 
        }
        else if (isGrounded) {
            //float forward Vector3.Dot(rigidbody.velocity.normalized, new Vector3(transform.forward.normalized.x, 0f, transform.forward.normalized.z)));
            qrot.eulerAngles += Vector3.up * rotationSpeed * horizontal * Mathf.Sign(speed) * Time.deltaTime;
            newSpeed = vertical * maxSpeed;
        }
        speed = newSpeed;

        if (speed > 0.1f && isGrounded) {
            rollingAudio.mute = false;
            rollingAudio.volume = Mathf.InverseLerp(0f, maxSpeed, speed);
        }
        else {
            rollingAudio.mute = true;
        }

        // if (carryObject != null && !chemicalAudio.isPlaying) {
        //     chemicalAudio.Play();
        // }
        // else if (carryObject == null && chemicalAudio.isPlaying) {
        //     chemicalAudio.Stop();
        // }

        if (Input.GetButtonDown("Activate")) {
            if (carryObject?.GetComponentInParent<ConfigurableJoint>()) {
                Destroy(carryObject.GetComponentInParent<ConfigurableJoint>());
                carryObject = null;
            }
            else if (targetActivatable != null) {
                targetActivatable.Activate();
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 forwVect = transform.forward;
        forwVect.y = 0f;
        rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, 0f) + forwVect.normalized * speed * Time.fixedDeltaTime;
        rigidbody.MoveRotation(qrot);
        /*if (rigidbody.velocity.magnitude <= maxSpeed) {
            rigidbody.velocity += forwVect * speed * Time.fixedDeltaTime;
        }
        rigidbody.velocity = Vector3.RotateTowards(rigidbody.velocity, transform.forward, 1f * Time.fixedDeltaTime, 0f);*/
        
        /*RaycastHit hit;
        
        Vector3 dir = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, dir, out hit, 0.2f, ~(1 << 7))) {
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }*/
        // if (Time.time > updateTime) {
        //     updateTime = Time.time + 1;
        //     Debug.Log("Rb velocity " + rigidbody.velocity);
        //     Debug.Log("Rb magn " + rigidbody.velocity.sqrMagnitude.ToString("F07"));
        // }
    }

    public bool CarryRequest(GameObject go)
    {
        Debug.Log("Requested carry");
        if (carryObject && carryObject.GetComponent<ConfigurableJoint>()) {
            Debug.Log("Already carrying");
            return false;
        }
        Debug.Log("New carry");
        carryObject = go;
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger");
        targetActivatable = other.GetComponentInParent<IActivatable>();
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger");
        targetActivatable = null;
    }

}
