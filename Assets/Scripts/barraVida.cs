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
    
    public void bajarVida()
    {
        sl.fillAmount -= 0.21f;
        if(sl.fillAmount <= 0)
        {
            mDead?.Invoke(this, EventArgs.Empty);
        }
    }
    public void subirVida()
    {
        sl.fillAmount += 0.2f;
    }
    public void Die()
    {
        sl.fillAmount = 0f;
        mDead?.Invoke(this, EventArgs.Empty);
    }
    public void AddDeadDelegate(EventHandler eventHandler)
    {
        mDead += eventHandler;
    }
}
