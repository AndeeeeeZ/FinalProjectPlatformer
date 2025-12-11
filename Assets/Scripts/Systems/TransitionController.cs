using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] private VoidEvent OnTransitionOutDone;
    [SerializeField] private VoidEvent OnTransitionInDone; 
    [SerializeField] private Animator transitionAnimator; 
    [SerializeField] private bool playsTransitionIn;

    private void Start()
    {
        if (playsTransitionIn)
        {
            transitionAnimator.Play("TransitionIn");
        }
    }
    public void TransitionOutDone()
    {
        OnTransitionOutDone.Raise(); 
    }

    public void TransitionInDone()
    {
        OnTransitionInDone.Raise(); 
    }

    public void PlayTransitionOut()
    {
        transitionAnimator.Play("TransitionOut"); 
    }
}
