using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class FigureAnimationManager : MonoBehaviour
{
    internal bool Walk {set => animator.SetBool("IsWalking", value); }
    public bool IsPunched { get => isPunched; set { isPunched = value; animator.SetBool("Punched", value); } }

    private bool isPunched, animatorReturn;
    private Animator animator;
    private Rigidbody2D body;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetFloat("VerticalSpeed", body.velocity.y);
    }
    
    private void AnimatorReturn ()
	{
		animatorReturn = true;
	}

    public IEnumerator Punch(System.Action<bool> onPunched = null, System.Action<bool> onCompleted = null)
    {
        animator.SetBool("Punch", true);
        while (!animatorReturn)
        {
            yield return null;
        }
        animatorReturn = false;

        onPunched(true);
        animator.SetBool("Punch", false);

        while (!animatorReturn)
        {
            yield return null;
        }
        animatorReturn = false;

        onCompleted(true);
    }    
}
