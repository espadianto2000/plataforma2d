using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class barraVida : MonoBehaviour
{
    public Image sl;
    private event EventHandler mDead;
    private bool muerto = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!muerto && sl.fillAmount <= 0)
        {
            muerto = true;
            mDead?.Invoke(this, EventArgs.Empty);
        }
    }
    public void bajarVida()
    {
        sl.fillAmount -= 0.2f;
    }
    public void subirVida()
    {
        sl.fillAmount += 0.2f;
    }
    public void AddDeadDelegate(EventHandler eventHandler)
    {
        mDead += eventHandler;
    }
}
