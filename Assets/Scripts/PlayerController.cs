using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float initialVolume = 0.4f;
    public float audioAddition = 0.6f;

    private Rigidbody rb;
    private AudioSource audioSource;
    private float nextFire;
    
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            float menorVolume = 0;
            float maiorVolume = 1.0f - initialVolume;
            float menorPan = -0.8f;
            float maiorPan = 0.8f;
            audioSource.volume = initialVolume + ConvertScale(boundary.xMin, boundary.xMax, Mathf.Abs(transform.position.x), menorVolume, maiorVolume);
            //audioSource.volume = initialVolume + (Mathf.Abs(transform.position.x) / boundary.xMax);
            audioSource.panStereo = ConvertScale(boundary.xMin, boundary.xMax, transform.position.x, menorPan, maiorPan);
            //audioSource.panStereo = Mathf.Clamp((transform.position.x / boundary.xMax), -0.8f, 0.8f);
            audioSource.Play();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;
        rb.position = new Vector3
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
            );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
	}

    private float ConvertScale(float initial, float end, float value, float initialTarget, float endTarget)
    {
        return ((value - initial) / (end - initial)) * (endTarget - initialTarget) + initialTarget;
    }
}
