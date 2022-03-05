using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject tilePrefab;
    public GameObject ballPrefab;
    public float tileSpawnDelayMinDefault = 0.05f;
    public float tileSpawnDelayMinInit = 0.5f;
    public float tileSpawnDelayMin = 0.1f;
    public float tileSpawnDelayMax = 1f;
    private float tileSpawnDelay = 0f;
    public float tileMoveSpeed = 10f;
    public float tileMoveSpeedDelta = 5f;
    public float tileMoveSpeedDeltaTime = 30f; // 30 seconds
    public Transform tileInitPosition;
    public Transform tileDestroyPosition;
    public float xLeft = -1f;
    public float xRigth = 1f;
    private float elapsedTime = 0f;

    public static Queue<GameObject> tilesQueue = new Queue<GameObject>();
    public GameObject nextTile;
    private bool tilesMoving;

    public Vector3 targetTilesPosition = Vector3.zero;
    private BounceController bounceController;
    public GameObject ui;

    public Transform ballEndRefPosition;
    public GameObject ball;
    private float startTime;
    public float tileMoveSpeedDefault = 10f;
    public Color[] tileColors;
    private Color[] materialColors;
    private int currentTileColorIndex;

    public volatile bool isRunning;

    public GameObject diamondPrefab;
    public GameObject starPrefab;
    public float diamondSpawnMinDistance = 6f;
    public float diamondSpawnMinDelay = 15f;
    private float diamondSpawnStartTime;

    public Transform diamInitPos;
    public Transform diamEndPos;

    private void Awake() {
        if(instance != null){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        materialColors = new Color[tileColors.Length];
        for(int i = 0; i < materialColors.Length; i++)
        {
            materialColors[i] = tileColors[i];
        }
        Init();
        //SpawnDiamonds(diamInitPos.position, diamEndPos.position);
    }

    public void Init()
    {
        UIManager.instance.sensitivityButtonGO.SetActive(true);
        UIManager.instance.tilesCountText.text = ScoreBoard.tilesCount.ToString();
        diamondSpawnStartTime = Time.time;
        tileSpawnDelayMin = tileSpawnDelayMinInit;

        currentTileColorIndex = Random.Range(0, tileColors.Length);
        startTime = Time.time;
        tileMoveSpeed = tileMoveSpeedDefault;

        // init tiles to start game
        for (int i = 0; i < 6; i++)
        {
            GameObject tile = InitTileRandomly();
            tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, i * 3);
            tilesQueue.Enqueue(tile);
        }
        nextTile = tilesQueue.Dequeue();
        ball = Instantiate(ballPrefab);
        bounceController = ball.GetComponent<BounceController>();
        // 0.7 = ball radius + half of tile height
        bounceController.SetBall(new Vector3(nextTile.transform.position.x, nextTile.transform.position.y + 0.7f, nextTile.transform.position.z),
                ballEndRefPosition.position, (nextTile.transform.position.z - 0.2f) / tileMoveSpeed);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isRunning && !IsPointerOverUIObject())
        {
            isRunning = true;
            UIManager.instance.sensitivityButtonGO.SetActive(false);
        }

        //if(Input.GetMouseButton(0) && !IsPointerOverUIObject()){
        if (isRunning) {
            // check for diamond spawning
            if (tilesQueue.Count > 3 && (Time.time - diamondSpawnStartTime) > diamondSpawnMinDelay)
            {
                GameObject[] tiles = tilesQueue.ToArray();
                Vector3 tile1Pos = tiles[tiles.Length - 2].transform.position;
                Vector3 tile2Pos = tiles[tiles.Length - 1].transform.position;
                if (tile2Pos.z - tile1Pos.z >= diamondSpawnMinDistance && Random.Range(0, 2) == 1)
                {
                    SpawnDiamonds(tile1Pos, tile2Pos);
                }
                diamondSpawnStartTime = Time.time;
            }

            // increase time after every tileMoveSpeedDeltaTime
            if (Time.time - startTime > tileMoveSpeedDeltaTime)
            {
                // decrease tile spawn delay min after every tileMoveSpeedDeltaTime
                tileSpawnDelayMin = Mathf.Max(tileSpawnDelayMinDefault, tileSpawnDelayMin - (tileMoveSpeedDelta / 20));
                //
                tileMoveSpeed += tileMoveSpeedDelta;                
                currentTileColorIndex++;
                currentTileColorIndex = currentTileColorIndex == materialColors.Length ? 0 : currentTileColorIndex;
                startTime = Time.time;
            }
            
            if(!tilesMoving){
                tilesMoving = true;
                Time.timeScale = 1;
                bounceController.startTime = Time.time;
            }
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= tileSpawnDelay){
                elapsedTime = 0f;
                tileSpawnDelay = Random.Range(tileSpawnDelayMin, tileSpawnDelayMax);
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApp();
        }            
    }

    private GameObject InitTileRandomly(){
        float xPos = Random.Range(xLeft, xRigth);
        GameObject tile = Instantiate(tilePrefab, new Vector3(xPos, tileInitPosition.position.y, tileInitPosition.position.z), Quaternion.identity);
        tile.GetComponent<Renderer>().material.color = materialColors[currentTileColorIndex];
        //TileController tileController = tile.GetComponent<TileController>();
        //tileController.SetSpeed(tileMoveSpeed);
        return tile;
    }

    private bool IsPointerOverUIObject()
    {
        // get current pointer position and raycast it
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // check if the target is in the UI
        foreach (RaycastResult r in results)
        {
            bool isUIClick = r.gameObject.transform.IsChildOf(this.ui.transform) ||
                r.gameObject.transform.IsChildOf(UIManager.instance.gameOverPanelGO.transform);
            if (isUIClick)
            {
                return true;
            }
        }
        return false;
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void OnGameOver()
    {
        if(ball != null)
        {
            Destroy(ball);
        }
        tilesQueue.Clear();
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject tile in tiles)
        {
            Destroy(tile);
        }

        // clear diamonds if any
        GameObject[] diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        foreach(GameObject diamond in diamonds)
        {
            Destroy(diamond);
        }
    }

    private void SpawnDiamonds(Vector3 startPos, Vector3 endPos)
    {
        Vector3[] spawnPositions = GetDiamondInitPositions(startPos, endPos, Random.Range(3, 7));
        foreach(Vector3 pos in spawnPositions)
        {
            Instantiate(diamondPrefab, pos, Quaternion.identity);
        }
    }

    private Vector3[] GetDiamondInitPositions(Vector3 startPos, Vector3 endPos, int noOfDiamond)
    {
        Vector3[] positions = new Vector3[noOfDiamond];
        float distance = Mathf.Abs(startPos.z - endPos.z);
        float intervalZ = distance / (noOfDiamond+1);
        float intervalY = ballEndRefPosition.position.y / (noOfDiamond + 1);
        float intervalX = (endPos.x - startPos.x) / (noOfDiamond + 1);

        float i = intervalZ;
        int j = 1;
        // go up
        while(i <= distance/ 2)
        {
            positions[j - 1] = new Vector3(startPos.x + intervalX * j, startPos.y + intervalY * j, startPos.z + intervalZ * j);
            i += intervalZ;
            j++;
        }
        // go down
        int k = 1;
        while(i < distance)
        {
            if(j -1 < positions.Length)
                positions[j - 1] = new Vector3(startPos.x + intervalX * j, ballEndRefPosition.position.y - intervalY * k, startPos.z + intervalZ * j);
            i += intervalZ;
            j++;
            k++;
        }
        return positions;
    }
}
