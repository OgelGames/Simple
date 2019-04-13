using UnityEngine;
using GameJolt.API;
using GameJolt.API.Objects;
using System.Threading.Tasks;

public class ConnectionManager : MonoBehaviour
{
	public static bool isConnected, isSignedIn;

	// Check connection on scene load
	void Start()
	{
		ConnectionCheck();
	}

	// Monitors the device's connection to the internet.
	void Update ()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable && isConnected)
		{
			isConnected = false;
			ConnectionCheck();
		}
	}

	private async void ConnectionCheck()
	{
		// Disable user input while not connected.
		DisableInput();
		Reference.instance.connectionStatusText.text = "Check internet connection...";

		// Wait for connection.
		while (Application.internetReachability == NetworkReachability.NotReachable)
		{
			await Task.Delay(1000);
		}

		// Wait for connection to stabilize
		await Task.Delay(1000);

		isConnected = true;
		TryAutoSignIn();
	}

	private void TryAutoSignIn ()
	{
		// Tell the player to wait.
		Reference.instance.connectionStatusText.text = "Please wait...";
	
		// Sign out any user to avoid errors.
		if (GameJoltAPI.Instance.HasUser)
		{
			SignOut ();
		}

		// Signs the player in if the player's username and token has been stored on the device.
		if (PlayerPrefs.HasKey ("UserName") && PlayerPrefs.HasKey ("UserToken"))
		{
			SignIn (new User (PlayerPrefs.GetString ("UserName"), PlayerPrefs.GetString ("UserToken")));
		} else {
			Reference.instance.connectionStatusText.text = "Please sign in to GameJolt...";
			Reference.instance.signInButtonText.text = "Sign In";
			EnableInput();
		}
	}

	// Disables all player input.
	private void DisableInput()
	{
		Reference.instance.SignInButton.interactable = false;
		Reference.instance.userNameInput.interactable = false;
		Reference.instance.userTokenInput.interactable = false;
	}

	// Enables all player input.
	private void EnableInput()
	{
		Reference.instance.SignInButton.interactable = true;
		Reference.instance.userNameInput.interactable = true;
		Reference.instance.userTokenInput.interactable = true;
	}

	// Signs the player out.
	public void SignOut ()
	{
		if (GameJoltAPI.Instance.HasUser)
		{
			GameJoltAPI.Instance.CurrentUser.SignOut ();
			Reference.instance.connectionStatusText.text = "Please sign in to GameJolt...";
			Reference.instance.signInButtonText.text = "Sign In";
			isSignedIn = false;
			EnableInput();
		}
	}

	// Signs the player in.
	public void SignIn (User Player)
	{
		if (GameJoltAPI.Instance.HasUser) {
			GameJoltAPI.Instance.CurrentUser.SignOut ();
		}
		DisableInput();
		Reference.instance.connectionStatusText.text = "Please wait...";
		Player.SignIn(
			signInSuccess => {
				Reference.instance.connectionStatusText.text = string.Format ("Sign in {0}!", signInSuccess ? "successful" : "failed");
				if (signInSuccess){
					PlayerPrefs.SetString ("UserName", Player.Name);
					PlayerPrefs.SetString ("UserToken", Player.Token);
					Reference.instance.userNameInput.text = string.Empty;
					Reference.instance.userTokenInput.text = string.Empty;
					Reference.instance.signInButtonText.text = "Sign Out";
					isSignedIn = true;
					Reference.instance.scoreManager.UpdateScore();
				} else {
					Reference.instance.signInButtonText.text = "Sign In";
					isSignedIn = false;
				}
				EnableInput();
			},
			userFetchSuccess => {
				if(userFetchSuccess){
					Reference.instance.connectionStatusText.text = string.Format ("Signed in as {0}", Player.Name);
				}
			});
	}
}
