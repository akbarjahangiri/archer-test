using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Pool;

namespace Archer.Managers
{
    public class ArcherController : MonoBehaviour
    {
   

        private IObjectPool<Arrow> _arrowPool;

        private void Awake()
        {
            GameManager.OnGameInit += HandleGameInit;
             //InputManager.Instance.OnRotate += StartRotationAnimation;
            // InputManager.Instance.OnCharge += StartCharging;
        }


        private void OnDestroy()
        {
            GameManager.OnGameInit -= HandleGameInit;
            // InputManager.Instance.OnRotate -= StartRotationAnimation;
            // InputManager.Instance.OnCharge -= StartCharging;
        }

        private void HandleGameInit()
        {
            //StartRotationAnimation();
        }
        
        


    
    }
}