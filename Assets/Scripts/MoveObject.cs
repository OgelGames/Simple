using System.Collections;
using UnityEngine;

public static class MoveObject
{
	// Smoothly moves the specified Object to an exact Target over an approximate amount of time. 
	public static IEnumerator SmoothMove (GameObject Object, Vector3 Target, float time)
	{
		Vector3 velocity = Vector3.zero;
		while (Object.transform.position != Target) {
			Object.transform.position = Vector3.SmoothDamp (Object.transform.position, Target, ref velocity, time);
			yield return null;
		}
		Object.transform.position = Target;
	}
		
	// Moves the specified Object to an exact Target with a constant speed.
	public static IEnumerator LinearMove (GameObject Object, Vector3 Target, float speed)
	{
		while (Object.transform.position != Target) {
			Object.transform.position = Vector3.MoveTowards (Object.transform.position, Target, speed * Time.deltaTime);
			yield return null;
		}
		Object.transform.position = Target;
	}
}
