using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class ConstellationRenderer : MonoBehaviour
{
    public float timeToStretch = 1.0f;

    private LineRenderer _lineRenderer;

    private List<GameObject> _startingPositions;
    private List<GameObject> _goalPositions;
    private Material _lineRendererMaterial;
    private static readonly int Lerp = Shader.PropertyToID("_Lerp");


    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _startingPositions = new List<GameObject>();

        _lineRendererMaterial = _lineRenderer.material;

        /* int maxPoint = 10;
         for (int i = 0; i < maxPoint; i++)
         {
             for (int j = 0; j < maxPoint; j++)
             {
                 AddPoint(new Vector3(i *  2, j));
             }
         }*/
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            DrawLine();
        }*/
    }

    public void AddPoint(Vector3 newPoint)
    {
        GameObject objectToAdd = new GameObject();
        objectToAdd.transform.parent = transform;
        objectToAdd.transform.position = newPoint;
        
        _startingPositions.Add(objectToAdd);
        _lineRenderer.positionCount = _startingPositions.Count;
        UpdatePositions();

    }

    public void DrawLine()
    {
        Sequence seq = DOTween.Sequence();
        seq.OnUpdate(UpdatePositions);

        Vector3 lineDir = (_startingPositions[_startingPositions.Count - 1].transform.position - _startingPositions[0].transform.position).normalized;
        
        List<GameObject> goalPositions = new List<GameObject>();
        goalPositions.AddRange(_startingPositions);

        for (int i = 1; i < _startingPositions.Count - 1; ++i)
        {
            Vector3 vecDir = _startingPositions[i].transform.position - _startingPositions[0].transform.position;
            float dotProduct = Vector3.Dot(lineDir, vecDir);
            
            GameObject goal = new GameObject();
            goal.transform.parent = transform;
            goal.transform.position = _startingPositions[0].transform.position + lineDir * dotProduct;
            goalPositions[i] = goal;

            
            seq.Join(_startingPositions[i].transform.DOMove(goalPositions[i].transform.position, timeToStretch))
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => Destroy(goal));
        }

        seq.Join(DOVirtual.Float(0, 1, timeToStretch, value => _lineRendererMaterial.SetFloat(Lerp, value)));
        seq.AppendCallback(Clean);
    }

    public void UpdatePositions()
    {
        Vector3[] pointPositions = new Vector3[_startingPositions.Count]; 
        for (var i = 0; i < _startingPositions.Count; ++i)
        {
            pointPositions[i] = _startingPositions[i].transform.position;
        }

        _lineRenderer.SetPositions(pointPositions);
    }

    public Vector3 GetLastPoint(int i)
    {
        if(_startingPositions.Count < 1 - i)
            return Vector3.zero;
        return _startingPositions[_startingPositions.Count - 1 - i].transform.position;

    }

    public void Clean()
    {
        Vector3 first = _startingPositions[0].transform.position;
        Vector3 last = _startingPositions[_startingPositions.Count - 1].transform.position;
        _startingPositions.Clear();
        AddPoint(first);
        AddPoint(last);
    }
    
}
