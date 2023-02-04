using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Receives animation events from a Unity AnimationController and notifies
// some observers
public class AnimationEventsHandler : MonoBehaviour
{
    public delegate void AnimationClipEnd(string animationClipName);
    private event AnimationClipEnd callbacks;

    public void AddAnimationClipEndCallback(AnimationClipEnd callback)
    {
        callbacks += callback;
    }

    public void AnimationClipEntEvent(string animationClipName)
    {
        callbacks?.Invoke(animationClipName);
    }
}
