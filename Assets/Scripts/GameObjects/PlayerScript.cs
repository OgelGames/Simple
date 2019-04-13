using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

	// Public variables.
	[Header("References")]
	public Text headText;

	[Header("Player Movement")]
	public float walkSpeed = 2.5f;
	public float jumpSpeed = 8.9f;
	public float fallMultiplier = 1.4f;
	
	internal int rank = 0;

	// Private variables.
	private Rigidbody2D body;
    private RigidbodyConstraints2D constraints;
	private FigureAnimationManager animator;
	private Vector3 startPosition;
	private bool isActive, playerJump;
	private EnemyScript enemy;

    void Start ()
	{
		body = GetComponent<Rigidbody2D>();
        constraints = body.constraints;
		animator = GetComponent<FigureAnimationManager>();
		startPosition = transform.position;
		SetHeadText();
	}
	
	void Update ()
	{
		// Get jump input
		if (Input.GetMouseButtonDown (0) | Input.GetKeyDown(KeyCode.Space) && transform.position.y < -1.8 && isActive) {
			playerJump = true;
			Reference.instance.jump.Play ();
		}
	}

	void FixedUpdate () {
		//Apply jumping
		if (playerJump){
			body.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
			playerJump = false;
		}
		if (isActive)
		{
			if (body.velocity.y < 0) 
			{
				body.gravityScale = fallMultiplier;
			} else {
				body.gravityScale = 1f;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			isActive = false;
			animator.Walk = false;
			Reference.instance.floor.moveFloor = false;
			enemy = other.gameObject.GetComponent<EnemyScript>();
			enemy.StopMoving();
			if (rank >= enemy.rank) {
				this.Punch();
			} else {
				enemy.Punch();
			}
		}
	}

	private void SetHeadText (bool isDead = false)
	{
		if (!isDead)
		{
			headText.text = rank.ToString();
		} else {
			headText.text = "XX";
		}
	}

	private void CanBePunched(bool canBePunched)
	{
		if (!canBePunched)
		{
            body.constraints = constraints;
		}
        else
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

	private void Punch ()
	{
		StartCoroutine(animator.Punch(
			(onPunched) => {
				enemy.GetPunched();
				Reference.instance.punch.Play();
				rank++;
				SetHeadText();
			}, (onCompleted) => {
				isActive = true;
				animator.Walk = true;
				Reference.instance.floor.moveFloor = true;
			}
		));
	}

	public void GetPunched ()
	{
		Reference.instance.masterScript.StopGame();
		body.gravityScale = 0f;
		CanBePunched(true);
		body.AddForce(new Vector2(-15f, 3f), ForceMode2D.Impulse);
		animator.IsPunched = true;
		SetHeadText(isDead: true);
	}

	public void Reset()
	{
		animator.IsPunched = false;
		CanBePunched(false);
		body.velocity = Vector3.zero;
		body.gravityScale = 1f;
		transform.position = startPosition;
		rank = 0;
		SetHeadText();
	}

	public IEnumerator MoveIntoScene()
	{
		animator.Walk = true;
		while (transform.position.x < -2.5f)
		{
			transform.Translate (Vector2.right * walkSpeed * Time.deltaTime);
			yield return null;
		}
		isActive = true;
		Reference.instance.masterScript.StartGame();
	}
}
