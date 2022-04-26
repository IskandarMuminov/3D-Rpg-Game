using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SFX
{    public class AudioEffectRandomPlayer : MonoBehaviour 
    {
        AudioSource playingSource;
        [SerializeField] AudioClip[] sfxs;

        private void Start() 
        {
            playingSource = GetComponent<AudioSource>();
        }


        public void PlaySFX()
        {
            AudioClip clip = sfxs[UnityEngine.Random.Range(0, sfxs.Length)];
            playingSource.pitch= UnityEngine.Random.Range(0.9f, 1.05f);
            playingSource.PlayOneShot(clip);
        }
    }
}
