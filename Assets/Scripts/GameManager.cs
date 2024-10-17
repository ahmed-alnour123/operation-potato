using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    // prefabs and game setup
    public Card cardPrefab;
    public List<Sprite> sprites;

    // components
    public SoundManager soundManager;
    public SaveManager saveManager;
    public GridLayoutGroup grid;

    // game parameters
    public float flipDelayTime = 2f;
    public float peekTimeout = 2f;
    public int rows = 3;
    public int columns = 3;

    // game state
    public int flips = 0;
    public int matches = 0;
    public int score = 0;
    public int combo = 0;
    public int pairsCount;
    Card firstCard;
    Card secondCard;

    void Awake() {
        Instance = this;
    }

    void Start() {
        if (saveManager.HasUnfinishedGame()) {
            ContinueLastGame();
        } else {
            StartNewGame();
        }
    }

    public void ContinueLastGame() {
        saveManager.LoadGameData();
        Invoke(nameof(FlipAllCards), peekTimeout);
    }

    public void StartNewGame() {
        SetupGrid();
        SetupCards();
        Invoke(nameof(FlipAllCards), peekTimeout);
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

        flips++;
        if (firstCard.letter == secondCard.letter) {
            matches++;
            combo++;
            score += (1 << (combo - 1));

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            soundManager.PlaySound(SoundEffect.Match);

            if (matches == pairsCount) {
                Invoke(nameof(EndGame), 1);
            }
        } else {
            combo = 0;

            firstCard.Flip(flipDelayTime);
            secondCard.Flip(flipDelayTime);
            soundManager.PlaySound(SoundEffect.Mismatch);
        }

        firstCard = null;
        secondCard = null;
    }

    void EndGame() {
        soundManager.PlaySound(SoundEffect.Endgame);
        // TODO: Show Endgame UI
    }

    public void LoadGame(string cards, string gameState) {
        List<int> state = gameState.Split(";").Select(s => int.Parse(s)).ToList();

        this.flips = state[0];
        this.matches = state[1];
        this.score = state[2];
        this.combo = state[3];

        SetupGrid();

        foreach (char c in cards) {
            var newCard = GenerateCard(c);
            if (c == '.') {
                Destroy(newCard.cardView.gameObject);
            }
        }
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
        pairsCount = Mathf.CeilToInt(rows * columns / 2f);

        for (int i = 0; i < pairsCount; i++) {
            char letter = (char)('a' + i);
            GenerateCard(letter);
            GenerateCard(letter);
        }

        for (int i = 0; i < grid.transform.childCount; i++) {
            Transform card = grid.transform.GetChild(i);
            card.SetSiblingIndex(Random.Range(0, grid.transform.childCount));
        }
    }

    Card GenerateCard(char letter) {
        Card newCard = Instantiate(cardPrefab, grid.transform);
        newCard.cardText.text = "" + letter;
        newCard.letter = letter;
        newCard.canFlip = false;
        return newCard;
    }

    void FlipAllCards() {
        for (int i = 0; i < grid.transform.childCount; i++) {
            Card card = grid.transform.GetChild(i).GetComponent<Card>();
            card.Flip();
        }
    }
}
