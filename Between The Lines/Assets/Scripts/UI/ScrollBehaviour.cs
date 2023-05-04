using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBehaviour : MonoBehaviour
{
    [SerializeField] private float scrollDistance;
    [SerializeField] private float boundarySize;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private AnimationCurve speedCurve;

    private float startY;

    void OnEnable()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (transform.position.y > startY) // Can scroll up
        {
            if (Input.mousePosition.y >= Screen.height - boundarySize)
            {
                float speed = speedCurve.Evaluate(boundarySize / Mathf.Max(Input.mousePosition.y - (Screen.height - boundarySize), 1));
                Vector3 newPosition = transform.position;
                newPosition.y = Mathf.Max(startY, newPosition.y - speed*Time.deltaTime);
                transform.position = newPosition;
            }
        }

        if (transform.position.y < startY + scrollDistance) // Can scroll down
        {
            if (Input.mousePosition.y <= boundarySize)
            {
                float speed = speedCurve.Evaluate(boundarySize / Mathf.Max(boundarySize - Input.mousePosition.y, 1));
                Vector3 newPosition = transform.position;
                newPosition.y = Mathf.Min(startY + scrollDistance, newPosition.y + speed*Time.deltaTime);
                transform.position = newPosition;
            }
        }
    }
}