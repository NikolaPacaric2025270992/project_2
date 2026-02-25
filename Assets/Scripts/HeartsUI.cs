using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject heartPrefab;
    public Transform heartsPanel;

    public Sprite fullHeart;
    public Sprite threeQuartersHeart;
    public Sprite halfHeart;
    public Sprite quarterHeart;
    public Sprite emptyHeart;

    private System.Collections.Generic.List<Image> hearts = new List<Image>();

    void Start()
    {
        for (int i = 0; i < playerHealth.maxHearts; i++)
        {
            AddHeartPrefab();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHearts();
    }

    void AddHeartPrefab()
    {
        GameObject heartObj = Instantiate(heartPrefab, heartsPanel);
        Image heartImage = heartObj.GetComponent<Image>();
        hearts.Add(heartImage);
    }

    public void AddHeart()
    {
        AddHeartPrefab();
    }
    private void UpdateHearts()
    {
        int segmentsPerHeart = playerHealth.segmentsPerHeart;

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartSegments = Mathf.Clamp(playerHealth.currentSegment - (i * segmentsPerHeart), 0, segmentsPerHeart);

            switch (heartSegments)
            {
                case 4:
                    hearts[i].sprite = fullHeart; break;
                case 3:
                    hearts[i].sprite = threeQuartersHeart; break;
                case 2: hearts[i].sprite = halfHeart; break;
                case 1: hearts[i].sprite = quarterHeart; break;
                default: hearts[i].sprite = emptyHeart; break;
            }
        }

    }
}
