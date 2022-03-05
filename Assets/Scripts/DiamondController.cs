using UnityEngine;

public class DiamondController : MonoBehaviour
{
    public float rotationSpeed = 45f;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isRunning)
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
            if (transform.position.z <= GameManager.instance.tileDestroyPosition.position.z)
            {
                Destroy(gameObject);
            }
            transform.position = transform.position + Vector3.back * Time.deltaTime * GameManager.instance.tileMoveSpeed;
        }
    }
}
