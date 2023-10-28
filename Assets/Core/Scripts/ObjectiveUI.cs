using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    #region COMPONENTS

    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;
    [SerializeField] private ParticleSystem objectiveDownParticle;
    #endregion

    public void Initialize(int count, Sprite sprite)
    {
        UpdateObjectiveCount(count, false);
        UpdateObjectiveImage(sprite);
    }
    
    public void UpdateObjectiveCount(int count, bool playParticle = true)
    {
        countText.text = count.ToString();

        if (playParticle)
        {
            objectiveDownParticle.Play();
        }
    }

    public void UpdateObjectiveImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
