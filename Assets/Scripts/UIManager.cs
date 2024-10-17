using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    [Header("Menues")]
    public GameObject mainMenu;
    public GameObject layoutSelectMenu;
    public GameObject ingameUI;

    [Header("Main Menu")]
    public Button resumeButton;

    [Header("Layout Selection")]
    public TMP_Text rowsText;
    public TMP_Text columnsText;
    public TMP_Text warningText;
    public Button playButton;

    [Header("In-Game")]
    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text turnsText;
    public TMP_Text matchesText;
    public GameObject gameCleared;

    GameManager gameManager { get => GameManager.Instance; }

    Dictionary<StatType, TMP_Text> statsMap;

    void Start() {
        resumeButton.interactable = gameManager.saveManager.HasUnfinishedGame();

        statsMap = new(){
          {StatType.Score, scoreText},
          {StatType.Combo, comboText},
          {StatType.Turns, turnsText},
          {StatType.Matches, matchesText},
        };
    }

    void CheckValidSize() {
        bool isValid = (gameManager.rows * gameManager.columns) % 2 == 0;
        playButton.interactable = isValid;
        warningText.gameObject.SetActive(!isValid);
    }

    public void UpdateStat(StatType stat, int newValue) {
        // TODO: add cool effect
        statsMap[stat].text = "" + newValue;
    }

    #region Callback Functions
    public void StartNewGame() {
        gameManager.StartNewGame();
        layoutSelectMenu.SetActive(false);
        ingameUI.SetActive(true);
    }

    public void ResumeGame() {
        gameManager.ContinueLastGame();
        mainMenu.SetActive(false);
        ingameUI.SetActive(true);
    }

    public void GotoMyWebsite() {
        Application.OpenURL("https://ahmedalnour.com");
    }

    public void ReloadScene() {
        // don't save if game is over
        if (gameManager.matches < gameManager.pairsCount) {
            gameManager.saveManager.SaveGame();
        } else {
            PlayerPrefs.DeleteAll();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame() {
        Application.Quit();
    }
    // --------------------
    public void ChangeRows(bool increase) {
        // TODO: Add cool effect
        gameManager.rows += increase ? 1 : -1;
        gameManager.rows = Mathf.Clamp(gameManager.rows, 1, 9);
        rowsText.text = "" + gameManager.rows;
        CheckValidSize();
    }

    public void ChangeColumns(bool increase) {
        // TODO: Add cool effect
        gameManager.columns += increase ? 1 : -1;
        gameManager.columns = Mathf.Clamp(gameManager.columns, 1, 9);
        columnsText.text = "" + gameManager.columns;
        CheckValidSize();
    }
    #endregion Callback Functions
}

public enum StatType {
    Score,
    Combo,
    Turns,
    Matches,
}
