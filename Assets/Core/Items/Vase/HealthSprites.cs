using System.Collections;
using System.Collections.Generic;
using Match3;
using UnityEngine;

public class HealthSprites : MonoBehaviour
{
    #region COMPONENTS

    [SerializeField] private SpriteRenderer spriteRenderer;
    [Tooltip("High HP to Low HP")]
    [SerializeField] private List<Sprite> healthSprites;
    #endregion


    public void Start()
    {
        spriteRenderer.sprite = healthSprites[0];
    }
    
    public async void UpdateHealthSprite(int remainingHp)
    {
        if(remainingHp > 0)
        {
            var destroyParticleFx = gameObject.GetComponentInChildren<ParticleSystem>();
            if (destroyParticleFx != null)
            {
                destroyParticleFx.Play();
            }
        }
        
        var index = healthSprites.Count - remainingHp;
        index = Mathf.Min(index, healthSprites.Count - 1);
        spriteRenderer.sprite = healthSprites[index];
    }
}
