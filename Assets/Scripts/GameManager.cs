using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager Instance;

  Card firstCard;
  Card secondCard;

  public float flipDelayTime = 2f;

  void Awake(){
    Instance = this;
  }

  void Update(){
    if (Input.GetKeyDown(KeyCode.R)){
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  }

  public void CheckCards(){
    if (firstCard == null || secondCard == null){
      throw new System.Exception($"one of the cards [{firstCard}, {secondCard}] is null, this shouldn't happen");
    }

    if (firstCard.data == secondCard.data){
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

  public void OnCardClicked(Card card, bool isShowingFace){
    if (!isShowingFace && firstCard != null){
      firstCard = null;
      return;
    }

    if (firstCard == null){
      firstCard = card;
    } else if(card != firstCard) {
      secondCard = card;
      CheckCards();
    }
  }
}
