using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController gManager;
    private AudioManager audioManager;

    private Rigidbody2D rb;
    private SpriteRenderer sRenderer;
    private Animator animator;

    [SerializeField] private float jumpForce = 1, glidingGravityMultiplier = 0.5f;
    private float gravity;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded, wasGrounded, gliding, inVenti, invi, shownRtext, cooked;

    [SerializeField] private GameObject restartText;
    private Animator rTextAnimator;

    [SerializeField] private Rigidbody2D[] deadPrefabs;

    [SerializeField] private ParticleSystem[] particles;

    [SerializeField] private bool cheat;

    void Start()
    {
        gManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        rb = GetComponent<Rigidbody2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (restartText)
            rTextAnimator = restartText.GetComponent<Animator>();

        gravity = rb.gravityScale;

        inVenti = false;

        invi = false;
        shownRtext = false;
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            CheckGround();

            if (!invi)
            {
                if (isGrounded && Input.GetKeyDown(KeyCode.Space))
                    Jump();

                if (rb.velocity.y < 0 && Input.GetKey(KeyCode.Space))
                    gliding = true;

                if ((gliding && Input.GetKeyUp(KeyCode.Space)) || isGrounded)
                    gliding = false;

                if (gliding && !inVenti)
                {
                    rb.gravityScale = gravity * glidingGravityMultiplier;

                    rb.velocity = new Vector2(0, -rb.gravityScale);
                }
                else if (!inVenti)
                    rb.gravityScale = gravity;

                if (inVenti)
                    rb.velocity = new Vector2(0, 6);
            }
            else
            {
                if (!gliding)
                    gliding = true;

                if (sRenderer.color.a < 1f)
                {// gradually increase alpha when invincible

                    SetAlpha(sRenderer.color.a + Time.deltaTime);
                }
                else // if invi and alpha full
                {
                    if (restartText && !shownRtext)
                    {
                        rTextAnimator.SetTrigger("show");
                        shownRtext = true;
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {// when space clicked while being invincible

                        gliding = false;

                        SetAlpha(1f);
                        invi = false;

                        if (restartText)
                            rTextAnimator.SetTrigger("hide");

                        gManager.CountScore(true);
                    }
                }
            }

            if (transform.position.y < -6f)
            {
                Die();
            }

            animator.SetBool("isGrounded", isGrounded);
            animator.SetBool("gliding", gliding);
        }
        else
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q)) gliding = false;
    }

    private void CheckGround()
    {
        wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(1, 0.1f), 0, groundLayer);

        if (!wasGrounded && isGrounded)
            OnLand();
    }

    private void Jump()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnLand()
    {
        if (gliding)
            gliding = false;
    }

    private void SpawnDeadPlayer()
    {
        int index = 0;

        if (!cooked) index = 1;

        Rigidbody2D deadRb = Instantiate(deadPrefabs[index], transform.position, transform.rotation);
        deadRb.AddForce(Vector2.up * 11, ForceMode2D.Impulse);
        deadRb.AddTorque(60);
    }

    private void Die()
    {
        if (transform.position.y > -6f)
            SpawnDeadPlayer();
            
        if (!cheat) gManager.ResetGame();

        SetAlpha(0f);

        gManager.CountScore(false);

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(-7, 2);

        shownRtext = false;
        invi = true;
    }

    public void SetAlpha(float alpha)
    {
        Color color = sRenderer.color;
        color.a = alpha;
        sRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invi)
        {
            if (collision.gameObject.tag == "Obstacle")
            {
                cooked = !collision.gameObject.name.Contains("Venti") && !collision.gameObject.name.Contains("Chimney");

                if (cooked)
                    audioManager.Play("fire");
                else
                    audioManager.Play("hit");

                Die();
            }
            else if (collision.gameObject.tag == "Coin")
            {
                Destroy(collision.gameObject);
                gManager.SetScore(gManager.GetScore() + 1);

                Instantiate(particles[0].gameObject, collision.transform.position, Quaternion.identity);

                audioManager.Play("collect");
            }
            else if (collision.gameObject.tag == "SpecialCoin")
            {
                Destroy(collision.gameObject);
                gManager.SetScore(gManager.GetScore() + 5);

                Instantiate(particles[1].gameObject, collision.transform.position, Quaternion.identity);

                audioManager.Play("collect");
            }
            else if (collision.gameObject.tag == "Ventilation")
            {
                inVenti = true;
                audioManager.Play("swoosh");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ventilation")
        {
            inVenti = false;
        }
    }
}
