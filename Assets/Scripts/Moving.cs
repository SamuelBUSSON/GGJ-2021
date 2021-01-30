using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Moving : MonoBehaviour
{
    public float speed = 0.4f;
    Vector2 _dest = Vector2.zero;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    [SerializeField]
    private Vector2 buffer = new Vector2(0,0);
    [SerializeField]
    private Vector2 orientation = new Vector2(0,0);
    private Vector2 lastKeyPressed = new Vector2(0,0);

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
        /*if ((Vector2) transform.position == _dest)
        {*/
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (lastKeyPressed != Vector2.up)
            {
                buffer = Vector2.up;
                lastKeyPressed = Vector2.up;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (lastKeyPressed != Vector2.right)
            {
                buffer = Vector2.right;
                lastKeyPressed = Vector2.right;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (lastKeyPressed != Vector2.down)
            {
                buffer = Vector2.down;
                lastKeyPressed =Vector2.down;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (lastKeyPressed != Vector2.left)
            {
                buffer = Vector2.left;
                lastKeyPressed = Vector2.left;
            }
        }
        
        if ((buffer != orientation) &&  Valid(buffer))
        {
            orientation = buffer;
        }
            
        if( Valid(orientation))
        {
            _dest = (Vector2) transform.position + orientation;
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