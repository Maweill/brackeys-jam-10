using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace House_Scripts
{
    public class HouseTemplate : MonoBehaviour
    {
        private List<BlockTemplate> _blockTemplates;

        public bool IsHouseFilled { get; private set; }

        private void Awake()
        {
            _blockTemplates = GetComponentsInChildren<BlockTemplate>().ToList();
        }

        private void OnEnable()
        {
            GameEvents.BlockTemplateFilled += OnBlockTemplateFilled;
        }
        
        private void OnDisable()
        {
            GameEvents.BlockTemplateFilled -= OnBlockTemplateFilled;
        }

        public void Show()
        {
            _blockTemplates.ForEach(template => template.Show());
        }
        
        public void Hide()
        {
            _blockTemplates.ForEach(template => template.Hide());
        }
    
        public IEnumerator Lighten()
        {
            foreach (BlockTemplate blockTemplate in _blockTemplates) {
                blockTemplate.LightenBlock();
                yield return new WaitForSeconds(0.5f);
            }
        }
        
        private void OnBlockTemplateFilled(int _, bool b)
        {
            float averageFillPercentage = _blockTemplates.Average(template => template.FillPercentage);
            IsHouseFilled = averageFillPercentage > GlobalConstants.MIN_FILLED_HOUSE_PERCENTAGE;
        }
    }
}
