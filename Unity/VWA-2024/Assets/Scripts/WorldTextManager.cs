using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class WorldTextManager : MonoBehaviour
{
    public TextMeshPro damageTextPrefab;

    public void ShowDamage(Vector3 position, int damageAmount)
    {
        Debug.Log(Quaternion.identity);
        TextMeshPro damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        damageText.text = damageAmount.ToString();
        StartCoroutine(FadeOut(damageText));
    }

    private IEnumerator FadeOut(TextMeshPro text)
    {
        float duration = 0.5f; //Fade out over 2 seconds.
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(text.gameObject);
        yield break;
    }
}
