using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tube : MonoBehaviour, ITubeIsSortingCompleted, IPointerDownHandler
{
    private bool isSorted;

    private bool isPushState;

    public Transform inOutPoint;

    public Stack<Ball> ballStack = new Stack<Ball>();

    [HideInInspector] public int stackSize;

    private void Start()
    {
        isSorted = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSorted || !Player.Ins.CanPlay)
            return;
        if(ballStack.Count == 0 && Player.Ins.BallPicked != null)
        {
            PushBall(Player.Ins.BallPicked);
        }
        else if(ballStack.Count > 0 && ballStack.Count < stackSize)
        {
            if(Player.Ins.BallPicked == null)
            {
                Player.Ins.GrabBall(PopBall(), this);
            }
            else
            {
                if(Player.Ins.FromTube.GetInstanceID() == this.GetInstanceID())
                {
                    PushBall(Player.Ins.BallPicked);
                }
                else
                {
                    if(Player.Ins.BallPicked.Ballcolor == this.ballStack.Peek().Ballcolor)
                    {
                        PushBall(Player.Ins.BallPicked);
                    }
                    else
                    {
                        Player.Ins.FromTube.PushBall(Player.Ins.BallPicked);
                        Player.Ins.GrabBall(PopBall(), this);
                    }
                }
            }
        }
        else if(ballStack.Count == 4)
        {
            if(Player.Ins.BallPicked == null)
            {
                Player.Ins.GrabBall(PopBall(), this);
            }
            else
            {
                Player.Ins.FromTube.PushBall(Player.Ins.BallPicked);
                Player.Ins.GrabBall(PopBall(), this);
            }
        }
        AudioManager.Instance.PlayTouchSound();
    }
    private void PushBall(Ball ball)
    {
        isPushState = true;
        ballStack.Push(ball);
        isSorted = CheckSorted();
        if (isSorted)
        {
            OnTubeIsSortingCompleted();
        }
        StartCoroutine(AnimateBall(ball, inOutPoint.position, 20f));
        Player.Ins.DropBall();
    }

    private Ball PopBall()
    {
        isPushState = false;
        Ball ballIsTaken = ballStack.Pop();
        StartCoroutine(AnimateBall(ballIsTaken, inOutPoint.position, 20f));
        
        if(ballStack.Count > 0 && ballStack.Peek() != null && ballStack.Peek().IsSpecialBall)
        {
            ballStack.Peek().ShowColorOfSpecialBall();
        }

        return ballIsTaken;
    }

    private bool CheckSorted()
    {
        if (ballStack.Count == stackSize)
        {
            Ball[] cloneBalls = ballStack.ToArray();
            for (int i = 0; i < cloneBalls.Length - 1; i++)
            {
                if (cloneBalls[i].Ballcolor != cloneBalls[i + 1].Ballcolor)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    IEnumerator AnimateBall(Ball ball, Vector3 targetPos, float speed)
    {
        if (!isPushState)
        {
            ball.GetComponent<Rigidbody2D>().simulated = false;
        }
        while (ball.transform.position != targetPos)
        {
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, targetPos, Time.deltaTime * speed);
            yield return null;
        }

        if (isPushState)
        {
            ball.GetComponent<Rigidbody2D>().simulated = true;
        }
    }

    public void OnTubeIsSortingCompleted()
    {
        EventManager.EmitEvent(EventName.On_Tube_Is_Sorting_Completed);
    }

}
