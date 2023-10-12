using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InitialFromItemDropper : MonoBehaviour
{
    [field: SerializeField] public ItemSO InventoryItem { get; set; }

    [Header("Connect Object")]
    public SpriteRenderer spriteRenderer;
    public Light2D spriteLight2D;
    public Light2D backgroundLight2D;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = InventoryItem.Image;
        spriteLight2D.lightCookieSprite = InventoryItem.Image;

        switch (InventoryItem.Rarity)
        {
            case Rarity.Common:
                backgroundLight2D.color = new Color(255, 255, 255);
                break;
            case Rarity.Uncommon:
                backgroundLight2D.color = new Color(245, 250, 50);
                break;
            case Rarity.Rare:
                backgroundLight2D.color = new Color(0, 255, 255);
                break;
            case Rarity.Exotic:
                backgroundLight2D.color = new Color(80, 0, 255);
                break;
            case Rarity.Mythic:
                backgroundLight2D.color = new Color(255, 0, 225);
                break;
            case Rarity.Legendary:
                backgroundLight2D.color = new Color(255, 0, 0);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
