using UnityEngine;

public abstract class ItemFactoryBase
{
    public virtual GameObject CreateBaseItem(string name)
    {
        GameObject item = new GameObject(name);

        // Add SpriteRenderer
        item.AddComponent<SpriteRenderer>().sortingOrder = 1;

        // Add Collider
        CircleCollider2D col = item.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        // Add Rigidbody
        Rigidbody2D rb = item.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        return item;
    }

    // Abstract function child factories must override
    public abstract GameObject SpawnItem();
}

