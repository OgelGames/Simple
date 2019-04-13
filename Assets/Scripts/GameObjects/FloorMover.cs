using UnityEngine;

public class FloorMover : MonoBehaviour
{
	public float speed = 2.5f;
	public bool moveFloor;

	void Update ()
	{
		if (moveFloor) {
			transform.Translate (Vector2.left * speed * Time.deltaTime);
		}
	}
}
