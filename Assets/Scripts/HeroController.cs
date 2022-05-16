
using System;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float accel;
    public float deccel;
    public float speedExp;

    [Header("Jump")]
    public float raycastDistance;
    public float jumpForce;
    public float fallMultiplier;

    [Header("Fire")]
    public GameObject fireball; //prefab
    private Transform mFireballPoint;

    [Header("dash")]
    public barraPower bp;
    private bool dash = false;
    private Vector3 posDash = Vector3.zero;
    public float velDash;

    [Header("vida")]
    public barraVida bv;
    private bool vivo = true;

    private Rigidbody2D mRigidBody;
    private float mMovement;
    private Animator mAnimator;
    private SpriteRenderer mSpriteRenderer;
    private bool doubleJ = true;
    public Collider2D platColl;
    

    private void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mFireballPoint = transform.Find("FireballPoint");
        bv.AddDeadDelegate(onDeadDelegate);
    }

    private void Update()
    {
        if (vivo)
        {
            if (!dash)
            {
                mMovement = Input.GetAxis("Horizontal");
                mAnimator.SetInteger("Move", mMovement == 0f ? 0 : 1);

                if (mMovement < 0f)
                {
                    //mSpriteRenderer.flipX = true;
                    transform.rotation = Quaternion.Euler(
                        0f,
                        180f,
                        0f
                    );
                }
                else if (mMovement > 0f)
                {
                    //mSpriteRenderer.flipX = false;
                    transform.rotation = Quaternion.Euler(
                        0f,
                        0f,
                        0f
                    );
                }
            }
            

            bool isOnAir = IsOnAir();
            if (Input.GetButtonDown("Jump"))
            {
                if (dash)
                {
                    dash = false;
                    terminarDash();
                }
                if (!isOnAir)
                {
                    Jump();
                }
                else if (doubleJ)
                {
                    doubleJ = false;
                    Jump();
                }

            }
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                if (bp.sl.fillAmount >= 1)
                {
                    dashear();
                    bp.sl.fillAmount = 0;
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
            if (dash)
            {
                transform.position = Vector3.MoveTowards(transform.position, posDash, velDash * Time.deltaTime);
                if (Vector3.Distance(transform.position, posDash) < 0.2)
                {
                    terminarDash();
                }
            }
        }
        
    }


    private void FixedUpdate()
    {
        if (vivo)
        {
            Move();
            if (mRigidBody.velocity.y < 0)
            {
                // Esta cayendo
                mRigidBody.gravityScale = 5;
                /*
                mRigidBody.velocity += (fallMultiplier - 1) * 
                    Time.fixedDeltaTime * Physics2D.gravity;
                */
            }
        }
    }

    private void Move()
    {
        float targetSpeed = mMovement * moveSpeed;
        float speedDif = targetSpeed - mRigidBody.velocity.x;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? accel : deccel;
        float movement = Mathf.Pow(
            accelRate * Mathf.Abs(speedDif),
            speedExp
        ) * Mathf.Sign(speedDif);

        mRigidBody.AddForce(movement * Vector2.right);
    }

    private void Jump()
    {
        mAnimator.SetTrigger("jump");
        mRigidBody.velocity = new Vector2(mRigidBody.velocity.x, 0);
        mRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        mRigidBody.gravityScale = 1;
    }
    private void dashear()
    {
        mRigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
        Collider2D[] cols = GetComponents<Collider2D>();
        cols[0].enabled = false;
        mAnimator.SetTrigger("dash");
        if (transform.rotation.y == 0)
        {
            posDash = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
        }
        else
        {
            posDash = new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z);
        }
        dash = true;
        Debug.Log("dash");
    }
    private void terminarDash()
    {
        mRigidBody.constraints = RigidbodyConstraints2D.None;
        mRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Collider2D[] cols = GetComponents<Collider2D>();
        if(platColl)
        {
            Debug.Log("intersecta");
            bv.sl.fillAmount = 0;
        }
        else
        {
            cols[0].enabled = true;
        }
        dash = false;
        
    }

    public bool IsOnAir()
    {
        Transform rayCastOrigin = transform.Find("RaycastPoint");
        RaycastHit2D hit = Physics2D.Raycast(
            rayCastOrigin.position,
            Vector2.down,
            raycastDistance
        );
        mRigidBody.gravityScale = hit ? 1 : mRigidBody.gravityScale;
        mAnimator.SetBool("IsJumping", !hit);
        if (!hit && mAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name !="Jump")
        {
            mAnimator.SetTrigger("jump");
        }
        doubleJ = !doubleJ ? hit : doubleJ;
        return !hit;
    }
    private void Fire()
    {
        mFireballPoint.GetComponent<ParticleSystem>().Play(); // ejecutamos PS
        GameObject obj = Instantiate(fireball, mFireballPoint);
        obj.transform.parent = null;
    }

    public Vector3 GetDirection()
    {
        return new Vector3(
            transform.rotation.y == 0f ? 1f : -1f,
            0f,
            0f
        );
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("platform"))
        {
            platColl = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("platform"))
        {
            platColl = null;
        }
    }
    private void onDeadDelegate(object sender, EventArgs e)
    {
        Collider2D[] cols = GetComponents<Collider2D>();
        cols[1].enabled = false;
        vivo = false;
        mRigidBody.velocity = Vector3.zero;
        mRigidBody.gravityScale = 0;
    }
}
