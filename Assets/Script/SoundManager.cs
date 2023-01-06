using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip centerHurtSound, enemyDeathSound, explosionSound, healSound, shockwaveSound, buttonSound;
    static AudioSource audioSource;

    void Start()
    {
        centerHurtSound = Resources.Load<AudioClip>("centerHurt");
        enemyDeathSound = Resources.Load<AudioClip>("enemyDeath");
        explosionSound = Resources.Load<AudioClip>("explosion");
        healSound = Resources.Load<AudioClip>("heal");
        shockwaveSound = Resources.Load<AudioClip>("shockwave");
        buttonSound = Resources.Load<AudioClip>("button");

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "centerHurt":
                audioSource.PlayOneShot(centerHurtSound);
                break;

            case "enemyDeath":
                audioSource.PlayOneShot(enemyDeathSound);
                break;

            case "explosion":
                audioSource.PlayOneShot(explosionSound);
                break;
            
            case "heal":
                audioSource.PlayOneShot(healSound);
                break;
                
            case "shockwave":
                audioSource.PlayOneShot(shockwaveSound);
                break;
            
            case "button":
                audioSource.PlayOneShot(buttonSound);
                break;
        }
    }
}
