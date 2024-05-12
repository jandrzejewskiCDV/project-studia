using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorTrigger : MonoBehaviour{
    public GameObject floorPrefab;
    public float offset = -10;
    public int obstacles = 10;
    public int minDistance = 5;
    public int floors = 2;
    public GameObject[] obstaclePrefabs;

    public GameObject coinPrefab;
    public int coins = 5;
    public int coinsDistance = 3;

    private int debugNumber = 0;

    private GameObject previouslyGeneratedFloor = null;

    private List<GameObject> generatedFloors = new List<GameObject>();

    public void OnFloorRemove(GameObject floor)
    {
        Debug.Log("Removing " + floor.name);
        generatedFloors.Remove(floor);
    }

    public bool IsPrimaryFloor(GameObject floor)
    {
        if (floor == null)
        {
            return false;
        }
        if (!floor.CompareTag("Floor"))
        {
            return false;
        }

        GameObject primaryFloor = generatedFloors.First();
        if (primaryFloor == null)
        {
            Debug.Log("Primary floor is fucking null!");
        }

        return floor.Equals(primaryFloor);
    }

    private void Start()
    {
        spawnFloors();
    }

    void OnTriggerEnter(Collider other){
        if ( GameState.Instance.isRunning() && other.gameObject.CompareTag("GenerateFloorTrigger")){
            spawnFloor();
        }
    }

    void spawnFloors()
    {
        for (int i = 0; i < floors; i++)
        {
            spawnFloor();
        }
    }

    void spawnFloor()
    {
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");

        // Sort the floors array based on their z positions
        Array.Sort(floors, (a, b) => a.transform.position.z.CompareTo(b.transform.position.z));

        // Get the last floor GameObject
        GameObject lastFloor = floors[0];

        // Calculate the ideal offset for spawning the new floor
        Vector3 lastFloorPosition = lastFloor.transform.position;

        // Get the bounds of the last floor prefab
        Bounds lastFloorBounds = lastFloor.GetComponent<Renderer>().bounds;

        // Calculate the spawn coordinates using the last floor's position and the offset
        Vector3 spawnCoordinates = new Vector3(lastFloorPosition.x, lastFloorPosition.y, lastFloorPosition.z + (offset));

        // Instantiate the new floor at the calculated spawn coordinates
        GameObject floor = Instantiate(floorPrefab, spawnCoordinates, Quaternion.identity);
        floor.name = "Floor " + debugNumber++;

        Debug.DrawRay(spawnCoordinates, Vector3.up * 10, Color.yellow, 99999f);

        SpawnObstacles(floor, new List<Vector3>());
        SpawnCoins(floor, new List<Vector3>());

        generatedFloors.Add(floor);

        this.previouslyGeneratedFloor = floor;
    }

    void SpawnObstacles(GameObject floor, List<Vector3> positions)
    {
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        Vector3 minFloorBounds = floorRenderer.bounds.min;
        Vector3 maxFloorBounds = floorRenderer.bounds.max;

        //Get the last obstacles from floor, to ensure that the min distance is respected when spawning new floors
        List<Vector3> oldPositions = null;
        if (previouslyGeneratedFloor != null)
        {
            oldPositions = new List<Vector3>();
            foreach (Transform transform in previouslyGeneratedFloor.transform)
            {
                if (transform.CompareTag("Obstacle"))
                {
                    oldPositions.Add(transform.position);
                }
            }
        }

        Vector3 randomPosition;
        while (true)
        {
            randomPosition = new Vector3(
                UnityEngine.Random.Range(minFloorBounds.x, maxFloorBounds.x),
                1,
                UnityEngine.Random.Range(minFloorBounds.z, maxFloorBounds.z)
            );

            if (TooClose(randomPosition, positions, minDistance) || TooClose(randomPosition, oldPositions, minDistance))
            {
                continue;
            }

            GameObject obstacle = obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)];

            if (CheckSpawnCollision(randomPosition, obstacle.GetComponent<Renderer>().bounds))
            {
                continue;
            }

            // If position is valid, instantiate the obstacle at the random position
            GameObject obstacleInstance = Instantiate(obstacle, randomPosition, Quaternion.identity);
            obstacleInstance.transform.parent = floor.transform;

            // Add the position of the newly spawned obstacle to the list of positions
            positions.Add(randomPosition);

            // Break out of the loop if the desired number of obstacles has been spawned
            if (positions.Count >= obstacles)
            {
                break;
            }
        }
    }

    public bool CheckSpawnCollision(Vector3 spawnPosition, Bounds objectBounds)
    {
        Collider[] colliders = Physics.OverlapBox(spawnPosition, objectBounds.extents, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Wall") || collider.gameObject.CompareTag("Obstacle"))
            {
                return true;
            }
        }

        return false;
    }

    void SpawnCoins(GameObject floor, List<Vector3> positions)
    {
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        Vector3 minFloorBounds = floorRenderer.bounds.min;
        Vector3 maxFloorBounds = floorRenderer.bounds.max;

        Renderer renderer = coinPrefab.GetComponentInChildren<Renderer>();
        Bounds bounds = renderer.bounds;

        Vector3 randomPosition;
        while (true)
        {
            randomPosition = new Vector3(
                UnityEngine.Random.Range(minFloorBounds.x, maxFloorBounds.x),
                1,
                UnityEngine.Random.Range(minFloorBounds.z, maxFloorBounds.z)
            );

            if(TooClose(randomPosition, positions, coinsDistance))
            {
                continue;
            }

            if (CheckSpawnCollision(randomPosition, bounds))
            {
                continue;
            }

            // If position is valid, instantiate the obstacle at the random position
            GameObject coinInstance = Instantiate(coinPrefab, randomPosition, Quaternion.identity);
            coinInstance.transform.parent = floor.transform;

            // Add the position of the newly spawned obstacle to the list of positions
            positions.Add(randomPosition);

            // Break out of the loop if the desired number of obstacles has been spawned
            if (positions.Count >= coins)
            {
                break;
            }
        }
    }

    bool TooClose(Vector3 position, List<Vector3> positions, int minDistance)
    {

        if(positions == null)
        {
            return false;
        }

        foreach (Vector3 lastPosition in positions)
        {
            float distance = Vector3.Distance(position, lastPosition);

            if (distance < minDistance)
            {
                return true;
            }
        }

        return false;
    }
}
