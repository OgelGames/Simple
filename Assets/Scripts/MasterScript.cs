using System.Threading.Tasks;
using UnityEngine;

public class MasterScript : MonoBehaviour
{
    public bool spawnEnemies;
	private bool increaseSpeed;

	void Update ()
	{
		// Fast quit for Windows.
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}

		if (Reference.instance.player.transform.position.x < -10 && Reference.instance.spawner.enemyCount <= 0)
		{
			RestartGame();
		}
	}

	public void StartGame ()
	{
		Reference.instance.floor.moveFloor = true;
		Reference.instance.spawner.StartSpawningEnemies();
		IncreaseSpeed(0.01f);
	}

	public void StopGame ()
	{
		Reference.instance.spawner.StopSpawningEnemies();
		increaseSpeed = false;
	}

	private void RestartGame ()
	{
		Reference.instance.scoreManager.UpdateScore(Reference.instance.player.rank);
		Reference.instance.uIManager.MoveUIDown();
		Reference.instance.player.Reset();
	}

	public void QuitGame ()
	{
		Instantiate(Reference.instance.Outro);
	}

	private async void IncreaseSpeed (float rate)
	{
		increaseSpeed = true;
		float newTimeScale = 1.0f;
		while (increaseSpeed && Application.isPlaying) {
			newTimeScale += rate;
			Time.timeScale = newTimeScale;
			await Task.Delay(1000);
		}
        Time.timeScale = 1.0f;
	}
}
