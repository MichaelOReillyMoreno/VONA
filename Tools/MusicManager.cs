using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enumerado de FadeIn, FadeOut
/// </summary>
public enum FadeType { IN, OUT }

/// <summary>
/// Clase que controla la musica del escenario
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    static MusicManager instance;

    //Instancia para el singleton
    public static MusicManager Instance
    {
        get
        {
            return instance;
        }
    }

    //Atributo propio
    private AudioSource myAudio;

    //Lista de canciones 
    public List<AudioClip> mySongs;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this.gameObject);

        this.myAudio = this.GetComponent<AudioSource>();
        this.SwitchSong(SceneManager.GetActiveScene().name);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.myAudio = this.GetComponent<AudioSource>();
        this.SwitchSong(scene.name);
    }

    private void SwitchSong(string level)
    {

    }

    /// <summary>
    /// Metodo que realiza el FadeSound de la musica
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="fadetype"></param>
    /// <param name="maxVolume"></param>
    /// <returns></returns>
    public IEnumerator FadeSound(float timer, FadeType fadetype, float maxVolume)
    {
        float start = (fadetype == FadeType.IN) ? 0.0f : maxVolume;
        float end = (fadetype == FadeType.IN) ? maxVolume : 0.0f;

        float i = 0.0f;
        float step = 1.0f / timer;

        while (i <= 1.0f)
        {
            i += step * Time.deltaTime;
            this.myAudio.volume = Mathf.Lerp(start, end, i);

            yield return null;
        }
    }

    /// <summary>
    /// Metodo que cambia y reproduce un sonido
    /// </summary>
    /// <param name="mySound"></param>
    private void ChangeAndPlaySound(AudioClip mySound)
    {
        this.myAudio.clip = mySound;
        this.myAudio.enabled = true;
        this.myAudio.Play();
        this.myAudio.volume = 0;
    }
}