using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Characters.AI_Character.Enemy
{
    public class TurretCharacter : EnemyCharacter
    {
        public float delayTime = 0f;

        private bool _hasWaited = false;
        protected override void Start()
        {
            base.Start();
            if (delayTime == 0f)
            {
                _hasWaited = true;
            }
            else
            {
                StartCoroutine(Wait());
            }
        }
        protected override void Update()
        {
            if(_hasWaited)
                PerformBasicAttack(transform.right);
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(delayTime);
            _hasWaited = true;
        }
    }
}