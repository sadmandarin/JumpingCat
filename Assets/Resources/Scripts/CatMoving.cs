using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CatMoving : MonoBehaviour
{
    [SerializeField] private float _jumpingForce;
    [SerializeField] private float _horizontalMove;

    private Rigidbody2D _rb;
    private float _startPos;
    private bool _firstDead;
    private bool _isImmortal = false;
    private bool _gameStarted;

    private void OnEnable()
    {
        GlobalGameManager.OnRevive += Revive;
        GlobalGameManager.OnGameRestarted += RestartGame;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position.y;
        _firstDead = true;
        Jump();
    }

    private void OnDisable()
    {
        GlobalGameManager.OnRevive -= Revive;
        GlobalGameManager.OnGameRestarted -= RestartGame;
    }

    private void FixedUpdate()
    {
        Movement();

        if (transform.position.y - _startPos > 1)
        {
            _startPos = transform.position.y;
            ScoreManager.Instance.AddScore(10);
        }
    }

    private void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpingForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformBase>())
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                if (!(collision.gameObject.GetComponent<PlatformBase>() is BreakablePlatform))
                {
                    Jump();
                }
                collision.gameObject.GetComponent<PlatformBase>().OnJump();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadCollider"))
        {
            if (!_isImmortal)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.simulated = false;

        if (_firstDead)
        {
            GlobalGameManager.Instance.FirstDead();
            _firstDead = false;
        }
        else
        {
            GlobalGameManager.Instance.SetState(GlobalGameManager.States.Gameover);
        }
    }

    private void RestartGame()
    {
        transform.position = new Vector3(0, transform.position.y, 0);
        transform.position += Vector3.up;
        _firstDead = true;
        _rb.simulated = true;
        Jump();
    }

    public void Revive()
    {
        _isImmortal = true;
        _rb.simulated = true;
        transform.position = new Vector3(0, transform.position.y, 0);
        Jump();
        StartCoroutine(ImmortalState());
    }

    private IEnumerator ImmortalState()
    {
        yield return new WaitForSeconds(GlobalGameManager.Instance.PlayerData.ImmortalTime);

        _isImmortal = false;
    }

    private void Movement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        _rb.linearVelocity = new Vector2(horizontalInput * _horizontalMove, _rb.linearVelocity.y);
    }
}
