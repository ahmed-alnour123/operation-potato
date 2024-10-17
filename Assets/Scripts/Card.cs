using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {
    public RectTransform cardView;
    public Image backImage;
    public TMP_Text cardText;
    Button button;

    public bool isShowingFace = false;
    public float cardFlipTime = 0.3f;
    public char letter;
    public Sprite backImageSprite;

    public bool canFlip = true;
    bool didChangSprite = false;

    private void Awake() {
        button = cardView.GetComponent<Button>();
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
        cardText.rectTransform.rotation = Quaternion.Euler(0, isShowingFace ? 0 : 180, 0);
        SoundManager.Instance.PlaySound(SoundEffect.CardFlip);

        while (time > 0) {
            float t = (time / cardFlipTime);
            cardView.rotation = Quaternion.Euler(0, t * 180, 0);

            if (t < 0.5f && !didChangSprite) {
                didChangSprite = true;
                backImage.sprite = isShowingFace ? null : backImageSprite;
                cardText.enabled = isShowingFace;
            }

            yield return null;
            time -= Time.deltaTime;
        }

        cardView.rotation = Quaternion.Euler(0, 0, 0);

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
        Invoke(nameof(StopAllCoroutines), 1);
        Destroy(cardView.gameObject, 1f);
    }
}
