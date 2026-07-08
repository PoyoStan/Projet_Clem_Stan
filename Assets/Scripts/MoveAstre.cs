using UnityEngine;

public class MoveAstre : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.right;

    void Start()
    {

    }

    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }
}