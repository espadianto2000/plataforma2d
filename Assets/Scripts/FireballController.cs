
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed;
    public float timeToDestroy;

    private Vector3 mDirection;
    private float mTimer = 0f;
    private barraPower bp;

    private void Start()
    {
        mDirection = GameManager.GetInstance().hero.GetDirection();
        bp = GameObject.Find("Bar_6").GetComponent<barraPower>();
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * mDirection;

        mTimer += Time.deltaTime;
        if (mTimer > timeToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("enemy"))
        {
            bp.hitEnemigo();
            collision.transform.GetComponent<EnemyController>().bajarVida();
        }
        Destroy(gameObject);
    }
}
