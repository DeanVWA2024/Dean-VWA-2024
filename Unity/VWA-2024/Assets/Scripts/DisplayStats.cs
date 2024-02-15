using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    // make slider accessable from editor without making it public
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        slider.value = (float) currentHealth / (float) maxHealth;
    }

    public TextMeshPro damageTextPrefab;

    public void ShowDamage(Vector3 position, int damageAmount)
    {
        Debug.Log(Quaternion.identity);
        TextMeshPro damageText = Instantiate(damageTextPrefab, this.transform.position, Quaternion.identity);
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
