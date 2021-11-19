using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SpawnManager : MonoBehaviour
{
    public GameObject tubeGrid;
    public Tube tubePrefab;
    public int stackSize;

    public bool genFirstLevel;
    public bool autoGenLevel;

    public int amountTubeToSpawn;
    public int amountEmptyTube;
    public int amountSpecialBall;

    public Ball ballPrefab;
    [Range(0f, 1.0f)]
    public float spawnBallTime = 0.2f;

    private List<BallColor> ballColors;
    private List<Tube> tubes;

    private void Awake()
    {
        ballColors = new List<BallColor>();
        tubes = new List<Tube>();
    }

    void Start()
    {
        if (!genFirstLevel)
        {
            if (autoGenLevel)
            {
                amountTubeToSpawn = Random.Range(6, ColorPicker.GetLength());
                amountEmptyTube = 2;
                amountSpecialBall = Random.Range(0, (amountTubeToSpawn - amountEmptyTube)*3);
            }

            GameManager.Instance.NumberTubeNeedToBeSorted = amountTubeToSpawn - amountEmptyTube;
            StartCoroutine(SpawnTubesDistributeBall());
        }
        else
        {
            amountTubeToSpawn = 2;
            amountEmptyTube = 0;
            GameManager.Instance.NumberTubeNeedToBeSorted = 1;
            StartCoroutine(StartGenFirstLevel());
        }
    }

    IEnumerator StartGenFirstLevel()
    {
        SpawnTubes();
        InitialColors();
        yield return new WaitForSeconds(1f);

        while (ballColors.Count > 0)
        {
            if (tubes[0].ballStack.Count < stackSize - 1)
            {
                SpawnBall(ballColors.Count - 1, tubes[0], false);
            }
            else
            {
                SpawnBall(ballColors.Count - 1, tubes[1], false);
            }
            yield return new WaitForSeconds(spawnBallTime);
        }

        Player.Ins.CanPlay = true;
    }

    IEnumerator SpawnTubesDistributeBall()
    {
        SpawnTubes();
        InitialColors();
        yield return new WaitForSeconds(1f);

        int amountTubeNeedFilled = amountTubeToSpawn - amountEmptyTube;

        for (int i = 0; i < amountTubeNeedFilled; i++)
        {
            while (tubes[i].ballStack.Count < stackSize)
            {
                int randomBallColorIndexToSpawnBall = Random.Range(0, ballColors.Count);
 
                if (tubes[i].ballStack.Count == stackSize - 1)
                {
                    if (tubes[i].ballStack.Peek().Ballcolor == ballColors[randomBallColorIndexToSpawnBall])
                        continue;
                }

                if (amountSpecialBall > 0 && tubes[i].ballStack.Count < stackSize - 1)
                {
                    SpawnBall(randomBallColorIndexToSpawnBall, tubes[i], true);
                    amountSpecialBall--;
                }
                else
                {
                    SpawnBall(randomBallColorIndexToSpawnBall, tubes[i], false);

                }
                yield return new WaitForSeconds(spawnBallTime);
            }
        }
        Player.Ins.CanPlay = true;
    }

    private void SpawnTubes()
    {
        for (int i = 0; i < amountTubeToSpawn; i++)
        {
            Tube tube = Instantiate(tubePrefab, tubeGrid.transform);
            tube.stackSize = stackSize;
            tubes.Add(tube);
        }
    }

    private void SpawnBall(int color, Tube tube, bool isSpecialBall)
    {
        Ball ball = Instantiate(ballPrefab, tube.inOutPoint.position, Quaternion.identity);
        ball.SetBallColor(ballColors[color]);
        ball.IsSpecialBall = isSpecialBall;
        ballColors.RemoveAt(color);
        tube.ballStack.Push(ball);
    }

    private void InitialColors()
    {
        int numberColorTypes = (genFirstLevel == true) ? 1 : (amountTubeToSpawn - amountEmptyTube);

        for (int i = 1; i <= numberColorTypes; i++)
        {
            for (int j = 1; j <= stackSize; j++)
            {
                ballColors.Add((BallColor)i);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpawnManager))]
class CustomSpawnManager : Editor
{
    public override void OnInspectorGUI()
    {

        var spawnManger = target as SpawnManager;

        spawnManger.tubeGrid = EditorGUILayout.ObjectField("Tube Grid", spawnManger.tubeGrid, typeof(GameObject), true) as GameObject;
        spawnManger.tubePrefab = EditorGUILayout.ObjectField("Tube Prefab", spawnManger.tubePrefab, typeof(Tube), true) as Tube;
        spawnManger.stackSize = EditorGUILayout.IntSlider("Tube Size", spawnManger.stackSize, 1, 4);

        spawnManger.ballPrefab = EditorGUILayout.ObjectField("Ball Prefab", spawnManger.ballPrefab, typeof(Ball), true) as Ball;
        spawnManger.spawnBallTime = EditorGUILayout.FloatField("Spawn Ball Time  ", spawnManger.spawnBallTime);

        spawnManger.genFirstLevel = GUILayout.Toggle(spawnManger.genFirstLevel, "Gen first Level");

        if (!spawnManger.genFirstLevel)
        {
            spawnManger.autoGenLevel = GUILayout.Toggle(spawnManger.autoGenLevel, "Auto Gen Level");
            if (!spawnManger.autoGenLevel)
            {
                spawnManger.amountTubeToSpawn = EditorGUILayout.IntSlider("Amount tube to spawn", spawnManger.amountTubeToSpawn, 2, ColorPicker.GetLength() - 1);
                spawnManger.amountEmptyTube = EditorGUILayout.IntSlider("Amount empty tube", spawnManger.amountEmptyTube, 1, 2);
                spawnManger.amountSpecialBall = EditorGUILayout.IntSlider("Amount special ball", spawnManger.amountSpecialBall, 0, (spawnManger.amountTubeToSpawn-spawnManger.amountEmptyTube)*3);
            }
        }
    }
}
#endif