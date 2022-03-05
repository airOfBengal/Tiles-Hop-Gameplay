using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 offset;
    public float moveSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - GameManager.instance.ball.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(GameManager.instance.ball.transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z + offset.z),
            moveSpeed * Time.deltaTime);
    }
}
