using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Moving : MonoBehaviour
{
    public float speed = 0.4f;private Vector2 _dest = Vector2.zero;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    [SerializeField]
    private Vector2 buffer = new Vector2(0,0);
    [SerializeField]
    private Vector2 orientation = new Vector2(0,0);
    private Vector2 lastKeyPressed = new Vector2(0,0);
    private SpriteRenderer character;
    private float size;

    [Header("TrailRenderer")] 
    public float timeBetweenEachPoint = 0.3f;
    public GameObject lineRenderModel;
    public VisualEffect particleFX;
    public Vector2 generateConstellationRange = new Vector2(4, 6);
    public List<GameObject> constellationPosition;
    
    private float _timerSetPoint;
    private ConstellationRenderer _CurrentConstellationRenderer;
    private List<Vector3> _lastPoints;
    private List<GameObject> _constellationLines;

    void Start()
    {
        character = GetComponent < SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _dest = transform.position;
        size = character.bounds.size.x;

        _constellationLines = new List<GameObject>();
        _lastPoints = new List<Vector3>();
        GameObject newConstellationRenderer = Instantiate(lineRenderModel, Vector3.zero, Quaternion.identity);
        newConstellationRenderer.AddComponent<ConstellationRenderer>();
        _CurrentConstellationRenderer = newConstellationRenderer.GetComponent<ConstellationRenderer>();
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/Music/StartTheMFMusicGame");  
    }

    private void Update()
    {
       _timerSetPoint += Time.deltaTime;
        if (_timerSetPoint > timeBetweenEachPoint)
        {
            _timerSetPoint = .0f;
            _CurrentConstellationRenderer.AddPoint(transform.position);

            
            if (_lastPoints.Count > 0)
            {
                foreach (var lastPoint in _lastPoints)
                {
                    _CurrentConstellationRenderer.AddPoint(lastPoint);
                }
                _lastPoints.Clear();
            }
        }


        
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
        
        if ((buffer != orientation) &&  Valid(buffer*size))
        {
            orientation = buffer;
            if (orientation == Vector2.down || orientation == Vector2.up)
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            else if (orientation == Vector2.left || orientation==Vector2.right)
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
            
        if(Valid(orientation*size))
        {
            particleFX.SetVector3("PlayerVelocity", orientation);
            _dest = (Vector2) transform.position + orientation*size;
             SetRotation(orientation);
        }
        else
        {
           // orientation = Vector2.zero;
        }
        

        bool Valid(Vector2 dir)
        {
            //if (dir == Vector2.zero) return (true);
            // Cast Line from 'next to Pac-Man' to 'Pac-Man'
            Vector2 pos = transform.position;
            RaycastHit2D hit = Physics2D.CircleCast(pos+dir, size*0.55f, -dir,2f*Vector2.Distance(pos,dir));
            Debug.DrawLine(pos+dir,pos,Color.red,1f);
            return (hit.collider == _collider2D);
        }
    }
    
    private void SetRotation(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        if (direction == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0,0,-90);
        }
        if (direction == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0,0,90);
        }
        if (direction == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0,0,180);
        }
    }

    public void GetStar()
    {
        
        if (_constellationLines.Count >= Random.Range(generateConstellationRange.x, generateConstellationRange.y) && constellationPosition.Count > 0)
        {
            
            Transform t = constellationPosition[0].transform;
            constellationPosition.RemoveAt(0);

            Debug.Log("Move");
            
            GameObject group = new GameObject();

            float timeToMove = 1.0f;
            int countPos = 0;
            Vector3 middle = Vector3.zero;
            
            foreach (var constellationLine in _constellationLines)
            {
                LineRenderer ld = constellationLine.GetComponent<LineRenderer>();
                middle += (ld.GetPosition(0) + ld.GetPosition(1)) / 2.0f;
                ++countPos; 
            }

            group.transform.position = middle / countPos;
            
            foreach (var constellationLine in _constellationLines)
            {
                constellationLine.transform.parent = group.transform;
            }

            group.transform.DOMove(t.position, timeToMove);
            group.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), timeToMove);
            
            _constellationLines.Clear();
            
            
            Debug.Break();
        }
        
        
        if (_CurrentConstellationRenderer)
        {
            _CurrentConstellationRenderer.DrawLine();
            _lastPoints.Add(_CurrentConstellationRenderer.GetLastPoint(1));
            _lastPoints.Add(_CurrentConstellationRenderer.GetLastPoint(2));
        }

        GameObject newConstellationRendere = Instantiate(lineRenderModel, Vector3.zero, Quaternion.identity);
        _CurrentConstellationRenderer = newConstellationRendere.GetComponent<ConstellationRenderer>();
        
        _constellationLines.Add(newConstellationRendere);

       
    }
}