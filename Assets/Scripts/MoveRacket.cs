using UnityEngine;
using System.Collections;

public class MoveRacket : MonoBehaviour {
    public float Speed = 30;
    public string Axis = "Vertical";
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float vertical = Input.GetAxisRaw(Axis);
        _rigidbody2D.velocity = new Vector2(0, vertical) * Speed;
    }
}
