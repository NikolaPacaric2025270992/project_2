using UnityEngine;

public class ParallaxLayers : MonoBehaviour
{

    public Transform cameraTransform;
    public float parallaxFactor = 0.5f;

    private float startPosX, startPosY, length;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cameraTransform.position.x * parallaxFactor;
        float distanceY = cameraTransform.position.y * parallaxFactor;
        float movement = cameraTransform.position.x * (1 - parallaxFactor);

        transform.position = new Vector3(startPosX + distance, startPosY + distanceY, transform.position.z);

        if (movement > startPosX + length)
        {
            startPosX += length;
        } else if(movement < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
