using UnityEngine;
using GameJolt.API.Objects;

public class UIManager : MonoBehaviour
{
	public void GoToWebsite ()
	{
		Application.OpenURL (Reference.instance.websiteURL);
	}

	public void Play ()
	{
		MoveUIUp();
		StartCoroutine(Reference.instance.player.MoveIntoScene());
	}

	public void Quit ()
	{
		Reference.instance.masterScript.QuitGame();
	}

	public void SignInOut ()
	{
		if (Reference.instance.signInButtonText.text == "Sign In"){
			Reference.instance.connectionManager.SignIn (new User (Reference.instance.userNameInput.text, Reference.instance.userTokenInput.text));
		} else {
			Reference.instance.connectionManager.SignOut ();
		}
	}

	public void PlayClick ()
	{
		Reference.instance.buttonClick.Play ();
	}

	public void MoveUIDown()
	{
		StopAllCoroutines();
		Vector3 newPosition = GetNewPosition(Reference.instance.UI, -1);
		StartCoroutine(MoveObject.SmoothMove(Reference.instance.UI, newPosition, 0.2f));
	}

	public void MoveUIUp()
	{
		StopAllCoroutines();
		Vector3 newPosition = GetNewPosition(Reference.instance.UI, 1);
		StartCoroutine(MoveObject.SmoothMove(Reference.instance.UI, newPosition, 0.2f));
	}

	public void MoveCameraDown()
	{
		StopAllCoroutines();
		Vector3 newPosition = GetNewPosition(Reference.instance.MainCamera, -1);
		StartCoroutine(MoveObject.SmoothMove(Reference.instance.MainCamera, newPosition, 0.2f));
	}

	public void MoveCameraUp()
	{
		StopAllCoroutines();
		Vector3 newPosition = GetNewPosition(Reference.instance.MainCamera, 1);
		StartCoroutine(MoveObject.SmoothMove(Reference.instance.MainCamera, newPosition, 0.2f));
	}

	private Vector3 GetNewPosition(GameObject gameObject, int change)
	{
		float currentYPosition = 8 * Mathf.Round(gameObject.transform.position.y / 8);
		return new Vector3(gameObject.transform.position.x, currentYPosition + (8 * change), gameObject.transform.position.z);
	}
}
