using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rings : MonoBehaviour
{
    public AudioSource ringSFX;

    public void Start()
    {
        ringSFX = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ringSFX.Play(1);
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.DiamondCollected();
            gameObject.SetActive(false);
        }
    }
}

