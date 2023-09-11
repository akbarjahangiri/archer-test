
using Archer.Managers;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int score;
    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            _gameManager.AddScore(score);
        }
    }
}