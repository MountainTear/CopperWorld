using Spine.Unity;
using Spine;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationHandle : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public List<StateNameToAnimationReference> statesAndAnimations = new List<StateNameToAnimationReference>();

    [System.Serializable]
    public class StateNameToAnimationReference
    {
        public string stateName;
        public AnimationReferenceAsset animation;
    }

    //readonly Dictionary<Spine.AnimationStateData.AnimationPair, Spine.Animation> transitionDictionary = new Dictionary<AnimationStateData.AnimationPair, Animation>(Spine.AnimationStateData.AnimationPairComparer.Instance);

    public Spine.Animation TargetAnimation { get; private set; }

    void Awake()
    {
        // Initialize AnimationReferenceAssets
        foreach (StateNameToAnimationReference entry in statesAndAnimations)
        {
            entry.animation.Initialize();
        }
    }

    /// <summary>Sets the horizontal flip state of the skeleton based on a nonzero float. If negative, the skeleton is flipped. If positive, the skeleton is not flipped.</summary>
    public void SetFlip(float horizontal)
    {
        if (horizontal != 0)
        {
            skeletonAnimation.Skeleton.ScaleX = horizontal > 0 ? 1f : -1f;
        }
    }

    /// <summary>Plays an animation based on the state name.</summary>
    public void PlayAnimationForState(string stateShortName, int layerIndex)
    {
        PlayAnimationForState(StringToHash(stateShortName), layerIndex);
    }

    /// <summary>Plays an animation based on the hash of the state name.</summary>
    public void PlayAnimationForState(int shortNameHash, int layerIndex)
    {
        Spine.Animation foundAnimation = GetAnimationForState(shortNameHash);
        if (foundAnimation == null)
            return;

        PlayNewAnimation(foundAnimation, layerIndex);
    }

    /// <summary>Gets a Spine Animation based on the state name.</summary>
    public Spine.Animation GetAnimationForState(string stateShortName)
    {
        return GetAnimationForState(StringToHash(stateShortName));
    }

    /// <summary>Gets a Spine Animation based on the hash of the state name.</summary>
    public Spine.Animation GetAnimationForState(int shortNameHash)
    {
        StateNameToAnimationReference foundState = statesAndAnimations.Find(entry => StringToHash(entry.stateName) == shortNameHash);
        return (foundState == null) ? null : foundState.animation;
    }

    /// <summary>Play an animation. If a transition animation is defined, the transition is played before the target animation being passed.</summary>
    public void PlayNewAnimation(Spine.Animation target, int layerIndex)
    {
        Spine.Animation current = null;
        current = GetCurrentAnimation(layerIndex);
        skeletonAnimation.AnimationState.SetAnimation(layerIndex, target, true);
        this.TargetAnimation = target;
    }

    /// <summary>Play a non-looping animation once then continue playing the state animation.</summary>
    public void PlayOneShot(Spine.Animation oneShot, int layerIndex)
    {
        Spine.AnimationState state = skeletonAnimation.AnimationState;
        state.SetAnimation(0, oneShot, false);
        state.AddAnimation(0, this.TargetAnimation, true, 0f);
    }

    Spine.Animation GetCurrentAnimation(int layerIndex)
    {
        TrackEntry currentTrackEntry = skeletonAnimation.AnimationState.GetCurrent(layerIndex);
        return (currentTrackEntry != null) ? currentTrackEntry.Animation : null;
    }

    int StringToHash(string s)
    {
        return Animator.StringToHash(s);
    }
}
