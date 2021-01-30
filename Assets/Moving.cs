using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public float speed = 0.4f;
    Vector2 _dest = Vector2.zero;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _dest = transform.position;
    }

    void FixedUpdate()
    {
        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, _dest, speed);
        _rigidbody2D.MovePosition(p);

        // Check for Input if not moving
        if ((Vector2) transform.position == _dest)
        {
            if (Input.GetKey(KeyCode.UpArrow) && Valid(Vector2.up))
                _dest = (Vector2) transform.position + Vector2.up;
            if (Input.GetKey(KeyCode.RightArrow) && Valid(Vector2.right))
                _dest = (Vector2) transform.position + Vector2.right;
            if (Input.GetKey(KeyCode.DownArrow) && Valid(-Vector2.up))
                _dest = (Vector2) transform.position - Vector2.up;
            if (Input.GetKey(KeyCode.LeftArrow) && Valid(-Vector2.right))
                _dest = (Vector2) transform.position - Vector2.right;
        }

        bool Valid(Vector2 dir)
        {
            // Cast Line from 'next to Pac-Man' to 'Pac-Man'
            Vector2 pos = transform.position;
            RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
            return (hit.collider == _collider2D);
        }
    }
}