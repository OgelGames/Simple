using UnityEngine;
using GameJolt.API;
using System.Threading.Tasks;

public class ScoreManager : MonoBehaviour
{
	private int localScore = -1, onlineScore = -1;
	private string guestID;
	
	void Start () {
		// Checks if a guest ID has been assigned.
		if (PlayerPrefs.HasKey ("GuestID")) {
			guestID = PlayerPrefs.GetString ("GuestID");
		} else {
			guestID = "Guest " + ExtraMethods.GenerateRandomString (10);
			PlayerPrefs.SetString ("GuestID", guestID);
		}
	}

	// Handles loading, comparing, and saving the best score.
	public async void UpdateScore(int newScore = 0)
	{
		// Variables.
		int bestScore;
		int[] scoreList = new int[3];
		bool localScoreUpdated = false, onlineScoreUpdated = false;

		// Get saved scores.
		GetLocalScore((bool updated) => {
			localScoreUpdated = updated;
		}, (int score) => {
			localScore = score;
		});
		GetOnlineScore((bool updated) => {
			onlineScoreUpdated = updated;
		}, (int score) => {
			onlineScore = score;
		});

		// Wait for scores to be retrieved.
		while (!localScoreUpdated || !onlineScoreUpdated)
		{
			await Task.Delay(10);
		}

		// Compare scores.
		scoreList[0] = newScore;
		scoreList[1] = localScore;
		scoreList[2] = onlineScore;
		bestScore = Mathf.Max(scoreList);

		// Save and display best score.
		if (bestScore > 0){
			SaveLocalScore(bestScore);
			if(bestScore > onlineScore){
				SaveOnlineScore(bestScore);
			}
		}
		Reference.instance.bestScoreText.text = "Best Score: " + bestScore;
	}
	
    #region Get/Save/Upload score functions

	// Get local score save from local binary file.
	private void GetLocalScore(System.Action<bool> callback = null, System.Action<int> score = null){
		if (localScore < 0){
			Save saveFile = ExtraMethods.LoadFile<Save>(Application.persistentDataPath, "SAVE");
			if (saveFile != null){
				score(saveFile.bestScore);
				saveFile = null;
			}
			callback(true);
		} else {
			score(localScore);
			callback(true);
		}
	}

	// Get online score save from GameJolt.
	private void GetOnlineScore(System.Action<bool> callback = null, System.Action<int> score = null){
		if (onlineScore < 0){
			if (ConnectionManager.isConnected && ConnectionManager.isSignedIn) {
				Scores.Get((GameJolt.API.Objects.Score[] scores) => {	
					if (scores.Length != 0){
						score(scores[0].Value);
					}
					callback(true);
				}, 0, 1, true);
			} else {
				score(onlineScore);
				callback(true);
			}
		} else {
			score(onlineScore);
			callback(true);
		}
	}

	// Save best score to local binary file.
	private void SaveLocalScore(int score)
	{
		Save save = new Save();
		save.bestScore = score;
        ExtraMethods.SaveFile(save, Application.persistentDataPath, "SAVE");
	}
	
	// Upload best score to GameJolt.
	private void SaveOnlineScore(int score)
	{
		if (ConnectionManager.isConnected) {
			if (ConnectionManager.isSignedIn) {
				Scores.Add (score, score.ToString());
			} 
			if (!ConnectionManager.isSignedIn) {
				Scores.Add (score, score.ToString(), guestID);
			}
		}
	}
    #endregion
}


// Save object class.
[System.Serializable]
public class Save {
	public int bestScore;
}
