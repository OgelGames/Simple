using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	internal int enemyCount;
	private bool spawnEnemies;

	public void StartSpawningEnemies()
	{
		spawnEnemies = true;
		StartCoroutine (SpawnEnemies ());
	}

	public void StopSpawningEnemies()
	{
		spawnEnemies = false;
	}

	private IEnumerator SpawnEnemies ()
	{
		yield return new WaitForSecondsRealtime(1f);
		while (spawnEnemies) {	
			Instantiate (Reference.instance.EnemyPrefab, transform.position, Quaternion.identity);
			enemyCount++;
			yield return new WaitForSecondsRealtime(Random.Range(1f, 5f));
		}
	}
}
