using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    
    public class Healthbar : MonoBehaviour 
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas healthBarCanvas = null;
        private void Update() 
        {
            
            if(Mathf.Approximately(healthComponent.GetFraction(),0) 
            || Mathf.Approximately(healthComponent.GetFraction(),1))
            {
                healthBarCanvas.enabled= false;
                return;
            }

            healthBarCanvas.enabled = true;
            foreground.localScale = new Vector3(healthComponent.GetFraction(),1,1);
            
        }
        
    }
    
}
