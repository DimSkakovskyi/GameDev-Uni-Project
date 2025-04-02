using UnityEngine;

public class HealingPotionFactory : ItemFactoryBase
{
    public override void SpawnItem(Vector2 position)
    {
        GameObject potion = new GameObject("HealingPotion");

        // Add SpriteRenderer (you can replace this with your actual sprite)
        SpriteRenderer renderer = potion.AddComponent<SpriteRenderer>();
        // Load a sprite from Resources (make sure you have one)
        renderer.sprite = Resources.Load<Sprite>("All Potions/Red potions/HealingPotionR"); 
        renderer.sortingOrder = 1;

        // Add other components as needed
        potion.AddComponent<CircleCollider2D>().isTrigger = true;
        Rigidbody2D rb = potion.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // Add healing logic script if you have one
        potion.AddComponent<HealingPotion>(); // your custom script

        // Set position
        potion.transform.position = position;
    }
}
