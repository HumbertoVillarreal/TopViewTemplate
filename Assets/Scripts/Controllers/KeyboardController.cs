using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private GameObject keyboard;
    [SerializeField] private GameObject actions;
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private SaveFileMenuController saveFileController;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject playerNamePanel;

    public int charLimit = 10; 

    private GameObject[,] keys;
    private GameObject[] actionKeys;

    private int rows = 5;
    private int columns = 10;

    private int currentX;
    private int currentY;

    private int currentAction;

    private bool inActions = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        keys = new GameObject[rows, columns];

        int index = 0;

        foreach (Transform child in keyboard.transform)
        {
            int row = index / columns;
            int column = index % columns;

            keys[row, column] = child.gameObject;

            index++;
        }


        actionKeys = new GameObject[actions.transform.childCount];

        for (int i = 0; i < actions.transform.childCount; i++)
        {
            actionKeys[i] = actions.transform.GetChild(i).gameObject;
        }


        UpdateSelection();
    }

    private void Update()
    {

    }

    private void MoveKeyboard(int x, int y)
    {
        currentX += x;
        currentY += y;

        // Ir a actions
        if (currentX > columns - 1)
        {
            inActions = true;

            currentX = columns - 1;

            UpdateSelection();

            return;
        }

        // Wrap izquierda
        if (currentX < 0)
        {
            currentX = columns - 1;
        }

        // Vertical
        if (currentY < 0)
        {
            currentY = rows - 1;
        }
        else if (currentY > rows - 1)
        {
            currentY = 0;
        }

        UpdateSelection();
    }

    private void MoveAction(int y)
    {
        currentAction += y;

        if (currentAction < 0)
        {
            currentAction = actionKeys.Length - 1;
        }
        else if (currentAction >= actionKeys.Length)
        {
            currentAction = 0;
        }

        UpdateSelection();
    }


    private void Move(int x, int y)
    {
        if (inActions)
        {
            // Volver al keyboard
            if (x < 0)
            {
                inActions = false;

                UpdateSelection();

                return;
            }

            MoveAction(y);
        }
        else
        {
            MoveKeyboard(x, y);
        }
    }



    private void UpdateSelection()
    {
        // Resetear todos
        foreach (GameObject key in keys)
        {
            TMP_Text text = key.GetComponent<TMP_Text>();

            text.color = Color.black;
        }

        //Resetear actions
        foreach (GameObject action in actionKeys)
        {
            TMP_Text text = action.GetComponent<TMP_Text>();

            text.color = Color.black;
        }


        // Seleccionar actual
        if (inActions)
        {
            TMP_Text text =
                actionKeys[currentAction].GetComponent<TMP_Text>();

            text.color = Color.white;
        }
        else
        {
            GameObject selectedKey = keys[currentY, currentX];

            TMP_Text text =
                selectedKey.GetComponent<TMP_Text>();

            text.color = Color.white;
        }
    }


    private void PressKey()
    {
        GameObject selectedKey = keys[currentY, currentX];

        TMP_Text keyText = selectedKey.GetComponentInChildren<TMP_Text>();

        string value = keyText.text;

        if (inActions)
        {
            switch (currentAction)
            {
                case 0:
                    DeleteChar();
                    break;
                case 1:

                    if (playerNameInput.text.Length > 0)
                    {
                        SaveController.playerName = playerNameInput.text;

                        SceneManager.LoadScene("Game");
                        Debug.Log("END");
                    }
                    else
                    {
                        //Play error SFX
                    }
                    break;
                case 2:
                    break;
            }
        }
        else
        {
            if (playerNameInput.text.Length < charLimit)
            {
                playerNameInput.text += value;
            }
        }
    }


    public void Navigate(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();


        if (ctx.performed)
        {
            if (input.x > 0)
            {
                Move(1, 0);
            }
            else if (input.x < 0)
            {
                Move(-1, 0);
            }

            if (input.y > 0)
            {
                Move(0, -1);
            }
            else if (input.y < 0)
            {
                Move(0, 1);
            }
        }

    }


    public void Submit()
    {
            PressKey();
    }


    public void Cancel()
    {

        playerNamePanel.SetActive(false);
        playerNameInput.text = "";

    }


    public void DeleteChar()
    {
        if(playerNameInput.text.Length > 0)
            playerNameInput.text = playerNameInput.text.Substring(0, playerNameInput.text.Length - 1);
    }
}
