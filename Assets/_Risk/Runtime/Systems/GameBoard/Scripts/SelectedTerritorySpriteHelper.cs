using System;
using UnityEngine;

public class SelectedTerritorySpriteHelper : MonoBehaviour
{
    [SerializeField] private Sprite _selectedSprite; 
    private SpriteRenderer _spriteRenderer;
    private Sprite _unselectedSprite;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _unselectedSprite = _spriteRenderer.sprite;
    }
    
    public void SetSelected(bool isSelected)
    {
        _spriteRenderer.sprite = isSelected ? _selectedSprite : _unselectedSprite;
    }
}
