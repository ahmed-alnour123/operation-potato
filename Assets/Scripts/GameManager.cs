using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    // prefabs and game setup
    public Card cardPrefab;
    public List<Sprite> sprites;

    // components
    public GridLayoutGroup grid;

    // game parameters
    public float flipDelayTime = 2f;
    public int rows = 3;
    public int columns = 3;

    // game state
    Card firstCard;
    Card secondCard;

    void Awake() {
        Instance = this;
    }

    void Start() {
        SetupGrid();
        SetupCards();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void CheckCards() {
        if (firstCard == null || secondCard == null) {
            throw new System.Exception($"one of the cards [{firstCard}, {secondCard}] is null, this shouldn't happen");
        }

        if (firstCard.letter == secondCard.letter) {
            // TODO: calculate score and combo
            firstCard.DestroyCard();
            secondCard.DestroyCard();
        } else {
            // TODO: increase misses, and reset combo
            firstCard.Flip(flipDelayTime);
            secondCard.Flip(flipDelayTime);
        }

        firstCard = null;
        secondCard = null;
    }

    public void OnCardClicked(Card card, bool isShowingFace) {
        if (!isShowingFace && firstCard != null) {
            firstCard = null;
            return;
        }

        if (firstCard == null) {
            firstCard = card;
        } else if (card != firstCard) {
            secondCard = card;
            CheckCards();
        }
    }

    void SetupGrid() {
        Vector2 gridSize = grid.GetComponent<RectTransform>().rect.size;
        float minDimension = Mathf.Min(gridSize.x, gridSize.y);
        int maxCount = Mathf.Max(rows, columns);
        Vector2 cellSize = Vector2.one * (minDimension / maxCount);

        // the constraint is set to `fixed row count`
        grid.constraintCount = rows;
        grid.cellSize = cellSize;
    }

    void SetupCards() {
        int pairsCount = Mathf.CeilToInt(rows * columns / 2f);

        for (int i = 0; i < pairsCount; i++) {
            Card newCard1 = Instantiate(cardPrefab, grid.transform);
            Card newCard2 = Instantiate(cardPrefab, grid.transform);

            char letter = (char)('a' + i);

            newCard1.letter = letter;
            newCard2.letter = letter;

            newCard1.cardText.text = "" + letter;
            newCard2.cardText.text = "" + letter;
        }

        for (int i = 0; i < grid.transform.childCount; i++) {
            Transform card = grid.transform.GetChild(i);
            card.SetSiblingIndex(Random.Range(0, grid.transform.childCount));
        }
    }
}
