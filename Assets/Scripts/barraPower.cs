using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barraPower : MonoBehaviour
{
    public Image sl;
    public Image poder;

    private void Update()
    {
        Color color = sl.fillAmount>=1 ? new Color(255f,255f,0f) : Color.white;
        poder.color = color;
    }
    public void hitEnemigo()
    {
        sl.fillAmount += sl.fillAmount>=1 ? 0 : 0.143f;
    }
}
