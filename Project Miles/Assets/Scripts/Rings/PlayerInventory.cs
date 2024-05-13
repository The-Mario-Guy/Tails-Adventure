using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
   

    public int NumberOfDiamonds { get; private set; }

    public UnityEvent<PlayerInventory> OnDiamondCollected;
    public UnityEvent<PlayerInventory> OnDiamondLost;


    public void DiamondCollected()
    {
        NumberOfDiamonds++;
        OnDiamondCollected.Invoke(this);
    }

    public void DiamondLost()
    {
        //PlayerController playerController = gameObject.GetComponent<PlayerController>();
        //if (playerController.isHurt == true)
        {
            NumberOfDiamonds = 0;
            OnDiamondLost.Invoke(this);
        }
        
    }

    public static implicit operator PlayerInventory(float v)
    {
        throw new NotImplementedException();
    }
}
