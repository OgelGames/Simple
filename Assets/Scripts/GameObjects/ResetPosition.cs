using UnityEngine;

// Resets floor section position.
public class ResetPosition : MonoBehaviour
{
	public float endPositionX, moveAmount;

	void Update ()
	{
		if (transform.position.x <= endPositionX) {
			transform.Translate (moveAmount, 0, 0);
		}
	}
}
