using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum Players
{
    PLAYER1,
    PLAYER2,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UnityEvent<Players> OnTurnChange;
    public UnityEvent<Players, int> OnPlayerScore;

    [SerializeField] private GameObject _ballPrefab;
    private Ball _ball;

    private int _p1score;
    private int _p2score;

    private const float SPAWN_TIME = 1.5f;

    private Players _currentTurn;
    public Players CurrentTurn => _currentTurn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _p1score = _p2score = 0;
        ChangeTurn((Players) Random.Range(0, 2)); //0 or 1
    }

    private void NewBall()
    {
        _ball = Instantiate(_ballPrefab).GetComponent<Ball>();
        _ball.OnPoint.AddListener(ChangePlayerScore);
        _ball.ResetPosition(_currentTurn);
    }

    public void ChangePlayerScore(Players player)
    {
        if (player == Players.PLAYER1)
        {
            _p1score++;
            OnPlayerScore?.Invoke(player, _p1score);
        } else if (player == Players.PLAYER2)
        {
            _p2score++;
            OnPlayerScore?.Invoke(player, _p2score);
        }

        ChangeTurn(player);
    }

    private IEnumerator ResetBall()
    {
        yield return new WaitForSeconds(SPAWN_TIME);
        NewBall();
    }

    public void ChangeTurn(Players turn)
    {
        if (_currentTurn != turn)
        {
            _currentTurn = turn;
            Debug.Log($"Different Turn: {turn}");
        }
        OnTurnChange?.Invoke(_currentTurn);
        StartCoroutine(ResetBall());
    }
}
