using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class MazeGenerator : MonoBehaviour
{
    private MazeNodes[,] firstFloor;
    private MazeNodes[,] secondFloor;
    public int mazeX;
    public int mazeZ;
    public GameObject mazeNodePrefab;
    public GameObject spawnRoomPrefab;
    public GameObject room1;
    public GameObject room2;
    public GameObject room3;
    public GameObject teleporter;
    public GameObject endRoomPrefab;
    public GameObject endRoomPortal;
    private Vector2Int spawnRoomSize = new(2, 2);
    private Vector2Int room1Size = new(3, 3);
    private Vector2Int room2Size = new(5, 5);
    private Vector2Int room3Size = new(5, 3);
    private Vector2Int teleporterSize = new(1, 3);
    private Vector2Int[] teleporterPosList;
    private List<Vector2Int> doorPath = new();
    public List<Vector2Int>doorPathFloor2 = new();
    private List<Vertex> connectedRooms = new List<Vertex>();
    private List<Vertex> connectedRooms2 = new List<Vertex>();
    Vector2 offsetPlayer = new Vector2();
    Vector2Int spawnRoomPos;
    private Vector2Int[] direction = 
    {
        new (0,1),
        new(1,0),
        new(0,-1),
        new(-1,0)
    };
    public GameObject player;

    void Start()
    {
        GenerateGrid();
        GenerateRooms();
        GenerateRooms2();
        connectedRooms = ConnectRooms(doorPath);
        connectedRooms2 = ConnectRooms(doorPathFloor2);
        for (int i = 0; i < connectedRooms.Count; i++)
        {
            if (connectedRooms[i].point1 == null || connectedRooms[i].point2 == null)
            {
                continue;
            }

            // Check if point1 or point2 coordinates are out of bounds
            if (connectedRooms[i].point1.x < 0 || connectedRooms[i].point1.x >= mazeX || connectedRooms[i].point1.y < 0 || connectedRooms[i].point1.y >= mazeZ ||
                connectedRooms[i].point2.x < 0 || connectedRooms[i].point2.x >= mazeX || connectedRooms[i].point2.y < 0 || connectedRooms[i].point2.y >= mazeZ)
            {
                continue;
            }

            // If everything is valid, find the path
            FindPath(connectedRooms[i].point1, connectedRooms[i].point2);
        }

        
        for (int i = 0; i < connectedRooms2.Count; i++)
        {
            // Check if point1 or point2 is null
            if (connectedRooms2[i].point1 == null || connectedRooms2[i].point2 == null)
            {
                continue;
            }

            // Check if point1 or point2 coordinates are out of bounds
            if (connectedRooms2[i].point1.x < 0 || connectedRooms2[i].point1.x >= mazeX || connectedRooms2[i].point1.y < 0 || connectedRooms2[i].point1.y >= mazeZ ||
                connectedRooms2[i].point2.x < 0 || connectedRooms2[i].point2.x >= mazeX || connectedRooms2[i].point2.y < 0 || connectedRooms2[i].point2.y >= mazeZ)
            {
                continue;
            }

            // If everything is valid, find the path
            FindPath2(connectedRooms2[i].point1, connectedRooms2[i].point2);
        }


        for (int i = 0; i < mazeX; i++)
        {
            for (int j= 0; j < mazeZ; j++)
            {
                if (firstFloor[i, j] != null && firstFloor[i,j].visited==false&&!doorPath.Contains(new(i,j)))
                {

                    Destroy(firstFloor[i,j].gameObject);
                }
            }
        }
        for (int i = 0; i < mazeX; i++)
        {
            for (int j = 0; j < mazeZ; j++)
            {
                if (secondFloor[i, j] != null && secondFloor[i, j].visited == false && !doorPathFloor2.Contains(new(i, j)))
                {

                    Destroy(secondFloor[i, j].gameObject);
                }
            }
        }
        Vector3 playerStartPosition = new Vector3(
            transform.position.x + spawnRoomPos.x * 2.5f + 1.25f,
            transform.position.y + 1,
            transform.position.z + spawnRoomPos.y * 2.5f + 1.25f
        );

        // Drop the player into the scene at the portal's position
        DropPlayerAtPosition(playerStartPosition);
        NavMeshSurface surface = firstFloor[doorPath[0].x, doorPath[0].y].navmesh;
        surface.BuildNavMesh();
        
        var minotaurAgents = FindObjectsOfType<NavMeshAgent>();
        foreach(var agent in minotaurAgents) {
            agent.Warp(agent.transform.position + new Vector3(0, -0.2f, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void DropPlayerAtPosition(Vector3 position)
    {
        // Assuming your player GameObject is tagged as "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Assign the player's position
            player.transform.position = position;
        }
        else
        {
            Debug.LogError("Player GameObject not found. Make sure it is tagged as 'Player'.");
        }
    }
    private void GenerateGrid()
    {
        firstFloor = new MazeNodes[mazeX, mazeZ];
        for (int x = 0; x < mazeX; x++)
        {
            for (int z = 0; z < mazeZ; z++)
            {
                var obj = Instantiate(mazeNodePrefab, transform);
                obj.transform.position = new Vector3(transform.position.x + x * (float)2.5, transform.position.y, transform.position.z + z * (float)2.5);
                firstFloor[x, z] = obj.GetComponent<MazeNodes>();

            }
        }

        secondFloor = new MazeNodes[mazeX, mazeZ];
        for (int x = 0; x < mazeX; x++)
        {
            for (int z = 0; z < mazeZ; z++)
            {
                var obj = Instantiate(mazeNodePrefab, transform);
                obj.transform.position = new Vector3(transform.position.x + x * (float)2.5, transform.position.y + 5, transform.position.z + z * (float)2.5);
                secondFloor[x, z] = obj.GetComponent<MazeNodes>();
            }
        }
    }

    private void GenerateRooms()
    {
        teleporterPosList = new Vector2Int[2];
        int tempX, tempZ;
        MazeNodes node;
        //spawn room
        bool isValid;
        do
        {
            spawnRoomPos = GenerateRandomCoord();
            tempX = spawnRoomPos.x + 1;
            tempZ = spawnRoomPos.y - 1;
            isValid = CheckRandomCoord(spawnRoomPos, spawnRoomSize,tempX,tempZ);
        } while (!isValid);

        var obj = Instantiate(spawnRoomPrefab, transform);
        obj.transform.position = new Vector3(transform.position.x + spawnRoomPos.x * (float)2.5, transform.position.y, transform.position.z + spawnRoomPos.y * (float)2.5);
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);
        offsetPlayer = obj.GetComponent<NodeData>().offset;
        doorPath.Add(new(tempX,tempZ));

        for(int x = spawnRoomPos.x; x < spawnRoomSize.x + spawnRoomPos.x; x++)
        {
            for(int z = spawnRoomPos.y; z < spawnRoomSize.y + spawnRoomPos.y; z++)
            {
                Destroy(firstFloor[x, z].gameObject);
                firstFloor[x, z] = null;
            }
        }
        node = firstFloor[tempX, tempZ];
        ClearDoorWall(tempX, tempZ, node);


        //room 1
        Vector2Int room1Pos;
        for(int i=0;i<2;i++)
        {

            do
            {
                room1Pos = GenerateRandomCoord();
                tempX = room1Pos.x + 1;
                tempZ = room1Pos.y + 3;
                isValid = CheckRandomCoord(room1Pos, room1Size,tempX,tempZ);
            } while (!isValid);
            var obj2 = Instantiate(room1, transform);
            obj2.transform.position = new Vector3(transform.position.x + room1Pos.x * (float)2.5, transform.position.y, transform.position.z + room1Pos.y * (float)2.5);
            Vector2 offset = obj2.GetComponent<NodeData>().offset;
            obj2.transform.position += new Vector3(offset.x,0f, offset.y);
            doorPath.Add(new(tempX, tempZ));
            
            for (int x = room1Pos.x; x < room1Size.x + room1Pos.x; x++)
            {
                for (int z = room1Pos.y; z < room1Size.y + room1Pos.y; z++)
                {
                    Destroy(firstFloor[x, z].gameObject);
                    firstFloor[x, z] = null;
                }
            }
            node = firstFloor[tempX, tempZ];
            ClearDoorWall(tempX, tempZ, node);
        }

        //room2
        Vector2Int room2Pos;
        for(int i = 0; i < 3; i++)
        {
            do
            {
                room2Pos = GenerateRandomCoord();
                tempX = room2Pos.x - 1;
                tempZ = room2Pos.y + 2;
                isValid = CheckRandomCoord(room2Pos, room2Size,tempX, tempZ);
            } while (!isValid);
            var obj3 = Instantiate(room2, transform);
            obj3.transform.position = new Vector3(transform.position.x + room2Pos.x * (float)2.5, transform.position.y, transform.position.z + room2Pos.y * (float)2.5);
            Vector2 offset2 = obj3.GetComponent<NodeData>().offset;
            obj3.transform.position += new Vector3(offset2.x, 0f, offset2.y);
            doorPath.Add(new(tempX, tempZ));
           
            for (int x = room2Pos.x; x < room2Size.x + room2Pos.x; x++)
            {
                for (int z = room2Pos.y; z < room2Size.y + room2Pos.y; z++)
                {
                    Destroy(firstFloor[x, z].gameObject);
                    firstFloor[x, z] = null;
                }
            }
            node = firstFloor[tempX, tempZ];
            ClearDoorWall(tempX, tempZ, node);
        }

        //room3
        Vector2Int room3Pos;
        for(int i = 0; i < 2; i++)
        {
            do
            {
                room3Pos = GenerateRandomCoord();
                tempX = room3Pos.x + 1;
                tempZ = room3Pos.y + 3;
                isValid = CheckRandomCoord(room3Pos, room2Size, tempX, tempZ);
            } while (!isValid);
            teleporterPosList[i] = room3Pos;
            var obj4 = Instantiate(room3, transform);
            obj4.transform.position = new Vector3(transform.position.x + room3Pos.x * (float)2.5, transform.position.y, transform.position.z + room3Pos.y * (float)2.5);
            Vector2 offset3 = obj4.GetComponent<NodeData>().offset;
            obj4.transform.position += new Vector3(offset3.x, 0f, offset3.y);

            doorPath.Add(new(tempX, tempZ));

            for (int x = room3Pos.x; x < room3Size.x + room3Pos.x; x++)
            {
                for (int z = room3Pos.y; z < room3Size.y + room3Pos.y; z++)
                {
                    Destroy(firstFloor[x, z].gameObject);
                    firstFloor[x, z] = null;
                }
            }
            node = firstFloor[tempX, tempZ];
            ClearDoorWall(tempX, tempZ, node);
        }

        //teleporter
        Vector2Int teleporterPos;
        for (int i = 0; i < 2; i++)
        {

            do
            {
                teleporterPos = GenerateRandomCoord();
                tempX = teleporterPos.x + 1;
                tempZ = teleporterPos.y + 1;
                isValid = CheckRandomCoord(teleporterPos, room2Size, tempX, tempZ);
            } while (!isValid);
            teleporterPosList[i] = teleporterPos;
            var obj5 = Instantiate(teleporter, transform);
            obj5.transform.position = new Vector3(transform.position.x + teleporterPos.x * (float)2.5, transform.position.y, transform.position.z + teleporterPos.y * (float)2.5);
            Vector2 offset4 = obj5.GetComponent<NodeData>().offset;
            obj5.transform.position += new Vector3(offset4.x, 0f, offset4.y);
            doorPath.Add(new(tempX, tempZ));

            for (int x = teleporterPos.x; x < teleporterSize.x + teleporterPos.x; x++)
            {
                for (int z = teleporterPos.y; z < teleporterSize.y + teleporterPos.y; z++)
                {
                    Destroy(firstFloor[x, z].gameObject);
                    firstFloor[x, z] = null;
                }
            }
            node = firstFloor[tempX, tempZ];
            ClearDoorWall(tempX, tempZ, node);
        }
    }

    private void GenerateRooms2()
    {
        bool isValid;
        int tempX, tempZ;
        MazeNodes node;
        //teleporter
        for (int i = 0; i < 2; i++)
        {
            var obj5 = Instantiate(teleporter, transform);
            obj5.transform.position = new Vector3(transform.position.x + teleporterPosList[i].x * (float)2.5, transform.position.y + 5, transform.position.z + teleporterPosList[i].y * (float)2.5);
            Vector2 offset4 = obj5.GetComponent<NodeData>().offset;
            obj5.transform.position += new Vector3(offset4.x, 0f, offset4.y);
            tempX = teleporterPosList[i].x + 1;
            tempZ = teleporterPosList[i].y + 1;
            doorPathFloor2.Add(new(tempX, tempZ));
            for (int x = teleporterPosList[i].x; x < teleporterSize.x + teleporterPosList[i].x; x++)
            {
                for (int z = teleporterPosList[i].y; z < teleporterSize.y + teleporterPosList[i].y; z++)
                {
                    Destroy(secondFloor[x, z].gameObject);
                    secondFloor[x, z] = null;
                }
            }
            node = secondFloor[tempX, tempZ];
            ClearDoorWallSecondFloor(tempX, tempZ, node);
        }
        
        

        //end room
        Vector2Int endRoomPos;
        do
        {
            endRoomPos = GenerateRandomCoord();
            tempX = endRoomPos.x + 1;
            tempZ = endRoomPos.y - 1;
            isValid = CheckRandomCoord2(endRoomPos, spawnRoomSize,tempX,tempZ);
        } while (!isValid);

        var obj = Instantiate(endRoomPrefab, transform);
        obj.transform.position = new Vector3(transform.position.x + endRoomPos.x * (float)2.5, transform.position.y + 5, transform.position.z + endRoomPos.y * (float)2.5);
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);
        doorPathFloor2.Add(new(tempX, tempZ));

        Vector3 endPortalPosition = new Vector3(transform.position.x + endRoomPos.x * (float)2.5, (float)(transform.position.y +5.07), transform.position.z + endRoomPos.y * (float)2.5);
        endRoomPortal.transform.position = endPortalPosition;

        for (int x = endRoomPos.x; x < spawnRoomSize.x + endRoomPos.x; x++)
        {
            for (int z = endRoomPos.y; z < spawnRoomSize.y + endRoomPos.y; z++)
            {
                Destroy(secondFloor[x, z].gameObject);
                secondFloor[x, z] = null;
            }
        }
        node = secondFloor[tempX, tempZ];
        ClearDoorWallSecondFloor(tempX, tempZ, node);

        //room 1
        Vector2Int room1Pos;
        for (int i = 0; i < 3; i++)
        {

            do
            {
                room1Pos = GenerateRandomCoord();
                tempX = room1Pos.x + 1;
                tempZ = room1Pos.y + 3;
                isValid = CheckRandomCoord2(room1Pos, room1Size,tempX,tempZ);
            } while (!isValid);
            var obj2 = Instantiate(room1, transform);
            obj2.transform.position = new Vector3(transform.position.x + room1Pos.x * (float)2.5, transform.position.y+5, transform.position.z + room1Pos.y * (float)2.5);
            Vector2 offset = obj2.GetComponent<NodeData>().offset;
            obj2.transform.position += new Vector3(offset.x, 0f, offset.y);
            doorPathFloor2.Add(new(tempX, tempZ));
            for (int x = room1Pos.x; x < room1Size.x + room1Pos.x; x++)
            {
                for (int z = room1Pos.y; z < room1Size.y + room1Pos.y; z++)
                {
                    Destroy(secondFloor[x, z].gameObject);
                    secondFloor[x, z] = null;
                }
            }
            node = secondFloor[tempX, tempZ];
            ClearDoorWallSecondFloor(tempX, tempZ, node);
        }

        //room2
        Vector2Int room2Pos;
        for (int i = 0; i < 4; i++)
        {
            do
            {
                room2Pos = GenerateRandomCoord();
                tempX = room2Pos.x - 1;
                tempZ = room2Pos.y + 2;
                isValid = CheckRandomCoord2(room2Pos, room2Size, tempX, tempZ);
            } while (!isValid);
            var obj3 = Instantiate(room2, transform);
            obj3.transform.position = new Vector3(transform.position.x + room2Pos.x * (float)2.5, transform.position.y+5, transform.position.z + room2Pos.y * (float)2.5);
            Vector2 offset2 = obj3.GetComponent<NodeData>().offset;
            obj3.transform.position += new Vector3(offset2.x, 0f, offset2.y);
            doorPathFloor2.Add(new(tempX, tempZ));
            for (int x = room2Pos.x; x < room2Size.x + room2Pos.x; x++)
            {
                for (int z = room2Pos.y; z < room2Size.y + room2Pos.y; z++)
                {
                    Destroy(secondFloor[x, z].gameObject);
                    secondFloor[x, z] = null;
                }
            }
            node = secondFloor[tempX,tempZ];
            ClearDoorWallSecondFloor(tempX, tempZ, node);
        }

        //room3
        Vector2Int room3Pos;
        for (int i = 0; i < 2; i++)
        {
            do
            {
                room3Pos = GenerateRandomCoord();
                tempX = room3Pos.x + 1;
                tempZ = room3Pos.y + 3;
                isValid = CheckRandomCoord2(room3Pos, room2Size, tempX, tempZ);
            } while (!isValid);
            var obj4 = Instantiate(room3, transform);
            obj4.transform.position = new Vector3(transform.position.x + room3Pos.x * (float)2.5, transform.position.y+5, transform.position.z + room3Pos.y * (float)2.5);
            Vector2 offset3 = obj4.GetComponent<NodeData>().offset;
            obj4.transform.position += new Vector3(offset3.x, 0f, offset3.y);
            doorPathFloor2.Add(new(tempX, tempZ));
            for (int x = room3Pos.x; x < room3Size.x + room3Pos.x; x++)
            {
                for (int z = room3Pos.y; z < room3Size.y + room3Pos.y; z++)
                {
                    Destroy(secondFloor[x, z].gameObject);
                    secondFloor[x, z] = null;
                }
            }
            node = secondFloor[tempX, tempZ];
            ClearDoorWallSecondFloor(tempX, tempZ, node);
        }
    }


    private List<Vertex> ConnectRooms(List<Vector2Int> doorPath)
    {
        List<Vertex> cntRoomTmp = new List<Vertex>();
        List<Vector2Int> unVisitedPath = new List<Vector2Int>(doorPath);
        List<Vector2Int> visitedPath = new List<Vector2Int>
        {
            unVisitedPath[0]
        };
        unVisitedPath.RemoveAt(0);
        
        
        while(unVisitedPath.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            Vector2Int visited = Vector2Int.zero, unvisited = Vector2Int.zero;
            foreach (Vector2Int v in visitedPath)
            {
                foreach(Vector2Int v2 in unVisitedPath)
                {
                    float distance = Vector2Int.Distance(v, v2);
                    
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        visited = v;
                        unvisited = v2;
                    }
                }
            }

            unVisitedPath.Remove(unvisited);
            visitedPath.Add(unvisited);
            
            cntRoomTmp.Add(new Vertex() { point1 = visited, point2 = unvisited });
        }
        return cntRoomTmp;
        
    }

    private void FindPath(Vector2Int source, Vector2Int destination)
    {
        resetAstar();
        List<MazeNodes> visited = new List<MazeNodes>();
        List<MazeNodes> canVisit = new List<MazeNodes>();
        canVisit.Add(firstFloor[source.x, source.y]);
        while(canVisit.Count > 0)
        {
            canVisit.Sort((left, right) => (left.cost + left.heuristicVal).CompareTo(right.cost + right.heuristicVal));
            var node = canVisit[0];
            canVisit.Remove(node);
            visited.Add(node);
            if(node.position == destination)
            {
                Debug.Log("Found path");
                var curr = node;

                while (curr.position != source)
                {
                    curr.visited = true;
                    ClearWalls(curr, curr.parent);
                    curr = curr.parent;
                }
                return;
            }
            for(int i = 0; i < 4; i++)
            {
                var curr = node.position + direction[i];
                if (curr.x >= mazeX || curr.x < 0 || curr.y >= mazeZ || curr.y < 0 || firstFloor[curr.x, curr.y] == null)
                {
                    continue;
                }
                if (visited.Contains(firstFloor[curr.x, curr.y]))
                {
                    continue;
                }

                var currNode = firstFloor[curr.x, curr.y];
                int newCost = node.cost + 1;
                float newHeuristic = Vector2Int.Distance(curr, destination);
                if(newCost < currNode.cost)
                {
                    currNode.cost = newCost;
                    currNode.parent = node;
                    if(!canVisit.Contains(currNode))
                    {
                        canVisit.Add(currNode);
                    }

                }

            }
        }

        

    }

    private void FindPath2(Vector2Int source, Vector2Int destination)
    {
        resetAstar2();
        List<MazeNodes> visited = new List<MazeNodes>();
        List<MazeNodes> canVisit = new List<MazeNodes>();
        canVisit.Add(secondFloor[source.x, source.y]);
        while (canVisit.Count > 0)
        {
            canVisit.Sort((left, right) => (left.cost + left.heuristicVal).CompareTo(right.cost + right.heuristicVal));
            var node = canVisit[0];
            canVisit.Remove(node);
            visited.Add(node);
            if (node.position == destination)
            {
                Debug.Log("Found path");
                var curr = node;

                while (curr.position != source)
                {
                    curr.visited = true;
                    ClearWalls(curr, curr.parent);
                    curr = curr.parent;
                }
                return;
            }
            for (int i = 0; i < 4; i++)
            {
                var curr = node.position + direction[i];
                if (curr.x >= mazeX || curr.x < 0 || curr.y >= mazeZ || curr.y < 0 || secondFloor[curr.x, curr.y] == null)
                {
                    continue;
                }
                if (visited.Contains(secondFloor[curr.x, curr.y]))
                {
                    continue;
                }

                var currNode = secondFloor[curr.x, curr.y];
                int newCost = node.cost + 1;
                float newHeuristic = Vector2Int.Distance(curr, destination);
                if (newCost < currNode.cost)
                {
                    currNode.cost = newCost;
                    currNode.parent = node;
                    if (!canVisit.Contains(currNode))
                    {
                        canVisit.Add(currNode);
                    }

                }

            }
        }



    }

    private void resetAstar()
    {
        for(int  i = 0; i < mazeX; i++)
        {
            for(int j=0;j<mazeZ; j++)
            {
                if (firstFloor[i, j] != null)
                {
                    firstFloor[i,j].cost = int.MaxValue;
                    firstFloor[i,j].heuristicVal = float.MaxValue;
                    firstFloor[i, j].parent = null;
                    firstFloor[i, j].position = new(i, j);
                }
            }
        }
    }

    private void resetAstar2()
    {
        for (int i = 0; i < mazeX; i++)
        {
            for (int j = 0; j < mazeZ; j++)
            {
                if (secondFloor[i, j] != null)
                {
                    secondFloor[i, j].cost = int.MaxValue;
                    secondFloor[i, j].heuristicVal = float.MaxValue;
                    secondFloor[i, j].parent = null;
                    secondFloor[i, j].position = new(i, j);
                }
            }
        }
    }

    private void ClearWalls(MazeNodes node1, MazeNodes node2)
    {
        if (node1.position.x < node2.position.x)
        {
            node1.rightWall.SetActive(false);
            node2.leftWall.SetActive(false);
            node1.rightLight.SetActive(false);
            node2.leftLight.SetActive(false);
        }
        else if (node1.position.x > node2.position.x)
        {
            node1.leftWall.SetActive(false);
            node2.rightWall.SetActive(false);
            node1.leftLight.SetActive(false);
            node2.rightLight.SetActive(false);
        }
        else if (node1.position.y < node2.position.y)
        {
            node1.topWall.SetActive(false);
            node2.bottomWall.SetActive(false);
            node1.topLight.SetActive(false);
            node2.bottomLight.SetActive(false);
        }
        else if (node1.position.y > node2.position.y)
        {
            node2.topWall.SetActive(false);
            node1.bottomWall.SetActive(false);
            node1.bottomLight.SetActive(false);
            node2.topLight.SetActive(false);
        }
    }

    private void ClearDoorWall(int tempX, int tempZ, MazeNodes node)
    {
        // Check if node is null or if its coordinates are out of bounds
        if (node == null || tempX < 0 || tempX >= mazeX || tempZ < 0 || tempZ >= mazeZ)
        {
            return;
        }

        // Check right wall
        if (tempX + 1 < mazeX && firstFloor[tempX + 1, tempZ] == null)
        {
            node.rightWall.SetActive(false);
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);  
        }

        // Check left wall
        if (tempX - 1 >= 0 && firstFloor[tempX - 1, tempZ] == null)
        {
            node.leftWall.SetActive(false);
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);
        }

        // Check top wall
        if (tempZ + 1 < mazeZ && firstFloor[tempX, tempZ + 1] == null)
        {
            node.topWall.SetActive(false);
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);
        }

        // Check bottom wall
        if (tempZ - 1 >= 0 && firstFloor[tempX, tempZ - 1] == null)
        {
            node.bottomWall.SetActive(false);
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);
        }
    }


    private void ClearDoorWallSecondFloor(int tempX, int tempZ, MazeNodes node)
    {
        // Check if node is null or if its coordinates are out of bounds
        if (node == null || tempX < 0 || tempX >= mazeX || tempZ < 0 || tempZ >= mazeZ)
        {
            return;
        }

        // Check right wall
        if (tempX + 1 < mazeX && secondFloor[tempX + 1, tempZ] == null)
        {
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);
            node.rightWall.SetActive(false);
        }

        // Check left wall
        if (tempX - 1 >= 0 && secondFloor[tempX - 1, tempZ] == null)
        {
            node.leftWall.SetActive(false);
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);
        }

        // Check top wall
        if (tempZ + 1 < mazeZ && secondFloor[tempX, tempZ + 1] == null)
        {
            node.topWall.SetActive(false);
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);
        }

        // Check bottom wall
        if (tempZ - 1 >= 0 && secondFloor[tempX, tempZ - 1] == null)
        {
            node.bottomWall.SetActive(false);
            node.leftLight.SetActive(false);
            node.rightLight.SetActive(false);
            node.topLight.SetActive(false);
            node.bottomLight.SetActive(false);
        }
    }



    private Vector2Int GenerateRandomCoord()
    {
        return new Vector2Int(Random.Range(0,mazeX), Random.Range(0,mazeZ));
    }

    private bool CheckRandomCoord(Vector2Int position, Vector2Int roomSize, int tempX,int tempZ)
    {
        if (tempX >= mazeX || tempX < 0 || tempZ >= mazeZ || tempZ < 0 || firstFloor[tempX, tempZ] == null)
        {
            return false;
        }
        for(int x = position.x; x < roomSize.x+position.x; x++)
        {
            for(int y=position.y;y< roomSize.y + position.y; y++)
            {
                if (x >= mazeX || x < 0 || y >= mazeZ || y < 0 || firstFloor[x, y] == null || doorPath.Contains(new(x,y)))
                {
                    return false;
                }
            }
        }
        return true;
        
        
    }

    private bool CheckRandomCoord2(Vector2Int position, Vector2Int roomSize,int tempX,int tempZ)
    {
        if (tempX >= mazeX || tempX < 0 || tempZ >= mazeZ || tempZ < 0 || secondFloor[tempX, tempZ] == null)
        {
            return false;
        }
        for (int x = position.x; x < roomSize.x + position.x; x++)
        {
            for (int y = position.y; y < roomSize.y + position.y; y++)
            {
                if (x >= mazeX || x < 0 || y >= mazeZ || y < 0 || secondFloor[x, y] == null|| doorPathFloor2.Contains(new(x, y)))
                {
                    return false;
                }
            }
        }
        return true;


    }

}
