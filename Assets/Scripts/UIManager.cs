using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text turn;
    [SerializeField] private Text p1score;
    [SerializeField] private Text p2score;

    private readonly WaitForSeconds time = new(0.25f);

    private void Start()
    {
        GameManager.Instance.OnTurnChange.AddListener(ChangeTurnText);
        GameManager.Instance.OnPlayerScore.AddListener(ChangePlayerScore);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTurnChange.RemoveAllListeners();
        GameManager.Instance.OnPlayerScore.RemoveAllListeners();
    }

    private IEnumerator BlinkTurn()
    {
        for (int i = 0; i < 3; i++)
        {
            turn.gameObject.SetActive(true);
            yield return time;
            turn.gameObject.SetActive(false);
            yield return time;
        }
    }

    public void ChangeTurnText(Players player)
    {
        string text = player == Players.PLAYER1? "1st" :"2nd";
        turn.text = $"{text} Player's turn";
        StartCoroutine(BlinkTurn());
    }

    public void ChangePlayerScore(Players player, int score)
    {
        switch (player)
        {
            case Players.PLAYER1:
                p1score.text = score.ToString();
                break;
            case Players.PLAYER2:
                p2score.text = score.ToString();
                break;
        }
    }
}
