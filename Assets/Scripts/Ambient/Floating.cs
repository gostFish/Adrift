using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float floatStrength = 3f;
    public float rotationLevel = 3f;
    public int buoyCount;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            rigidBody = gameObject.GetComponent<Rigidbody>();
        }
        else
        {
            rigidBody = gameObject.transform.parent.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / buoyCount, transform.position, ForceMode.Acceleration);
        float x = Waves.instance.GetWaveHeight(transform.position.x);
        float z = Waves.instance.GetWaveHeight(transform.position.z);

        float waveHeight = x + z;
        if(transform.position.y < waveHeight)
        {            
            float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y) / depthBeforeSubmerged) * floatStrength;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(rotationLevel * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
