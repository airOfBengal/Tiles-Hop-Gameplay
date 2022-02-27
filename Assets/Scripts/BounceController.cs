using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour
{
    public Transform startRefPosition;
    public Transform endRefPosition;

    public float distanceToJump = 5f;
    private float fraction = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    private float startTime;
    private float journeyTime;

    // Start is called before the first frame update
    void Start()
    {
        startPos = startRefPosition.position;
        endPos = endRefPosition.position;
        startTime = Time.time;
        journeyTime = (distanceToJump / 3) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        fraction = (Time.time - startTime) / journeyTime;
        transform.position = Vector3.Slerp(startPos, endPos, fraction);
        //Debug.Log("transform position y: " + transform.position.y);

        if(Mathf.Abs(transform.position.y - startRefPosition.position.y) <= Mathf.Epsilon){
            startTime = Time.time;
            startPos = startRefPosition.position;
            endPos = endRefPosition.position;
            journeyTime = (distanceToJump / 3) * 2;
        }
        else if(Mathf.Abs(transform.position.y - endRefPosition.position.y) <= Mathf.Epsilon){
            startTime = Time.time;
            startPos = endRefPosition.position;
            endPos = startRefPosition.position;
            journeyTime = (distanceToJump / 3);
        }        
    }
}
