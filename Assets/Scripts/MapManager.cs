using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private GameController gManager;

    [SerializeField] private float startSpeed, gapLength, minY, maxY;
    private float currentSpeed;

    [SerializeField] private Transform buildingsParent;
    [SerializeField] private Building buildingPrefab, homePrefab;
    [SerializeField] private GameObject[] collectiblePrefabs;

    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Sprite[] buildingSprites, homeSprites;

    private Building[] buildings;

    void Start()
    {
        gManager = GetComponent<GameController>();

        SetBuildings();

        SetSpeed(startSpeed);
    }

    void Update()
    {
        if (currentSpeed < 20)
            SetSpeed(currentSpeed += Time.deltaTime * 0.08f);
    }

    private void SetBuildings()
    {
        buildings = buildingsParent.GetComponentsInChildren<Building>();
    }

    public void SpawnBuilding(bool home)
    {
        Building prefab = buildingPrefab;

        if (home) prefab = homePrefab;

        Transform prevBuilding = buildings[buildings.Length - 1].transform;

        minY += 2f;

        float y = prevBuilding.position.y + Random.Range(-2f, 2f);
        y = Mathf.Clamp(y, minY, maxY);

        minY -= 2f;

        Building newBuilding = Instantiate(prefab, new Vector3(prevBuilding.position.x + gapLength + Random.Range(-1f, 4f), y, 0), Quaternion.identity, buildingsParent.transform);

        newBuilding.name = "Building";
        newBuilding.SetSpeed(currentSpeed);

        if (!home)
            newBuilding.GetComponent<SpriteRenderer>().sprite = buildingSprites[Random.Range(0, buildingSprites.Length)];
        else
            newBuilding.GetComponent<SpriteRenderer>().sprite = homeSprites[Random.Range(0, homeSprites.Length)];

        SetBuildings();

        // OBSTACLES

        float minObstacleX = -5f, maxObstacleX = 5f;

        if (home)
        {
            minObstacleX = -5;
            maxObstacleX = 1;
        }

        bool obstaclePlaced = false;
        GameObject newObstacle = null;
        if (Random.Range(0, 3) < 2)
        {
            newObstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], Vector3.zero, Quaternion.identity, newBuilding.transform);
            newObstacle.transform.localPosition = new Vector3(Random.Range(minObstacleX, maxObstacleX), 5f, 0);
            
            obstaclePlaced = true;
        }

        // COLLECTIBLES

        float minCollX = -3f, maxCollX = 3f;

        if (home)
        {
            minCollX = -7f;
            maxCollX = -3f;
        }

        if (Random.Range(0,5) < 3)
        {
            Vector3 collectiblePos = new Vector3(Random.Range(minCollX, maxCollX), 4.5f, 0);
            for (int i = 0; i < Random.Range(3,6); i++)
            {
                GameObject newCollectible = null;

                if (Random.Range(0, 15) >= 1)
                {
                    newCollectible = Instantiate(collectiblePrefabs[0], Vector3.zero, Quaternion.identity, newBuilding.transform);
                }
                else
                {
                    newCollectible = Instantiate(collectiblePrefabs[1], Vector3.zero, Quaternion.identity, newBuilding.transform);
                }

                newCollectible.transform.localPosition = collectiblePos + new Vector3(i, 0, 0);

                if (obstaclePlaced && Vector3.Distance(newCollectible.transform.localPosition, newObstacle.transform.localPosition) < 2f)
                {
                    Destroy(newCollectible);
                    break;
                }
            }
        }
    }

    private void SetSpeed(float speed)
    {
        currentSpeed = speed;
        foreach (Building building in buildings)
        {
            building.SetSpeed(currentSpeed);
        }
    }

    public void ResetMap()
    {
        SetSpeed(startSpeed);
    }

    public bool GetNextBuilding()
    {
        bool home = false;

        float s = gManager.GetScore();

        if (s >= 60 && s < 80)
        {
            if (Random.Range(0, 8) == 0) home = true;
            //Debug.Log(s + " 60-80 " + home);
        }
        else if (s >= 80 && s < 100)
        {
            if (Random.Range(0, 5) == 0) home = true;
            //Debug.Log(s + " 80-100 " + home);
        }
        else if (s >= 80 && s < 100)
        {
            if (Random.Range(0, 3) == 0) home = true;
            //Debug.Log(s + " 80-100 " + home);
        }
        else if (s >= 100)
        {
            home = true;
            //Debug.Log(s + " 100+ " + home);
        }

        return home;
    }
}
