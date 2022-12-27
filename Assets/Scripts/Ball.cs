using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Ball : MonoBehaviour {
    public float Speed = 20f;
    public UnityEvent<Players> OnPoint;

    private Rigidbody2D _rigidbody;
    [SerializeField] private Vector2 _startDir;

    [SerializeField] private Transform _p1Spawn;
    [SerializeField] private Transform _p2Spawn;

    [SerializeField] private GameObject _trailEffect;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    float hitFactor(Vector2 ballPos, Vector2 racketPos,
                    float racketHeight) {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    public void ResetPosition(Players player)
    {
        _rigidbody.velocity = Vector2.zero;
        transform.position = new Vector3(
            player == Players.PLAYER1 ? _p1Spawn.position.x : _p2Spawn.position.x,
            Random.Range(-13f, 13f) //Random y
        );
       
        _trailEffect.SetActive(false);

        _startDir = Vector2.zero;
        int randomStart = Random.Range(0, 100);

        switch (GameManager.Instance.CurrentTurn)
        {
            case Players.PLAYER1:
                _startDir = randomStart < 50 ?
                    Vector2.right + Vector2.up : Vector2.right + Vector2.down;
                break;
            case Players.PLAYER2:
                _startDir = randomStart < 50 ?
                    Vector2.left + Vector2.up : Vector2.left + Vector2.down;
                break;
        }

        // Initial Velocity
        _rigidbody.velocity = _startDir * Speed;
        _trailEffect.SetActive(true); //show trail again 
    }

    void OnCollisionEnter2D(Collision2D col) {
        // Note: 'col' holds the collision information. If the
        // Ball collided with a racket, then:
        //   col.gameObject is the racket
        //   col.transform.position is the racket's position
        //   col.collider is the racket's collider
        
        // Hit the left Racket?
        if (col.gameObject.name == "RacketLeft") {
            // Calculate hit Factor
            float y = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1, y).normalized;

            // Set Velocity with dir * speed
            _rigidbody.velocity = dir * Speed;
        }

        // Hit the right Racket?
        if (col.gameObject.name == "RacketRight") {
            // Calculate hit Factor
            float y = hitFactor(transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);

            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1, y).normalized;
            
            // Set Velocity with dir * speed
            _rigidbody.velocity = dir * Speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Point>(out var point))
            return;

        Debug.Log($"Player {point.playerNumber} scored");
        OnPoint?.Invoke(point.playerNumber);
        OnPoint.RemoveAllListeners();
        Destroy(gameObject);
    }

}
