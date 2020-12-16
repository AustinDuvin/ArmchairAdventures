using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.current;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookAtVector = camera.transform.position - transform.position;

        lookAtVector.x = 0.0f;

        lookAtVector.z = 0.0f;

        transform.LookAt(camera.transform.position - lookAtVector);

        transform.Rotate(0, 180, 0);
    }
}
