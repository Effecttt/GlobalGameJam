using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Player.Capabilities;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] private EventReference footsteps;

    private void OnEnable()
    {
        Jump.landSound += PlayFootstep;
    }

    private void OnDisable()
    {
        Jump.landSound -= PlayFootstep;
    }

    public void PlayFootstep()
    {
        RuntimeManager.PlayOneShot(footsteps);
    }
}
