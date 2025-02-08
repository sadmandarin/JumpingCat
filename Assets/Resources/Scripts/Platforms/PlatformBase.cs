using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    protected abstract void SpecialAction();
    public abstract void OnJump();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadCollider"))
        {
            gameObject.SetActive(false);
            PlatformPool.Instance.RemoveFromActive(gameObject);
        }
    }

}
