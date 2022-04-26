using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;
using System;

namespace RPG.UI
{

    public class DamageText : MonoBehaviour 
    {
        [SerializeField] Text damageText= null;
        Health health;
        public void SetValue(float amount)
        {
            damageText.text = String.Format("{0:0}",amount);
        }
        
    }

}