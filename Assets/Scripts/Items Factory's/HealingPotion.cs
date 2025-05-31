using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    public int healingAmount = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.Healing(healingAmount);
                Debug.Log("I am healed");
                Destroy(gameObject);
            }
        }
    }
}
