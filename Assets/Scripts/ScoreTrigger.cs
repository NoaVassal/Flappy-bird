using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private bool _hasScored;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_hasScored)
        {
            return;
        }

        var bird = collision.GetComponent<BirdController>();

        if (bird == null)
        {
            return;
        }

        _hasScored = true;

        GameManager.Instance.AddScore();
    }
}