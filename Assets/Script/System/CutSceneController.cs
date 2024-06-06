using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutSceneController : MonoBehaviour
{
    public GameObject Ending;
    public RawImage[] cutScenes; // Array yang berisi CutScene0 sampai CutScene9
    public GameObject[] stories; // Array yang berisi Story0 sampai Story13

    private int currentCutSceneIndex = 0;
    public float dissolveDuration = 1.5f;
    public float displayDuration = 2f;
    public float textFadeDuration = 0.5f;

    void Start()
    {
        // Inisialisasi semua story ke inactive dan alpha 0
        foreach (GameObject story in stories)
        {
            story.SetActive(true);
            CanvasGroup canvasGroup = story.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = story.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
            story.SetActive(false);
        }
        StartCoroutine(PlayCutScenes());
        
    }

    void Update()
    {
        SFXTipis();
    }

        IEnumerator PlayCutScenes()
    {

        // Cutscene 0
        yield return StartCoroutine(PlayCutScene(0, 0, null));

        // Cutscene 1
        yield return StartCoroutine(PlayCutScene(1, 1, null));

        // Cutscene 2
        yield return StartCoroutine(PlayCutScene(2, 2, null));

        // Cutscene 3
        yield return StartCoroutine(PlayCutScene(3, 3, null));

        // Cutscene 4
        yield return StartCoroutine(PlayCutScene(4, 4, null));

        // Story 6 (Appears with Cutscene 5)
        yield return StartCoroutine(PlayStoryWithCutscene(5, 6));

        // Cutscene 5
        yield return StartCoroutine(PlayCutScene(5, null, null));

        // Story 7 (Appears without cutscene, after Story 6)
        yield return StartCoroutine(PlayStory(7));

        // Cutscene 6
        yield return StartCoroutine(PlayCutScene(6, 8, null));

        // Cutscene 7
        yield return StartCoroutine(PlayCutScene(7, 9, null));

        // Cutscene 8
        yield return StartCoroutine(PlayCutScene(8, 11, null));

        // Cutscene 9
        yield return StartCoroutine(PlayCutScene(9, 12, null));

        // Story 10 (Appears without cutscene, after Story 6)
        yield return StartCoroutine(PlayStory(13));
        AudioManager.Instance.StopMusic("BGM Prolog");
        Ending.SetActive(true);
    }

    IEnumerator PlayCutScene(int? index, int? storyIndexToShow, int? storyIndexToHide)
    {
        if (index != null)
        {
            // Aktifkan cutscene saat ini
            RawImage currentCutScene = cutScenes[(int)index];
            currentCutScene.gameObject.SetActive(true);

            // Animasi dissolve dari 1 ke 0 (muncul)
            yield return StartCoroutine(Dissolve(currentCutScene, 1f, 0f, dissolveDuration));

            // Aktifkan dan fade-in teks saat cutscene terlihat
            if (storyIndexToShow != null)
            {
                yield return StartCoroutine(PlayStory((int)storyIndexToShow));
            }

            // Animasi dissolve dari 0 ke 1 (menghilang)
            yield return StartCoroutine(Dissolve(currentCutScene, 0f, 1f, dissolveDuration));

            // Nonaktifkan cutscene saat ini
            currentCutScene.gameObject.SetActive(false);

            // Fade-out teks saat ini (jika storyIndexToHide tidak null)
            if (storyIndexToHide != null)
            {
                yield return StartCoroutine(PlayStory((int)storyIndexToHide));
            }
        }
    }

    IEnumerator PlayStoryWithCutscene(int? cutsceneIndex, int? storyIndex)
    {
        if (cutsceneIndex != null)
        {
            // Tunggu hingga cutscene terkait telah terdisplay (aktif dan dissolve = 0)
            RawImage relatedCutScene = cutScenes[(int)cutsceneIndex];
            while (relatedCutScene.gameObject.activeSelf && relatedCutScene.material.GetFloat("_DissolveAmount") > 0f)
            {
                yield return null;
            }

            // Play Story
            if (storyIndex != null)
            {
                yield return StartCoroutine(PlayStory((int)storyIndex));
            }
        }
    }

    IEnumerator PlayStory(int? index)
    {
        if (index != null && (int)index < stories.Length)
        {
            GameObject story = stories[(int)index];
            if (!story.activeSelf) // Cek apakah story sudah aktif
            {
                story.SetActive(true);
                yield return StartCoroutine(FadeText(story, 0f, 1f, textFadeDuration));

                // Tunggu selama displayDuration
                yield return new WaitForSeconds(displayDuration);

                yield return StartCoroutine(FadeText(story, 1f, 0f, textFadeDuration));
                story.SetActive(false);
            }
        }
    }

    IEnumerator Dissolve(RawImage cutScene, float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float dissolveAmount = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            cutScene.material.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
        }
        cutScene.material.SetFloat("_DissolveAmount", endValue); // Pastikan set ke endValue
    }

    IEnumerator FadeText(GameObject story, float startAlpha, float endAlpha, float duration)
    {
        CanvasGroup canvasGroup = story.GetComponent<CanvasGroup>();
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha; // Pastikan set ke endAlpha
    }

    public void SFXTipis()
    {
        if (currentCutSceneIndex == 5)
        {
            AudioManager.Instance.PlaySFX("Berdetak");
        }
        else
        {
            AudioManager.Instance.StopSFX("Berdetak");
        }
    }
}
