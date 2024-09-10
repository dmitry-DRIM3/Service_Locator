using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _roadPrefabs = new List<GameObject>();
    [SerializeField] private int _countSpawns;
    [SerializeField] private float _lenghtRoad;

    private Queue<GameObject> _roads = new Queue<GameObject>();
    private PlayerMovementController _playerMovementController;

    private void Awake()
    {
        Spawn();
    }

    private void Start()
    {
        _playerMovementController = ServiceLocator.GetService<PlayerMovementController>();       
    }

    private void Update()
    {
        ReloadRoad();
    }

    private void Spawn()
    {
        Vector3 position = Vector3.zero;
        for (int i = 0; i < _countSpawns; i++)
        {
            position.z += _lenghtRoad;
            int indexRoad = Random.Range(0, _roadPrefabs.Count);
            GameObject road = Instantiate(_roadPrefabs[indexRoad], position, Quaternion.identity);
            _roads.Enqueue(road);
        }
    }

    private void ReloadRoad()
    {
        GameObject road = _roads.Peek();
        Vector3 playerPosition = _playerMovementController.GetPlayerPosition();

        if(Vector3.Distance(playerPosition,road.transform.position) > _lenghtRoad)
        {       
            road = _roads.Dequeue();
            Vector3 position = road.transform.position;
            position.z += _countSpawns * _lenghtRoad;
            road.transform.position = position;
            _roads.Enqueue(road);
        }
    }
}
