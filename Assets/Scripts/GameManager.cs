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
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= tileSpawnDelay){
            elapsedTime = 0f;
            InitTileRandomly();
        }
    }

    IEnumerator InitTileCoroutine()
    {
        yield return new WaitForSeconds(tileSpawnDelay);        
    }

    private void InitTileRandomly(){
        float xPos = Random.Range(xLeft, xRigth);
        GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, tileInitPosition.position.y, tileInitPosition.position.z), Quaternion.identity);
    }
}
