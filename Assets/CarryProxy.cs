using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryProxy : MonoBehaviour
{

    bool canAttach = true;
    public GameObject referenceJoint;
    ConfigurableJoint myJoint = null;
    float maxForce = 0f;
    float maxTorque = 0f;

    public AudioSource chemicalAudio;
    public AudioSource droppingSound;
    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        chemicalAudio = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chemicalAudio && rigidbody) {
            if (!rigidbody.isKinematic && !chemicalAudio.isPlaying) {
                chemicalAudio.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RobotController2 rc = other.GetComponentInParent<RobotController2>();
        if (rc && canAttach) {
            if (rc.CarryRequest(gameObject)) {
                canAttach = false;
                GameObject newSpring = Instantiate(referenceJoint, other.transform.position, Quaternion.identity);
                Destroy(GetComponent<Rigidbody>());
                myJoint = newSpring.GetComponent<ConfigurableJoint>();
                myJoint.connectedBody = rc.GetComponent<Rigidbody>();
                transform.SetParent(newSpring.transform);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Dropped");
        if (droppingSound) {
            droppingSound.Play();
        }
    }

}
