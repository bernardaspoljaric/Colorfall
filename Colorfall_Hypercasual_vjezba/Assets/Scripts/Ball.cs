using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Ball : MonoBehaviour {

	public static bool Move = false;
	[SerializeField] float jumpStrength = 100;
	[SerializeField] float gravityForce = 10;

	Cylinder cylinder;
	Rigidbody rb;

	public GameObject cube;

	float nextBallPosToJump;
	int skippedCounter = 0;
	float vel;

	public int score = 0;
	public int coins;
	public Text scoreText;
	public Text finalScoreText;
	public Text highscoreText;
	public Text coinsText;
	public Text coinsTextShop;
	public Image winPanel;
	public Slider volumeSlider;

	public AudioSource ass;
	public AudioClip jumpClip;
	public AudioClip deathClip;
	public AudioClip collectClip;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		cylinder = FindObjectOfType<Cylinder>();

		ass = GetComponent<AudioSource>();
		ass.volume = PlayerPrefs.GetFloat("Volume");
		volumeSlider.value = PlayerPrefs.GetFloat("Volume");

		nextBallPosToJump = cylinder.firstCirclePos + GetComponent<SphereCollider>().bounds.size.y / 2 + cylinder.circlesHeight;
		highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("Highscore").ToString();
		coinsText.text = PlayerPrefs.GetInt("Coins").ToString();
		coinsTextShop.text = PlayerPrefs.GetInt("Coins").ToString();
		coins = PlayerPrefs.GetInt("Coins");
	}

	void Update() 
	{
		Debug.DrawLine(transform.position, transform.position + Vector3.down * cylinder.distanceBtwCircles / 2);
	}

	void FixedUpdate() 
	{
		if (!Move)
			return;

		vel -= gravityForce * Time.deltaTime;

		float overlap = nextBallPosToJump - (transform.position.y + vel);
		if (overlap >= 0)
		{
			transform.Translate(Vector3.up * (vel + overlap));
			CheckCollision();
		}
		transform.Translate(Vector3.up * vel);
	}

	void CheckCollision() 
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.down, out hit, cylinder.distanceBtwCircles / 2,
			LayerMask.GetMask("Circles"))) {
			if (hit.collider.CompareTag("Good")) {
				if (skippedCounter >= 2) {
					// TODO: Apply good-looking break force.
					if (hit.collider.transform.parent.CompareTag("Cylinder Object")) {
						Destroy(hit.collider.gameObject);
					}
					else {
						Destroy(hit.collider.transform.parent.gameObject);	
					}
					score += skippedCounter * 10;
					scoreText.text = score.ToString();
				}

				skippedCounter = 0;
				Jump();
				Debug.Log("Good.");
			}
			else if (hit.collider.CompareTag("Bad"))
			{
				ass.clip = deathClip;
				ass.Play();
				SceneManager.LoadScene("Game");
			}
			else if (hit.collider.CompareTag("Finish"))
			{
				winPanel.gameObject.SetActive(true);
				finalScoreText.text = score.ToString();

				coins += 300;
				PlayerPrefs.SetInt("Coins", coins);

				int highscore = PlayerPrefs.GetInt("Highscore");
				if(score > highscore)
                {
					PlayerPrefs.SetInt("Highscore", score);
                }

				Stop();

			}
			else
			{
				Debug.LogWarning("COLLIDED WITH UNKNOWN OBJECT.");
			}
		}
		else
		{
			++skippedCounter;
			score += 10;
			scoreText.text = score.ToString();
			nextBallPosToJump -= cylinder.distanceBtwCircles;
		}
	}
	private void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Shape")
        {
			ass.clip = collectClip;
			ass.Play();
			GetComponent<MeshFilter>().mesh = cube.GetComponent<MeshFilter>().mesh;
			Destroy(other.gameObject);

			score += score * 2;
		}
	}

	void Jump()
	{
		Debug.Log("Jump");
		vel = jumpStrength;
		ass.clip = jumpClip;
		ass.Play();
	}

	public void Stop()
	{
		Move = false;
		vel = 0;
	}

	public void ChangeVolume()
	{
		ass.volume = volumeSlider.value;
		PlayerPrefs.SetFloat("Volume", volumeSlider.value);
	}
}
