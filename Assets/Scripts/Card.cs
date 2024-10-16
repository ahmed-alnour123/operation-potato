using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    RectTransform rectTransform;
    Image image;
    Button button;

    public bool isShowingFace = false;
    public float cardFlipTime = 0.3f;
    public char data;

    public bool canFlip = true;
    bool didChangSprite = false;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(OnCardClicked);
    }

    public void Flip(float delay = 0) {
        StartCoroutine(FlipRoutine(delay));
    }

    private IEnumerator FlipRoutine(float delay) {
        canFlip = false;

        if (delay > 0) {
            yield return new WaitForSeconds(delay);
        }

        isShowingFace = !isShowingFace;
        float time = cardFlipTime;
        didChangSprite = false;

        while (time > 0) {
            float t = 1 - (time / cardFlipTime);
            rectTransform.rotation = Quaternion.Euler(0, t * 180, 0);

            if (t > 0.5f && !didChangSprite) {
                didChangSprite = true;
                image.color = isShowingFace ? Color.red : Color.white;
            }

            yield return null;
            time -= Time.deltaTime;
        }

        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        canFlip = true;
    }

    private void OnCardClicked() {
        if (!canFlip) {
            return;
        }

        Flip();
        GameManager.Instance.OnCardClicked(this, isShowingFace);
    }

    public void DestroyCard() {
        // TODO: Play VFX
        Destroy(gameObject, 1f);
    }
}
