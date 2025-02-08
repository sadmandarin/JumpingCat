using UnityEngine;

public class MoneyItem : MonoBehaviour
{
    private Transform _parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CatMoving>())
        {
            GlobalGameManager.Instance.PlayerData.AddMoney(1);
            Destroy(gameObject);
        }
    }

    public void Init(Transform platform)
    {
        _parent = platform;
    }

    private void Update()
    {
        transform.position = _parent.position + new Vector3(0, 0.5f, 0); 
    }
}
