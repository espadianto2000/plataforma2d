using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    GameObject player;
    bool vivo = true;
    public float speed;
    bool caminando = true;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float vidaMax;
    private float vida;
    public GameObject bv; 
    public Slider vidaSlider;
    public float timerAttack;
    public GameObject proyectil;
    public GameObject puntoFireBall;
    public float timerTP;
    public GameObject portal;
    private GameObject p1;
    private GameObject p2;
    public GameObject at1;
    public GameObject at2;
    public GameObject at3;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Hero");
        vida = vidaMax;
        animator.SetInteger("state", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (vivo)
        {
            if (animator.GetInteger("state")==1)
            {
                if(player != null)
                {
                    if(Mathf.Abs(transform.position.x - player.transform.position.x)<0.4f){
                        animator.SetInteger("state", 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, player.transform.position.x < transform.position.x ? 180 : 0, 0);
                        //spriteRenderer.flipX = player.transform.position.x < transform.position.x ? true : false;
                        transform.position = Vector2.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
                        if (Mathf.Abs(player.transform.position.y - transform.position.y) > 7)
                        {
                            if (timerTP > 0)
                            {
                                timerTP -= Time.deltaTime;
                            }
                            else
                            {
                                animator.SetInteger("state", 4);
                            }
                        }
                    }
                }
                else
                {
                    animator.SetInteger("state",0);
                }
                
            }
            if (animator.GetInteger("state") == 0 && player != null)
            {
                if(Mathf.Abs(transform.position.x - player.transform.position.x) >= 0.4f)
                {
                    animator.SetInteger("state", 1);
                }
                if (Mathf.Abs(player.transform.position.y - transform.position.y) > 7)
                {
                    if (timerTP > 0)
                    {
                        timerTP -= Time.deltaTime;
                    }
                    else
                    {
                        animator.SetInteger("state", 4);
                    }
                }
            }
            if(vidaSlider.value <= 0)
            {
                vivo = false;
                animator.SetTrigger("die");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                GetComponent<Collider2D>().enabled = false;
            }
            if(player != null && Mathf.Abs(transform.position.x - player.transform.position.x) < 3 && Mathf.Abs(transform.position.y - player.transform.position.y) < 3f)
            {
                animator.SetInteger("state", 2);
            }
            if(timerAttack > 0)
            {
                timerAttack -= Time.deltaTime;
            }
            else if(player!=null && Vector2.Distance(transform.position, player.transform.position) > 7)
            {
                timerAttack = 5;
                //puntoFireBall.transform.position = new Vector3(puntoFireBall.transform.position.x, transform.rotation.y == 180 ? -0.79f : 0.79f, puntoFireBall.transform.position.z);
                animator.SetInteger("state", 3);
            }
            
        }
    }

    public void inicioTP()
    {
        p1 = Instantiate(portal, new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z), Quaternion.identity);
        p2 = Instantiate(portal, new Vector3(transform.position.x, player.transform.position.y - 0.25f, transform.position.z), Quaternion.identity);
    }
    public void teleport()
    {
        transform.position = new Vector3(transform.position.x, p2.transform.position.y, transform.position.z);
        Destroy(p1);
        Destroy(p2);
    }
    public void endTeleport()
    {
        timerTP = 7;
        animator.SetInteger("state", 1);
    }

    public void bajarVida()
    {
        vida -= vidaMax * 0.01f;
        vidaSlider.value = vida / vidaMax;
    }
    public void avanzar()
    {
        //transform.position = new Vector3(spriteRenderer.flipX ? transform.position.x - 1.5f : transform.position.x + 1.5f, transform.position.y, transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(spriteRenderer.flipX ? transform.position.x - 20 : transform.position.x + 20, transform.position.y, transform.position.z),30*Time.deltaTime);
        GetComponent<Rigidbody2D>().velocity = transform.rotation.y != 0 ? Vector2.left * 10f : Vector2.right * 10f;
        //GetComponent<Rigidbody2D>().AddForce(spriteRenderer.flipX ? Vector2.left * 25f : Vector2.right * 25f, ForceMode2D.Impulse);
    }
    public void desactivarTriggers()
    {
        at1.SetActive(false);
        at2.SetActive(false);
        at3.SetActive(false);
    }
    public void activarat1()
    {
        at1.SetActive(true);
    }
    public void activarat2()
    {
        at2.SetActive(true);
    }
    public void activarat3()
    {
        at3.SetActive(true);
    }
    public void terminarAtaque()
    {
        animator.SetInteger("state", 1);
    }
    public void pararAvance()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    public void instanciarProyectil()
    {
        Vector3 targ = player.transform.position;
        targ.z = 0f;
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;

        GameObject obj = Instantiate(proyectil, puntoFireBall.transform.position, Quaternion.Euler(new Vector3(0, 0, angle + 90f)));
        obj.GetComponent<FireballBossController>().destino = player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("player"))
        {
            if (collision.transform.GetComponent<HeroController>().vulnerable)
            {
                //collision.transform.GetComponent<HeroController>().vulnerable = false;
                collision.transform.GetComponent<HeroController>().hit();
                //collision.transform.GetComponent<HeroController>().bv.bajarVida();
            }
        }
    }
}
