using UnityEngine;

public class SaveManager : MonoBehaviour {
    GameManager gameManager { get => GameManager.Instance; }

    const string GRID_ROWS_KEY = "grid_rows";
    const string GRID_COLUMNS_KEY = "grid_columns";
    const string NUM_OF_PAIRS_KEY = "num_of_pairs";
    const string CARDS_KEY = "cards";
    const string GAME_STATE_KEY = "game_state";

    public void SaveGame() {
        string cards = "";
        for (int i = 0; i < gameManager.grid.transform.childCount; i++) {
            Card card = gameManager.grid.transform.GetChild(i).GetComponent<Card>();
            char letter = (card.transform.childCount == 0) ? '.' : card.letter;
            cards += letter;
        }

        string gameState = "";
        gameState += "" + gameManager.turns;
        gameState += ";" + gameManager.matches;
        gameState += ";" + gameManager.score;
        gameState += ";" + gameManager.combo;

        PlayerPrefs.SetInt(GRID_ROWS_KEY, gameManager.rows);
        PlayerPrefs.SetInt(GRID_COLUMNS_KEY, gameManager.columns);
        PlayerPrefs.SetInt(NUM_OF_PAIRS_KEY, gameManager.pairsCount);
        PlayerPrefs.SetString(CARDS_KEY, cards);
        PlayerPrefs.SetString(GAME_STATE_KEY, gameState);

        print($"saving: ({gameManager.rows}x{gameManager.columns}), ({gameManager.pairsCount}), ({gameState}), ({cards})");
    }

    public void LoadGameData() {
        string cards = PlayerPrefs.GetString(CARDS_KEY);
        string gameState = PlayerPrefs.GetString(GAME_STATE_KEY);

        gameManager.rows = PlayerPrefs.GetInt(GRID_ROWS_KEY);
        gameManager.columns = PlayerPrefs.GetInt(GRID_COLUMNS_KEY);
        gameManager.pairsCount = PlayerPrefs.GetInt(NUM_OF_PAIRS_KEY);

        gameManager.LoadGame(cards, gameState);

        print($"loaded: ({gameManager.rows}x{gameManager.columns}), ({gameManager.pairsCount}), ({gameState}), ({cards})");

        PlayerPrefs.DeleteAll();
    }

    public bool HasUnfinishedGame() {
        return PlayerPrefs.HasKey(GRID_ROWS_KEY)
            && PlayerPrefs.HasKey(GRID_COLUMNS_KEY)
            && PlayerPrefs.HasKey(NUM_OF_PAIRS_KEY)
            && PlayerPrefs.HasKey(CARDS_KEY)
            && PlayerPrefs.HasKey(GAME_STATE_KEY);
    }
}
