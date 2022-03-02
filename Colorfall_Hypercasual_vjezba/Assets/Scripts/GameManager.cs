using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	bool gameStarted = false;
	public Image mainMenu;
    public Ball ball;

	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

    private void Start()
    {
        ball.Stop();
    }

    void Update () {
		if (!gameStarted) {

            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
             
          
            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0 || currentSwipe.x > -0.5f || currentSwipe.x < 0.5f)
                {
                    mainMenu.gameObject.SetActive(false);
                    Ball.Move = gameStarted = true;
                }
            }
            }
            

            //GameObject.FindGameObjectWithTag("Destroy").SetActive(false);
            //FindObjectOfType<Text>().gameObject.SetActive(false);
   //         mainMenu.gameObject.SetActive(false);
			//Ball.Move = gameStarted = true;
		}
	}

