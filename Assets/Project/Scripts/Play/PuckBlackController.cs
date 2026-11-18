using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PuckBlackController : MonoBehaviour
{
    private const float TransitionEnableDelay = 5f;
    private const float MaxSpeed = 15f;
    private const float MinSpeed = 5f;
    private const float MoveDelay = 0.8f;
    private const float BendDelay = 0.5f;
    private const float BendCoef = 1f;
    private const int ScoreToWin = 9;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector3 initialPosition;
    private Coroutine bendCoroutine;
    private bool canTransition = false;
    private bool isGameOver = false;
    private int scoreRed = 0;
    private int scoreBlue = 0;

    [SerializeField] private FadeController fade;
    [SerializeField] private GameObject[] scoreLampRedOn;
    [SerializeField] private GameObject[] scoreLampRedOff;
    [SerializeField] private GameObject[] scoreLampBlueOn;
    [SerializeField] private GameObject[] scoreLampBlueOff;
    [SerializeField] private TMP_Text resultRedMessage;
    [SerializeField] private TMP_Text resultBlueMessage;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip goalSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = transform.position;

        StartCoroutine(fade.FadeIn());
        StartCoroutine(ResetPuck());
    }

    private void Update()
    {
        if (Input.anyKeyDown && isGameOver && canTransition)
        {
            canTransition = false;
            StartCoroutine(Transition());
        }
    }

    private void FixedUpdate()
    {
        if (MaxSpeed < rb.velocity.magnitude)
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
        }
        if (rb.velocity.magnitude < MinSpeed)
        {
            rb.velocity = rb.velocity.normalized * MinSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        audioSource.PlayOneShot(hitSound);

        if (col.gameObject.CompareTag("Player"))
        {
            var player = col.gameObject.GetComponent<PlayerControllerBase>();

            if (player.IsTricking && bendCoroutine == null && 0 < player.Energy && player.MoveInput != 0)
            {
                player.DecreaseEnergy();
                bendCoroutine = StartCoroutine(Bend(player.MoveInput));
            }
        }

        if (!isGameOver)
        {
            // 赤のゴールに入ったら青の得点、青のゴールに入ったら赤の得点
            if (col.gameObject.CompareTag("GoalRed"))
            {
                scoreBlue = AddScore(scoreBlue, scoreLampBlueOn, scoreLampBlueOff, resultBlueMessage, resultRedMessage);
            }
            if (col.gameObject.CompareTag("GoalBlue"))
            {
                scoreRed = AddScore(scoreRed, scoreLampRedOn, scoreLampRedOff, resultRedMessage, resultBlueMessage);
            }
        }
    }

    // 得点を 1 加えてランプを点け、規定点に達したら勝敗を表示する。加算後の得点を返す
    private int AddScore(int score, GameObject[] lampOn, GameObject[] lampOff, TMP_Text winnerMessage, TMP_Text loserMessage)
    {
        StartCoroutine(ResetPuck());
        lampOff[score].SetActive(false);
        lampOn[score].SetActive(true);
        audioSource.PlayOneShot(goalSound);
        score++;

        if (ScoreToWin <= score)
        {
            isGameOver = true;
            winnerMessage.text = "Win";
            loserMessage.text = "Lose";
            StartCoroutine(WaitForTransitionEnable());
        }
        return score;
    }

    private IEnumerator ResetPuck()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = initialPosition;
        if (bendCoroutine != null)
        {
            StopCoroutine(bendCoroutine);
            bendCoroutine = null;
        }
        yield return new WaitForSeconds(MoveDelay);

        rb.velocity = new Vector2(Random.Range(0, 2) == 0 ? -MinSpeed : MinSpeed, 0);
    }

    private IEnumerator Bend(float bendDirection)
    {
        yield return new WaitForSeconds(BendDelay);

        Vector2 force = new Vector2(0, bendDirection * BendCoef * rb.velocity.magnitude);
        rb.AddForce(force, ForceMode2D.Impulse);

        bendCoroutine = null;
    }

    private IEnumerator WaitForTransitionEnable()
    {
        yield return new WaitForSeconds(TransitionEnableDelay);

        canTransition = true;
    }

    private IEnumerator Transition()
    {
        yield return StartCoroutine(fade.FadeOut());

        SceneManager.LoadScene("Start");
    }
}
