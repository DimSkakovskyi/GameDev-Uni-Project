using UnityEngine;

public class HealingPotionFactory : ItemFactoryBase
{
    public override GameObject SpawnItem()
    {
        GameObject potion = CreateBaseItem("HealingPotion");

        // Set sprite
        SpriteRenderer renderer = potion.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("All Potions/Red potions/HealingPotionR");

        // Add custom behavior
        potion.AddComponent<HealingPotion>();

        return potion;
    }
}
