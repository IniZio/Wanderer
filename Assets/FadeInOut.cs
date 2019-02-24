using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    public CanvasGroup uiElement;

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1));
    }
    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0));
    }
    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerptime = 0.5f)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerptime;

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerptime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
        print("done");
    }
}
