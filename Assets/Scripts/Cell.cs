using UnityEngine;

//use for managing cell states (normal, highlight, hover, hide)(attach to cell prefab)
public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite spriteNormal;
    [SerializeField] private Sprite spriteHighlight;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        spriteRenderer.sprite = spriteNormal;
    }

    public void Normal()
    {
        gameObject.SetActive(true);
        spriteRenderer.sprite = spriteNormal;
        spriteRenderer.color = Color.white;
    }

    public void Highlight()
    {
        gameObject.SetActive(true);
        spriteRenderer.sprite = spriteHighlight;
        spriteRenderer.color = Color.white;
    }

    public void Hover()
    {
        gameObject.SetActive(true);
        spriteRenderer.sprite = spriteNormal;
        spriteRenderer.color = new Color(spriteRenderer.color.r,
            spriteRenderer.color.g,
            spriteRenderer.color.b,
            0.5f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
