using UnityEngine;

public class OutroController : MonoBehaviour
{
    private void OnAnimationCompleted ()
    {
        Application.Quit();
    }
}
