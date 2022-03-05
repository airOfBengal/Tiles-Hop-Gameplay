using UnityEngine;

public class BounceController : MonoBehaviour
{
    public Vector3 startRefPosition;
    public Vector3 endRefPosition;

    public float timeToJump = 5f;
    private float fraction = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    public float startTime;
    private float journeyTime;

    private Camera mainCamera;
    private Vector3 prevTouchPos;
    private float ballPositionX = 0f;
    public float ballDeltaPositionX = 0.1f;
    public float ballMoveSensitivityReduceValue = 0.1f;

    private float radius;
    private bool isHittingHighway;

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
        mainCamera = Camera.main;
        prevTouchPos = new Vector3(0.5f, 0f, 0f);
        radius = GetComponent<SphereCollider>().radius;
    }

    public void SetBall(Vector3 startRPos, Vector3 endRPos, float distToJump){
        startRefPosition = startRPos;
        startPos = startRPos;
        endRefPosition = endRPos;
        endPos = endRPos;
        timeToJump = distToJump;
        journeyTime = (timeToJump / 3) * 2;
        //journeyTime = timeToJump / 2;
        startTime = Time.time;
        transform.position = startRPos;
        ballPositionX = transform.position.x;
        isHittingHighway = false;
    }

    private void Update()
    {
        //if (Time.timeScale == 1){
        if (GameManager.instance.isRunning)
        {
            if (Input.GetMouseButton(0))
            {
                // control ball drag left-right         
                Vector3 currentTouchPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
                //Debug.Log("world x:" + currentTouchPos.x + " world y:" + currentTouchPos.y + "world z:" + currentTouchPos.z);
                if (currentTouchPos.x < prevTouchPos.x)
                {
                    endRefPosition = new Vector3(endRefPosition.x - ballDeltaPositionX, endRefPosition.y, endRefPosition.z);
                    prevTouchPos = currentTouchPos;
                    ballPositionX -= ballDeltaPositionX;
                }
                else if (currentTouchPos.x > prevTouchPos.x)
                {
                    endRefPosition = new Vector3(endRefPosition.x + ballDeltaPositionX, endRefPosition.y, endRefPosition.z);
                    prevTouchPos = currentTouchPos;
                    ballPositionX += ballDeltaPositionX;
                }
            }
            //if (Input.touchCount > 0)
            //{
            //    Touch touch = Input.GetTouch(0);

            //    int direction = (touch.position.x > (Screen.width / 2)) ? 1 : -1;
            //    if(direction > 0)
            //    {
            //        endRefPosition = new Vector3(endRefPosition.x + ballDeltaPositionX, endRefPosition.y, endRefPosition.z);
            //    }
            //    else
            //    {
            //        endRefPosition = new Vector3(endRefPosition.x - ballDeltaPositionX, endRefPosition.y, endRefPosition.z);
            //    }
            //    MovePaddle(direction);
            //}

            fraction = ((Time.time - startTime)) / journeyTime;
            transform.position = Vector3.Lerp(startPos, endPos, fraction);

            //Debug.Log("transform position y: " + transform.position.y);

            if (Mathf.Abs(transform.position.y - startRefPosition.y) <= Mathf.Epsilon)
            {
                if (isHittingHighway)
                {
                    GameManager.instance.isRunning = false;
                    UIManager.instance.gameOverPanelGO.SetActive(true);
                    GameManager.instance.OnGameOver();
                    //ScoreBoard.SaveTilesCount();
                    ScoreBoard.tilesCount = 0;
                    ScoreBoard.SaveDiamondCount();
                    return;
                }
                else
                {
                    ScoreBoard.tilesCount++;
                    UIManager.instance.tilesCountText.text = ScoreBoard.tilesCount.ToString();
                }

                GameObject nextTile = GameManager.tilesQueue.Dequeue();
                timeToJump = (nextTile.transform.position.z - 0.05f) / GameManager.instance.tileMoveSpeed;

                startTime = Time.time;
                startPos = startRefPosition;
                endPos = endRefPosition;
                journeyTime = (timeToJump / 3) * 2;
                //journeyTime = timeToJump / 2;
            }
            else if (Mathf.Abs(transform.position.y - endRefPosition.y) <= Mathf.Epsilon)
            {
                startTime = Time.time;
                startPos = endRefPosition;
                endPos = startRefPosition;
                journeyTime = (timeToJump / 3);
                //journeyTime = timeToJump / 2;
            }

        }
    }

    void MovePaddle(int direction)
    {
        float xPos = transform.position.x + (direction * Time.deltaTime * 5f);
        //playerPos = new Vector3(Mathf.Clamp(xPos, -2f, 2f), transform.position.y, transform.position.z);
        transform.position = new Vector3(Mathf.Clamp(xPos, -2f, 2f), transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isRunning)
        {

            CheckIfHit();

        }
    }

    private void CheckIfHit()
    {
        // Does the ray intersect any objects
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - radius, transform.position.z);
        RaycastHit[] hits = Physics.RaycastAll(origin, Vector3.down, 1f);
        
        // check for perfect hit
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Tile"))
            {
                isHittingHighway = false;
                return;
            }
        }

        // check for tiles hit
        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].transform.CompareTag("Tile"))
            {
                isHittingHighway = false;
                return;
            }
        }

        // check for highway hit
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Highway"))
            {
                isHittingHighway = true;
                return;
            }
        }
    }

    private void LateUpdate()
    {
        if (GameManager.instance.isRunning)
        {
            transform.position = new Vector3(Mathf.Clamp(ballPositionX, -2, 2), transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ball collide");
        if (collision.transform.CompareTag("Diamond"))
        {
            Destroy(collision.gameObject);
            ScoreBoard.diamondCount++;
            UIManager.instance.diamondCountText.text = ScoreBoard.diamondCount.ToString();
            Debug.Log("dimonds: " + ScoreBoard.diamondCount);
        }
    }
}
