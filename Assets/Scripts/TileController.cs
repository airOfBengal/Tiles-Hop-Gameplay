using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public float boosterJumpDuration = 1f;
    public float boosterJumpSpeed = 10f;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isRunning)
        {
            if (Time.time - startTime <= boosterJumpDuration && transform.position.z > 15f)
            {
                transform.position = transform.position + Vector3.back * Time.deltaTime * (GameManager.instance.tileMoveSpeed + boosterJumpSpeed);
                return;
            }

            if (transform.position.z <= GameManager.instance.tileDestroyPosition.position.z)
            {
                Destroy(gameObject);
            }
            transform.position = transform.position + Vector3.back * Time.deltaTime * GameManager.instance.tileMoveSpeed;
        }
    }
}
