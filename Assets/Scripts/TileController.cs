using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if(transform.position.z <= GameManager.instance.tileDestroyPosition.position.z){            
            Destroy(gameObject);
        }
        transform.position = transform.position + Vector3.back * Time.deltaTime * GameManager.instance.tileMoveSpeed;
    }
}
