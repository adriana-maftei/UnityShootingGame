using System.Linq;
using UnityEngine;

public class AudioMNG : MonoBehaviour
{
    [Header("Player sounds")]
    [SerializeField] PlayerMovement character;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] PlayerJump jump;
    [SerializeField] PlayerCrouch crouch;
    [SerializeField] AudioSource walkAudio, runAudio, jumpAudio, landAudio, crouchStartAudio, crouchedAudio, crouchEndAudio;
    [SerializeField] AudioClip[] jumpSFX, landSFX, crouchStartSFX, crouchEndSFX;
    float velocityThreshold = .01f;
    Vector2 lastCharacterPosition;
    Vector2 CurrentCharacterPosition => new Vector2(character.transform.position.x, character.transform.position.z);
    AudioSource[] MovingAudios => new AudioSource[] { walkAudio, runAudio, crouchedAudio };

    void Reset()
    {
        character = GetComponentInParent<PlayerMovement>();
        groundCheck = (transform.parent ?? transform).GetComponentInChildren<GroundCheck>();
        walkAudio = GetOrCreateAudioSource("walkAudio");
        runAudio = GetOrCreateAudioSource("runAudio");
        landAudio = GetOrCreateAudioSource("landAudio");

        jump = GetComponentInParent<PlayerJump>();
        if (jump) jumpAudio = GetOrCreateAudioSource("jumpAudio");

        crouch = GetComponentInParent<PlayerCrouch>();
        if (crouch)
        {
            crouchStartAudio = GetOrCreateAudioSource("Crouch Start Audio");
            crouchStartAudio = GetOrCreateAudioSource("Crouched Audio");
            crouchStartAudio = GetOrCreateAudioSource("Crouch End Audio");
        }
    }

    void OnEnable() => SubscribeToEvents();
    void OnDisable() => UnsubscribeToEvents();

    void FixedUpdate()
    {
        float velocity = Vector3.Distance(CurrentCharacterPosition, lastCharacterPosition);
        if (velocity >= velocityThreshold && groundCheck && groundCheck.isGrounded)
        {
            if (crouch && crouch.IsCrouched) SetPlayingMovingAudio(crouchedAudio);
            else if (character.IsRunning) SetPlayingMovingAudio(runAudio);
            else SetPlayingMovingAudio(walkAudio);
        }
        else
        {
            SetPlayingMovingAudio(null);
        }
        lastCharacterPosition = CurrentCharacterPosition;
    }

    void SetPlayingMovingAudio(AudioSource audioToPlay)
    {
        foreach (var audio in MovingAudios.Where(audio => audio != audioToPlay && audio != null))
        {
            audio.Pause();
        }
        if (audioToPlay && !audioToPlay.isPlaying) audioToPlay.Play();
    }

    #region Play instant-related audios.
    void PlayLandingAudio() => PlayRandomClip(landAudio, landSFX);
    void PlayJumpAudio() => PlayRandomClip(jumpAudio, jumpSFX);
    void PlayCrouchStartAudio() => PlayRandomClip(crouchStartAudio, crouchStartSFX);
    void PlayCrouchEndAudio() => PlayRandomClip(crouchEndAudio, crouchEndSFX);
    #endregion

    #region Subscribe/unsubscribe to events.
    void SubscribeToEvents() //enable related sounds
    {
        groundCheck.Grounded += PlayLandingAudio;
        if (jump) jump.Jumped += PlayJumpAudio; 
        if (crouch)
        {
            crouch.CrouchStart += PlayCrouchStartAudio;
            crouch.CrouchEnd += PlayCrouchEndAudio;
        }
    }

    void UnsubscribeToEvents() //disable play related sounds
    {
        groundCheck.Grounded -= PlayLandingAudio;
        if (jump) jump.Jumped -= PlayJumpAudio;
        if (crouch)
        {
            crouch.CrouchStart -= PlayCrouchStartAudio;
            crouch.CrouchEnd -= PlayCrouchEndAudio;
        }
    }
    #endregion

    #region Utility.
    AudioSource GetOrCreateAudioSource(string name)
    {
        // Try to get the audiosource.
        AudioSource result = System.Array.Find(GetComponentsInChildren<AudioSource>(), a => a.name == name);
        if (result) return result;

        // if audiosource does not exist, create it.
        result = new GameObject(name).AddComponent<AudioSource>();
        result.spatialBlend = 1;
        result.playOnAwake = false;
        result.transform.SetParent(transform, false);
        return result;
    }

    static void PlayRandomClip(AudioSource audio, AudioClip[] clips)
    {
        if (!audio || clips.Length <= 0) return;

        // Get a random clip. If possible, make sure that it's not the same as the clip that is already on the audiosource.
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clips.Length > 1)
            while (clip == audio.clip)
                clip = clips[Random.Range(0, clips.Length)];

        audio.clip = clip;
        audio.Play();
    }
    #endregion 
}
