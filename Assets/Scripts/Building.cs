using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private GameController gManager;
    private MapManager mapManager;

    private Vector3 currentPosition;
    private float movementSpeed;

    [SerializeField] private float minX;

    [SerializeField] private bool isHome;

    void Start()
    {
        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        mapManager = gManager.GetComponent<MapManager>();

        currentPosition = transform.position;
    }

    void Update()
    {
        if (movementSpeed != 0)
            Move();

        if (transform.position.x <= minX)
        {
            mapManager.SpawnBuilding(mapManager.GetNextBuilding());
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        movementSpeed = speed;
    }

    private void Move()
    {
        currentPosition -= new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        transform.position = currentPosition;
    }
}
