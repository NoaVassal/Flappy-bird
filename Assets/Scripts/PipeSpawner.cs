using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private PipeController _pipePrefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private float _firstSpawnDelay = 1f;
    [SerializeField] private float _spawnInterval = 1.5f;

    [SerializeField] private float _minY = -2f;
    [SerializeField] private float _maxY = 2f;

    private Coroutine _spawnCoroutine;

    public void StartSpawning()
    {
        if (_spawnCoroutine != null)
        {
            return;
        }

        _spawnCoroutine = StartCoroutine(SpawnPipes());
    }

    public void StopSpawning()
    {
        if (_spawnCoroutine == null)
        {
            return;
        }

        StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = null;
    }

    private IEnumerator SpawnPipes()
    {
        yield return new WaitForSeconds(_firstSpawnDelay);

        while (GameManager.Instance.CurrentState == GameState.Game)
        {
            SpawnPipe();

            yield return new WaitForSeconds(_spawnInterval);
        }

        _spawnCoroutine = null;
    }

    private void SpawnPipe()
    {
        var spawnPosition = _spawnPoint.position;

        spawnPosition.y = Random.Range(_minY, _maxY);

        Instantiate(
            _pipePrefab,
            spawnPosition,
            Quaternion.identity);
    }
}