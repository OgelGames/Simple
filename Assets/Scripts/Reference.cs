using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Reference : MonoBehaviour 
{
	[Header("String references")]
	public string websiteURL;

	[Header("Script references")]
	public MasterScript masterScript;
	public ScoreManager scoreManager;
	public ConnectionManager connectionManager;
	public UIManager uIManager;
	public Spawner spawner;
	public FloorMover floor;
	public PlayerScript player;

	[Header("GameObject references")]
	public GameObject MainCamera;
	public GameObject UI;
	public GameObject EnemyPrefab;
	public GameObject Outro;

	[Header("UI Object references")]
	public Button SignInButton;	
	public Text signInButtonText;
	public Text bestScoreText;
	public Text connectionStatusText;
	public InputField userNameInput;
	public InputField userTokenInput;
	
	[Header("Audio references")]
	public AudioSource buttonClick;
	public AudioSource jump;
	public AudioSource punch;
	public AudioSource gameOver;

	internal static Reference instance;	

	void Awake()
	{
		instance = this;
	}
}

