using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HouseTemplate : MonoBehaviour
{
    private List<BlockTemplate> _houseBlockTemplates;

    public bool IsHouseFilled { get; private set; }

    private void Awake()
    {
        _houseBlockTemplates = GetComponentsInChildren<BlockTemplate>().ToList();
    }

    private void Start()
    {
        GameEvents.BlockTemplateFilled += OnBlockTemplateFilled;
    }
    
    private void OnBlockTemplateFilled(int _)
    {
        IsHouseFilled = _houseBlockTemplates.All(template => template.IsPlaced);
    }
}