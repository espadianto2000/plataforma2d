using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barraPower : MonoBehaviour
{
    public Image sl;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void hitEnemigo()
    {
        sl.fillAmount += sl.fillAmount>=1 ? 0 : 0.25f;
    }
}
