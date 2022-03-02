using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject tilePrefab;
    public float tileSpawnDelay = 1f;
    public float tileMoveSpeed = 10f;
    public Transform tileInitPosition;
    public Transform tileDestroyPosition;
    public float xLeft = -1f;
    public float xRigth = 1f;
    private float elapsedTime = 0f;

    public static Queue<GameObject> tilesQueue = new Queue<GameObject>();
    public GameObject nextTile;
    private bool tilesMoving;

    public Vector3 targetTilesPosition = Vector3.zero;
    public BounceController bounceController;

    private void Awake() {
        if(instance != null){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {        
        // init tiles to start game
        for (int i = 0; i < 6;i++){
            GameObject tile = InitTileRandomly();
            tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, i * 5);
            tilesQueue.Enqueue(tile);
        }
        nextTile = tilesQueue.Dequeue();
        bounceController.SetBall(new Vector3(nextTile.transform.position.x, nextTile.transform.position.y + 1, nextTile.transform.position.z),
                nextTile.transform.position.z / tileMoveSpeed);
        Time.timeScale = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        //if(nextTile != null){
        //    if(Time.timeScale == 1 && Mathf.Abs(nextTile.transform.position.z-targetTilesPosition.z) <= Mathf.Epsilon){
        //        nextTile = tilesQueue.Dequeue();
        //        bounceController.timeToJump = nextTile.transform.position.z / tileMoveSpeed;                
        //    }
        //}

        if(Input.GetMouseButton(0)){
            if(!tilesMoving){
                tilesMoving = true;
                Time.timeScale = 1;
                bounceController.startTime = Time.time;
            }
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= tileSpawnDelay){
                elapsedTime = 0f;
                tilesQueue.Enqueue(InitTileRandomly());
                Debug.Log("queue len: " + GameManager.tilesQueue.Count);
            }
        }
        else{
            if(tilesMoving){
                tilesMoving = false;
                Time.timeScale = 0;
            }
        }
    }

    private GameObject InitTileRandomly(){
        float xPos = Random.Range(xLeft, xRigth);
        GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, tileInitPosition.position.y, tileInitPosition.position.z), Quaternion.identity);
        return tile;
    }
}
