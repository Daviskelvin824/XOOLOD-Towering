using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MazeNodes : MonoBehaviour
{
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftLight;
    public GameObject rightLight;
    public GameObject topLight;
    public GameObject bottomLight;
    public int cost;
    public float heuristicVal;
    public MazeNodes parent;
    public Vector2Int position;
    public bool visited = false;
    public NavMeshSurface navmesh;

}
