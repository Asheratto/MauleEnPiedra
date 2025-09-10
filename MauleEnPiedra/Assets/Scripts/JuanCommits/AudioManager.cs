using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private SoundEntry[] sounds;

    private Dictionary<SoundID, AudioClip> _clipDict;
    private Coroutine _sfxCoroutine;

    [Serializable]
    private struct SoundEntry
    {
        public SoundID id;
        public AudioClip clip;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void BuildDictionary()
    {
        _clipDict = new Dictionary<SoundID, AudioClip>();
        foreach (var e in sounds)
            if (e.clip != null && !_clipDict.ContainsKey(e.id))
                _clipDict.Add(e.id, e.clip);
    }

    public void PlaySFX(SoundID id, float volume = 1f)
    {
        if (_clipDict.TryGetValue(id, out var clip))
            sfxSource.PlayOneShot(clip, volume);
        else
            Debug.LogWarning($"[AudioManager] SFX no encontrado: {id}");
    }

    public void PlaySFXSegment(SoundID id, float startTime, float duration, float volume = 1f)
    {
        if (!_clipDict.TryGetValue(id, out var clip))
        {
            Debug.LogWarning($"[AudioManager] SFX no encontrado: {id}");
            return;
        }

        if (_sfxCoroutine != null)
            StopCoroutine(_sfxCoroutine);

        _sfxCoroutine = StartCoroutine(PlaySegmentCoroutine(clip, startTime, duration, volume));
    }

    private IEnumerator PlaySegmentCoroutine(AudioClip clip, float startTime, float duration, float volume)
    {
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.time = Mathf.Clamp(startTime, 0, clip.length);
        sfxSource.Play();
        yield return new WaitForSeconds(duration);
        sfxSource.Stop();
        _sfxCoroutine = null;
    }

    public void StopSFX()
    {
        if (_sfxCoroutine != null)
            StopCoroutine(_sfxCoroutine);
        sfxSource.Stop();
        _sfxCoroutine = null;
    }

    public void PlayMusic(SoundID id, bool loop = true)
    {
        if (_clipDict.TryGetValue(id, out var clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
            Debug.LogWarning($"[AudioManager] Música no encontrada: {id}");
    }

    public void StopMusic() => musicSource.Stop();
}
//Para poner los ultimos 2s de tirar carta: var clip = AudioManager.Instance.GetClip(SoundID.TirarCarta);
//float start = clip.length - 2f;
//AudioManager.Instance.PlaySFXSegment(SoundID.TirarCarta, start, 2f);
//Para cuando se repartan, que el sonido se corte: AudioManager.Instance.PlaySFX(SoundID.DarCartas); Al empezar la animacion y 
//AudioManager.Instance.StopSFX(); al terminar
//Para la musica de fondo: AudioManager.Instance.PlayMusic(SoundID.FondoMusicaFondo, loop: true);