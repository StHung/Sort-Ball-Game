using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private static Player _ins;

    public static Player Ins { get => _ins; private set => _ins = value; }

    public Ball BallPicked { get; set; }
    public Tube FromTube { get; set; }
    public bool CanPlay { get; set; }

    private void Awake()
    {
        BallPicked = null;
        FromTube = null;
        CanPlay = false;
        Ins = this;
    }

    public void GrabBall(Ball ball, Tube tube)
    {
        BallPicked = ball;
        FromTube = tube;
    }

    public void DropBall()
    {
        BallPicked = null;
        FromTube = null;
    }
}
