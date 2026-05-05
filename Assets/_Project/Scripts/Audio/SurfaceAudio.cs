using UnityEngine;

public class SurfaceAudio : MonoBehaviour // Stores footstep sounds for one surface type.
{
    [SerializeField] private AudioClip[] walkLeftClips; // Walking sounds for the left foot.

    [SerializeField] private AudioClip[] walkRightClips; // Walking sounds for the right foot.

    [SerializeField] private AudioClip[] runLeftClips; // Running sounds for the left foot.

    [SerializeField] private AudioClip[] runRightClips; // Running sounds for the right foot.

    [SerializeField] private AudioClip jumpClip; // Sound played when the player jumps from this surface.

    [SerializeField] private AudioClip landClip; // Sound played when the player lands on this surface.

    public AudioClip GetStepClip(bool isRunning, bool isLeftFoot) // Returns the correct footstep sound.
    {
        if (isRunning) // Checks if the player is running.
        {
            return isLeftFoot ? GetRandomClip(runLeftClips, walkLeftClips) : GetRandomClip(runRightClips, walkRightClips); // Uses run clips, or walk clips as fallback.
        }

        return isLeftFoot ? GetRandomClip(walkLeftClips, null) : GetRandomClip(walkRightClips, null); // Uses walking clips.
    }

    public AudioClip GetJumpClip() // Returns the jump sound.
    {
        return jumpClip; // Gives the jump sound.
    }

    public AudioClip GetLandClip() // Returns the landing sound.
    {
        return landClip; // Gives the landing sound.
    }

    private AudioClip GetRandomClip(AudioClip[] mainClips, AudioClip[] fallbackClips) // Picks a random sound from an array.
    {
        if (mainClips != null && mainClips.Length > 0) // Checks if the main clip array has sounds.
        {
            int index = Random.Range(0, mainClips.Length); // Picks a random index.

            return mainClips[index]; // Returns the selected sound.
        }

        if (fallbackClips != null && fallbackClips.Length > 0) // Checks if fallback clips exist.
        {
            int index = Random.Range(0, fallbackClips.Length); // Picks a random fallback index.

            return fallbackClips[index]; // Returns the selected fallback sound.
        }

        return null; // Returns nothing if no clips exist.
    }
}