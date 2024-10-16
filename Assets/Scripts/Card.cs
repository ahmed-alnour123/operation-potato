using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    RectTransform rectTransform;
    Image image;
    Button button;

    public bool showingFace = true;
    public float cardFlipTime = 0.3f;

    bool isFlipping = false;
    bool changedSprite = false;


    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(OnCardClicked);
    }

    IEnumerator Flip() {
        isFlipping = true;

        float time = cardFlipTime;
        changedSprite = false;

        while (time > 0) {
            float t = 1 - (time / cardFlipTime);
            rectTransform.rotation = Quaternion.Euler(0, t * 180, 0);

            if (t > 0.5f && !changedSprite) {
                changedSprite = true;
                image.color = showingFace ? Color.red : Color.white;
            }

            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
        }

        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        isFlipping = false;
    }

    private void OnCardClicked() {
        if (isFlipping) {
            return;
        }

        showingFace = !showingFace;
        StartCoroutine(Flip());
    }
}
