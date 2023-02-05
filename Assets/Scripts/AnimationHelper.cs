using System.Collections;
using System.Collections.Generic;
using Player.Capabilities;
using UnityEngine;
using UnityEngine.Events;

public class AnimationHelper : MonoBehaviour
{
    public UnityEvent playFootsteps;
    public Jump j;

    public void PlayFootsteps()
    {
        if(j.onGround)
            playFootsteps ?.Invoke();
    }
}
