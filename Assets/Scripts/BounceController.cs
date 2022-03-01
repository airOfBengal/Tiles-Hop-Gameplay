using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour
{
    public Vector3 startRefPosition;
    public Transform endRefPosition;

    public float distanceToJump = 5f;
    private float fraction = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    public float startTime;
    private float journeyTime;

    // Start is called before the first frame update
    void Start()
    {
        // Vector3 startTilePos = GameManager.instance.nextTile.transform.position;
        // startRefPosition = new Vector3(startTilePos.x, startTilePos.y + 1f, startTilePos.z);
        // startPos = startRefPosition;
        endPos = endRefPosition.position;
        //startTime = Time.time;
        // distanceToJump = GameManager.instance.nextTile.transform.position.z;
        // journeyTime = (distanceToJump / 3) * 2;
        // transform.position = startPos;
    }

    public void SetBall(Vector3 startRPos, float distToJump){
        startRefPosition = startRPos;
        startPos = startRPos;
        distanceToJump = distToJump;
        journeyTime = (distanceToJump / 3) * 2;
        startTime = Time.time;
        transform.position = startRPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1){
            fraction = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Slerp(startPos, endPos, fraction);
            //Debug.Log("transform position y: " + transform.position.y);

            if(Mathf.Abs(transform.position.y - startRefPosition.y) <= Mathf.Epsilon){
                startTime = Time.time;
                startPos = startRefPosition;
                endPos = endRefPosition.position;
                journeyTime = (distanceToJump / 3) * 2;
            }
            else if(Mathf.Abs(transform.position.y - endRefPosition.position.y) <= Mathf.Epsilon){
                startTime = Time.time;
                startPos = endRefPosition.position;
                endPos = startRefPosition;
                journeyTime = (distanceToJump / 3);
            }     
        }   
    }

    private void OnCollisionEnter(Collision other) {
        if(Time.timeScale == 1 && other.gameObject.CompareTag("Tile")){
            startRefPosition = new Vector3(transform.position.x, startRefPosition.y + 1, startRefPosition.z);
        }
    }
}
