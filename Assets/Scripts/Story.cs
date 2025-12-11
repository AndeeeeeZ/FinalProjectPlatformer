using UnityEngine;

public class Story : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private VoidEvent onStoryEnd; 
    private SpriteRenderer sr;
    private int i; 
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 
        i = 0; 
        sr.sprite = sprites[i]; 
    }

    public void DisplayNextPage()
    {
        i++; 
        if (i >= sprites.Length)
        {
            onStoryEnd.Raise(); 
            return; 
        }
        sr.sprite = sprites[i]; 
    }
}
