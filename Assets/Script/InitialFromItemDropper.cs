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
    public GameObject backgroundLightObject;
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
                backgroundLight2D.color = new Color(0.8f, 0.8f, 0.8f);
                break;
            case Rarity.Uncommon:
                backgroundLight2D.color = new Color(1, 1, 0);
                break;
            case Rarity.Rare:
                backgroundLight2D.color = new Color(0, 1, 0);
                break;
            case Rarity.Exotic:
                backgroundLight2D.color = new Color(0, 1, 1);
                break;
            case Rarity.Mythic:
                backgroundLight2D.color = new Color(2, 0, 2);
                break;
            case Rarity.Legendary:
                backgroundLight2D.color = new Color(2, 0, 0);
                break;
        }

        backgroundLightObject.transform.position = spriteRenderer.bounds.center;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
