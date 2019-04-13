using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
	[Header("References")]
	public Text headText;
	[Header("Enemy Movement")]
	public float walkSpeed = 5f;

	internal int rank = 0;
	private FigureAnimationManager animator;
	private Rigidbody2D body;
    private bool isMoving = true;

    void Start () {
		body = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<FigureAnimationManager>();
		rank = (int)(Mathf.Clamp ((Random.Range (Reference.instance.player.rank - 5, Reference.instance.player.rank + 5)), 0, Mathf.Infinity));
		SetHeadText();
		animator.Walk = true;
	}
	
	void Update () {
		if (isMoving)
		{
			transform.Translate (Vector2.left * walkSpeed * Time.deltaTime);
		}
	}

	// Destroy on contact with enemy destroyer.
	void OnTriggerEnter2D(Collider2D other)
	{
		Reference.instance.spawner.enemyCount--;
		Destroy(gameObject);
	}

	private void SetHeadText(bool isDead = false)
	{
		if (!isDead){
			headText.text = rank.ToString();
		} else {
			headText.text = "XX";
		}
	}

	public void StopMoving()
	{
		isMoving = false;
		animator.Walk = false;
	}

	public void Punch()
	{
		StartCoroutine(animator.Punch(
			(onPunched) => {
				Reference.instance.player.GetPunched();
				Reference.instance.gameOver.Play();
			}, (onCompleted) => {
				isMoving = true;
				animator.Walk = true;
			}
		));
	}

	public void GetPunched()
	{
		body.gravityScale = 0f;
		body.AddForce(new Vector2(15f, 3f), ForceMode2D.Impulse);
		animator.IsPunched = true;
		SetHeadText(isDead: true);
	}
}
