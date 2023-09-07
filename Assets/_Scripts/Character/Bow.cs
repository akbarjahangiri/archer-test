using System;
using Archer.Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace Archer.Character
{
    public class Bow : MonoBehaviour
    {
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private Transform arrowPosition;
        [SerializeField] private int magzineSize = 5;
        [SerializeField] private int magzineMaxSize = 50;

        private IObjectPool<Arrow> _arrowPool;
        private Arrow _loadedArrow;
       [SerializeField] private float forceAmount = 1;

        private void Start()
        {
            _arrowPool = new ObjectPool<Arrow>(CreateArrow, ActionOnGetArrow, ActionOnReleaseBullet,
                arrow => { Destroy(arrow.gameObject); }, false, magzineSize, magzineMaxSize);
         //   GetArrow();
            InputManager.Instance.OnRotate += GetArrow;
            InputManager.Instance.OnShoot += ShootArrow;
        }


        private void OnDestroy()
        {
            InputManager.Instance.OnRotate -= GetArrow;
        }

        private Arrow CreateArrow()
        {
            Arrow arrow = Instantiate(this.arrowPrefab);
            arrow.transform.SetParent(transform, false);
            arrow.rigidbody2D.gravityScale = 0;
            //arrow.transform.localRotation = Quaternion.identity;

            return arrow;
        }

        private void ActionOnGetArrow(Arrow obj)
        {
            return;
        }

        private void ActionOnReleaseBullet(Arrow obj)
        {
            arrowPrefab.gameObject.SetActive(false);
        }

        private void GetArrow()
        {
            if (_arrowPool != null)
            {
                Debug.Log("Get Arrow");
                _loadedArrow = _arrowPool.Get();
                _loadedArrow.gameObject.SetActive(true);
            }
        }

        private void ShootArrow()
        {
            // Activate the loaded arrow and set its position and rotation
            //_loadedArrow.gameObject.SetActive(true);
           // _loadedArrow.transform.position = arrowPosition.position;
            //_loadedArrow.transform.rotation = arrowPosition.rotation;

            // Get the rigidbody of the loaded arrow
            
            if (_loadedArrow.rigidbody2D != null)
            {
                _loadedArrow.rigidbody2D.gravityScale = 2.5f;
                // Apply force to the arrow in the forward direction (you can adjust the force value)
                _loadedArrow.rigidbody2D.AddForce(-_loadedArrow.transform.right * forceAmount, ForceMode2D.Impulse);
                _loadedArrow.isShooting = true;
            }
            // Release the arrow back to the pool
            //_arrowPool.Release(_loadedArrow);
            _loadedArrow = null;
        }
    }
}