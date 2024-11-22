using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MovePlayer : MonoBehaviour
{

    float step_time = 0.76f, zoom = 1920f / 24.4f;
    private static Transform _move;
    private static Vector3
        _left = new Vector3(0, 270, 0),
        _right = new Vector3(0, 90, 0),
        _up = new Vector3(0, 0, 0),
        _down = new Vector3(0, 180, 0),
        prev_pos_head = new Vector3(),
        prev_pos_tail = new Vector3(),
        next_pos_tail = new Vector3();
    public GameObject food_prefab, snake_tail_prefab;
    private GameObject food;
    private List<GameObject> tail_list = new List<GameObject>();
    public TextMeshProUGUI score_text, high_score_text;
    private int score = 0;
    private bool win = false, lose = false;
    public GameObject text_win, text_lose;
    public Button button_left, button_right, button_up, button_down, main_menu, restart;


    void Start()
    {
        _move = gameObject.GetComponent<Transform>();
        Camera.main.orthographicSize = Screen.height / zoom;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            SaveTheGame.high_score = PlayerPrefs.GetInt("HighScore");
            high_score_text.SetText("High score: " + SaveTheGame.high_score);
        }
        StartCoroutine(step_move());
    }

    private static void move_left()
    {
        if (_move.eulerAngles.y != 90)
        {
            _move.rotation = Quaternion.Euler(_left);
        }
    }

    private static void move_right()
    {
        if (_move.eulerAngles.y != 270)
        {
            _move.rotation = Quaternion.Euler(_right);
        }
    }

    private static void move_up()
    {
        if (Math.Abs(_move.eulerAngles.y) != 180)
        {
            _move.rotation = Quaternion.Euler(_up);
        }
    }

    private static void move_down()
    {
        if (_move.eulerAngles.y != 0)
        {
            _move.rotation = Quaternion.Euler(_down);
        }
    }



    void Update()
    {
        if (!win && !lose)
        {
            if (food == null)
            {
                food = Instantiate(food_prefab, new Vector3(
                    UnityEngine.Random.Range(-13, 13),
                    0f,
                    UnityEngine.Random.Range(-11, 22)), Quaternion.identity
                    );

            }
        }

        button_left.onClick.AddListener(move_left);
        button_right.onClick.AddListener(move_right);
        button_up.onClick.AddListener(move_up);
        button_down.onClick.AddListener(move_down);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StopLine" || other.tag == "SnakeTail")
        {
            Destroy(gameObject);
            text_lose.SetActive(true);
            food.SetActive(false);
            lose = true;
            EndGame();
        }
        if (other.tag == "Food")
        {
            Destroy(other.transform.parent.gameObject);
            score++;
            if (step_time >= .12f) step_time -= .025f;
            score_text.SetText("Score: " + score);

            CreateTail();

            if (score > SaveTheGame.high_score)
            {
                SaveTheGame.high_score = score;
                SaveTheGame.SaveGame();
            }
            high_score_text.SetText("High score: " + SaveTheGame.high_score);
            if (score >= 500)
            {
                win = true;
                text_win.SetActive(true);
                EndGame();
                _move.position = new Vector3(0, 0, 6.4f);
                food.SetActive(false);
            }
        }
    }

    void EndGame()
    {
        main_menu.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        button_down.gameObject.SetActive(false);
        button_up.gameObject.SetActive(false);
        button_left.gameObject.SetActive(false);
        button_right.gameObject.SetActive(false);
        foreach (GameObject tail in tail_list)
        {
            Destroy (tail);
        }
    }

    IEnumerator step_move()
    {
        prev_pos_head = _move.position;
        _move.Translate(new Vector3(0, 0, 1), Space.Self);
        if (tail_list.Count > 0)
        {
            for (int i = 0; i < tail_list.Count; i++)
            {
                if (i == 0)
                {
                    prev_pos_tail = tail_list[i].transform.position;
                    tail_list[i].transform.position = prev_pos_head;
                }
                if (i > 0)
                {
                    next_pos_tail = tail_list[i].transform.position;
                    tail_list[i].transform.position = prev_pos_tail;
                    prev_pos_tail = next_pos_tail;
                }
            }
        }
        yield return new WaitForSeconds(step_time);
        if (!win && !lose)
            StartCoroutine(step_move());
    }

    void CreateTail()
    {
        if (tail_list.Count == 0)
        {
            tail_list.Add(Instantiate(snake_tail_prefab, prev_pos_head, Quaternion.identity));
            Debug.Log(tail_list);
            Debug.Log("Create tail after head");
            Debug.Log("Coordinates head: " + prev_pos_head);
            Debug.Log("Coordinates tale after head: " + tail_list[0].GetComponent<Transform>().position);
        }
        else
        {
            tail_list.Add(Instantiate(snake_tail_prefab, prev_pos_tail, Quaternion.identity));
            prev_pos_tail = tail_list.Last().GetComponent<Transform>().position;
        }
    }

}
