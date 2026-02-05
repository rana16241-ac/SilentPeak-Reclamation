using UnityEngine;

namespace SilentPeak.Core
{
    /// <summary>
    /// Manages all audio in the game including music and sound effects
    /// Singleton pattern for global access
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        public AudioSource musicSource;
        public AudioSource sfxSource;
        public AudioSource ambienceSource;

        [Header("Sound Effects")]
        public AudioClip sniperShot;
        public AudioClip enemyHit;
        public AudioClip enemyHeadshot;
        public AudioClip alarmSiren;
        public AudioClip buttonClick;
        public AudioClip missionComplete;
        public AudioClip missionFailed;
        public AudioClip scopeZoom;
        public AudioClip breathHold;
        public AudioClip reload;
        public AudioClip coinCollect;
        public AudioClip purchaseSuccess;
        public AudioClip purchaseFailed;

        [Header("Music")]
        public AudioClip menuMusic;
        public AudioClip gameplayMusic;
        public AudioClip victoryMusic;

        [Header("Ambience")]
        public AudioClip windAmbience;
        public AudioClip baseAmbience;

        [Header("Volume Settings")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 0.7f;
        [Range(0f, 1f)] public float sfxVolume = 1f;
        [Range(0f, 1f)] public float ambienceVolume = 0.5f;

        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
                LoadVolumeSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initialize audio sources if they don't exist
        /// </summary>
        private void InitializeAudioSources()
        {
            if (musicSource == null)
            {
                GameObject musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                musicSource = musicObj.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }

            if (sfxSource == null)
            {
                GameObject sfxObj = new GameObject("SFXSource");
                sfxObj.transform.SetParent(transform);
                sfxSource = sfxObj.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }

            if (ambienceSource == null)
            {
                GameObject ambienceObj = new GameObject("AmbienceSource");
                ambienceObj.transform.SetParent(transform);
                ambienceSource = ambienceObj.AddComponent<AudioSource>();
                ambienceSource.loop = true;
                ambienceSource.playOnAwake = false;
            }
        }

        /// <summary>
        /// Load volume settings from PlayerPrefs
        /// </summary>
        private void LoadVolumeSettings()
        {
            masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
            sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
            
            ApplyVolumeSettings();
        }

        /// <summary>
        /// Apply volume settings to audio sources
        /// </summary>
        private void ApplyVolumeSettings()
        {
            if (musicSource != null)
                musicSource.volume = musicVolume * masterVolume;
            
            if (sfxSource != null)
                sfxSource.volume = sfxVolume * masterVolume;
            
            if (ambienceSource != null)
                ambienceSource.volume = ambienceVolume * masterVolume;
        }

        /// <summary>
        /// Play a sound effect
        /// </summary>
        public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
        {
            if (clip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(clip, volumeMultiplier);
            }
        }

        /// <summary>
        /// Play music track
        /// </summary>
        public void PlayMusic(AudioClip clip, bool fadeIn = false)
        {
            if (clip == null || musicSource == null) return;

            if (musicSource.clip != clip)
            {
                if (fadeIn)
                {
                    StartCoroutine(FadeMusic(clip));
                }
                else
                {
                    musicSource.clip = clip;
                    musicSource.Play();
                }
            }
        }

        /// <summary>
        /// Play ambience sound
        /// </summary>
        public void PlayAmbience(AudioClip clip)
        {
            if (clip != null && ambienceSource != null)
            {
                ambienceSource.clip = clip;
                ambienceSource.Play();
            }
        }

        /// <summary>
        /// Stop ambience
        /// </summary>
        public void StopAmbience()
        {
            if (ambienceSource != null)
            {
                ambienceSource.Stop();
            }
        }

        /// <summary>
        /// Fade music transition
        /// </summary>
        private System.Collections.IEnumerator FadeMusic(AudioClip newClip)
        {
            float fadeDuration = 1f;
            float startVolume = musicSource.volume;

            // Fade out
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            // Change clip
            musicSource.clip = newClip;
            musicSource.Play();

            // Fade in
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
                yield return null;
            }

            musicSource.volume = startVolume;
        }

        /// <summary>
        /// Play alarm siren
        /// </summary>
        public void PlayAlarmSiren()
        {
            PlaySFX(alarmSiren, 1.2f);
        }

        /// <summary>
        /// Play button click sound
        /// </summary>
        public void PlayButtonClick()
        {
            PlaySFX(buttonClick, 0.8f);
        }

        /// <summary>
        /// Play sniper shot sound
        /// </summary>
        public void PlaySniperShot()
        {
            PlaySFX(sniperShot, 1f);
        }

        /// <summary>
        /// Play enemy hit sound
        /// </summary>
        public void PlayEnemyHit(bool isHeadshot)
        {
            if (isHeadshot && enemyHeadshot != null)
            {
                PlaySFX(enemyHeadshot, 1f);
            }
            else
            {
                PlaySFX(enemyHit, 1f);
            }
        }

        /// <summary>
        /// Set master volume
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterVolume);
            ApplyVolumeSettings();
        }

        /// <summary>
        /// Set music volume
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, musicVolume);
            ApplyVolumeSettings();
        }

        /// <summary>
        /// Set SFX volume
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
            ApplyVolumeSettings();
        }

        /// <summary>
        /// Mute/unmute all audio
        /// </summary>
        public void ToggleMute()
        {
            AudioListener.pause = !AudioListener.pause;
        }

        /// <summary>
        /// Stop all audio
        /// </summary>
        public void StopAllAudio()
        {
            if (musicSource != null) musicSource.Stop();
            if (sfxSource != null) sfxSource.Stop();
            if (ambienceSource != null) ambienceSource.Stop();
        }
    }
}
