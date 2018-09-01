using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace com.natgeo.xr
{
    [RequireComponent(typeof(Animator))]
    public class AssetAnimate : MonoBehaviour
    {
        private int playhead = 0;

        private Animator animator;

        public enum AnimatorTriggerType
        {
            Trigger,
            Integer,
            Float,
            Bool
        }

        [System.Serializable]
        public struct SequenceInfo
        {
            public float startDelay;
            public AnimatorTriggerType triggerType;
            public string triggerName;
            public string triggerValue;
            public UnityEvent OnStartAnimation;
            public UnityEvent OnFinishAnimation;
        }

        public List<SequenceInfo> animationTriggers;
        private bool animating = false;

        private UnityEvent currentOnFinishAnimation;


        private void StartAnimate()
        {
            if (animationTriggers == null)
                return;
            
            SequenceInfo sInfo = animationTriggers[playhead];

            currentOnFinishAnimation = sInfo.OnFinishAnimation;

            if (sInfo.OnStartAnimation != null)
            {
                sInfo.OnStartAnimation.Invoke();
            }

            switch (sInfo.triggerType)
            {
                case AnimatorTriggerType.Bool:
                    bool valBool = Convert.ToBoolean(sInfo.triggerValue);
                    animator.SetBool(sInfo.triggerName, valBool);
                    break;
                case AnimatorTriggerType.Float:
                    float valFloat = 0;
                    if (float.TryParse(sInfo.triggerValue, out valFloat))
                    {
                        animator.SetFloat(sInfo.triggerName, valFloat);
                    }
                    else
                    {
                        Debug.LogError(this.name + " Error parsing value for animator FLOAT");
                    }
                    break;
                case AnimatorTriggerType.Integer:
                    int valInt = 0;
                    if (int.TryParse(sInfo.triggerValue, out valInt))
                    {
                        animator.SetInteger(sInfo.triggerName, valInt);
                    }
                    else
                    {
                        Debug.LogError(this.name + " Error parsing value for animator INTEGER");
                    }
                    break;
                case AnimatorTriggerType.Trigger:
                    animator.SetTrigger(sInfo.triggerName);
                    break;
            }
            animating = true;
        }

        IEnumerator DelayWait(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartAnimate();
        }


        void Start()
        {

            animator = GetComponent<Animator>();
        }


        void Update()
        {
            if (animating)
            {
                bool isPlaying = (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0));
                if (isPlaying == false)
                {
                    animating = false;
                    if (currentOnFinishAnimation != null)
                    {
                        currentOnFinishAnimation.Invoke();
                    }
                }
            }
        }


        public void StartAnimator(int animationTrigger)
        {
            if (animationTriggers == null)
                return;

            playhead = animationTrigger;

            SequenceInfo sInfo = animationTriggers[playhead];
            StartCoroutine(DelayWait(sInfo.startDelay));
        }
    }
}
