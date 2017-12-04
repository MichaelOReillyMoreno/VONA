using UnityEngine;
using System.Collections;

/// <summary>
/// Clase que controla la musica juego
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AmbientMusicManager : MonoBehaviour
{
    static AmbientMusicManager instance;

    public static AmbientMusicManager Instance
    {
        get
        {
            return instance;
        }
    }

    //Atributo propio
    private AudioSource myAudio;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        this.myAudio = this.GetComponent<AudioSource>();
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
    public void ChangeAndPlaySound(AudioClip mySound)
    {
        this.myAudio.clip = mySound;
        this.myAudio.enabled = true;
        this.myAudio.Play();
        this.myAudio.volume = 0;
    }
}