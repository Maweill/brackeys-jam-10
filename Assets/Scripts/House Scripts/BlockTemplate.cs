using System;
using System.Collections;
using System.Collections.Generic;
using House_Scripts;
using UnityEngine;

public class BlockTemplate : MonoBehaviour
{
    [SerializeField]
    private int _blockTemplateId;

    public bool IsPlaced { get; private set; } = false;

    private const float MIN_PERCENTAGE = 60f;
    private void Start()
    {
        GameEvents.BlockPlaced += OnBlockPlaced;
    }

    private void OnBlockPlaced(Block block)
    {
        if (_blockTemplateId != block.ID)
        {
            return;
        }
        
        float distance = Vector2.Distance(transform.position, block.transform.position);
        float maxSize = Mathf.Max(transform.localScale.x, block.transform.localScale.x);
        float distancePercentage = Mathf.Clamp01(1 - (distance / maxSize));
        float percentage = distancePercentage * 100f;
        
        if (percentage >= MIN_PERCENTAGE)
        {
            IsPlaced = true;
        }
        
        GameEvents.InvokeBlockTemplateFilled(_blockTemplateId);
        
        // Выводим значение в процентах в консоль
        Debug.Log("BlockTemplate: Расстояние в процентах: " + percentage.ToString("F2") + "%");
    }

}
