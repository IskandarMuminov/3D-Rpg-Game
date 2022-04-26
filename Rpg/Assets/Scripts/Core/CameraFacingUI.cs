
using UnityEngine;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Core
{   
    public class CameraFacingUI : MonoBehaviour 
    {
        //UI will face camer all the time
        public void LateUpdate() 
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
