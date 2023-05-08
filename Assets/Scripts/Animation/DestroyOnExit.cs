using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class DestroyOnExit : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject.GetComponent<Rigidbody2D>());
            Destroy(animator.gameObject, stateInfo.length);
        }
    }
}