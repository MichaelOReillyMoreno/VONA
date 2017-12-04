using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    static FadeInOut instance;

    public static FadeInOut Instance
    {
        get
        {
            return instance;
        }
    }

    public bool IsFading { get; set; }

    [SerializeField]
    private Image faderImage;

    private Color currentColor;
    private Color visibleColor;
    private float counter;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(this);

        IsFading = false;
    }

    public IEnumerator FadeInOutCo(int alpha, float waitTime)
    {
        yield return StartCoroutine(FadeInOut.Instance.CrossFadeAlpha(1, 0.5f));
        yield return new WaitForSeconds(waitTime);

        yield return StartCoroutine(FadeInOut.Instance.CrossFadeAlpha(0, 0.5f));
    }

    public IEnumerator ResetDay()
    {
        int nextDay = PlayerPrefs.GetInt("Day") + 1;
        MenuCanvas.Instance.SetDay(nextDay);

        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(FadeInOut.Instance.CrossFadeAlpha(1, 0.5f));

        SceneManager.LoadScene("Gameplay");
    }

    public IEnumerator CrossFadeAlpha(float alpha, float duration)
    {
        faderImage.enabled = true;
        faderImage.raycastTarget = (alpha == 0) ? false : true;

        currentColor = faderImage.color;
        visibleColor = faderImage.color;
        visibleColor.a = alpha;

        counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            faderImage.color = Color.Lerp(currentColor, visibleColor, counter / duration);
            yield return null;
        }

        IsFading = !IsFading;

    }
}