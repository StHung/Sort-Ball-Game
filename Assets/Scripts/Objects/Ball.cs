using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] BallColor ballColor;

    [SerializeField] SpriteRenderer ballRenderer;

    [SerializeField] GameObject mask;

    [SerializeField] bool isSpecialBall;

    public BallColor Ballcolor { get => ballColor; private set => ballColor = value; }
    public bool IsSpecialBall { get => isSpecialBall; set => isSpecialBall = value;}

    private void Start()
    {
        SetBallColor(ballColor);
        mask.SetActive(isSpecialBall);
    }
    public void SetBallColor(BallColor color)
    {
        ballColor = color;
        ballRenderer.color = ColorPicker.GetColor((int)color);
    }
    
    public void ShowColorOfSpecialBall()
    {
        mask.GetComponent<Animator>().SetBool("showBallColor", true);
    }
}
