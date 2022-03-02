using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour
{
    public Vector3 startRefPosition;
    public Transform endRefPosition;

    public float timeToJump = 5f;
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
        //endPos = endRefPosition.position;
        //startTime = Time.time;
        // timeToJump = GameManager.instance.nextTile.transform.position.z;
        // journeyTime = (timeToJump / 3) * 2;
        // transform.position = startPos;
        journeyTime = 1;
    }

    public void SetBall(Vector3 startRPos, float distToJump){
        startRefPosition = startRPos;
        startPos = startRPos;
        endPos = endRefPosition.position;
        timeToJump = distToJump;
        journeyTime = (timeToJump / 3) * 2;
        //journeyTime = timeToJump / 2;
        startTime = Time.time;
        transform.position = startRPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1){
            fraction = ((Time.time - startTime)) / journeyTime;
            transform.position = Vector3.Lerp(startPos, endPos, fraction);
            //Debug.Log("transform position y: " + transform.position.y);

            if (Mathf.Abs(transform.position.y - startRefPosition.y) <= Mathf.Epsilon){
                GameObject nextTile = GameManager.tilesQueue.Dequeue();
                timeToJump = nextTile.transform.position.z / GameManager.instance.tileMoveSpeed;

                startTime = Time.time;
                startPos = startRefPosition;
                endPos = endRefPosition.position;
                journeyTime = (timeToJump / 3) * 2;
                //journeyTime = timeToJump / 2;
            }
            else if(Mathf.Abs(transform.position.y - endRefPosition.position.y) <= Mathf.Epsilon){
                startTime = Time.time;
                startPos = endRefPosition.position;
                endPos = startRefPosition;
                journeyTime = (timeToJump / 3);
                //journeyTime = timeToJump / 2;
            }
            
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(Time.timeScale == 1 && other.gameObject.CompareTag("Tile")){
            startRefPosition = new Vector3(transform.position.x, startRefPosition.y + 1, startRefPosition.z);
        }
    }
}
