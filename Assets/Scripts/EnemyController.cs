
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;

    private float mHealth;

    private void Start()
    {
        mHealth = maxHealth;
    }

    public void bajarVida()
    {
        mHealth -= maxHealth * 0.25f;
        healthbar.value = mHealth/maxHealth;


        if (mHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
