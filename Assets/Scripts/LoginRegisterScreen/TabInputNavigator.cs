using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TabInputNavigator : MonoBehaviour
{
    public TMP_InputField[] inputFields;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;
            bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            for (int i = 0; i < inputFields.Length; i++)
            {
                if (current == inputFields[i].gameObject)
                {
                    int nextIndex;

                    if (shift)
                        nextIndex = (i - 1 + inputFields.Length) % inputFields.Length;
                    else
                        nextIndex = (i + 1) % inputFields.Length;

                    inputFields[nextIndex].ActivateInputField();
                    EventSystem.current.SetSelectedGameObject(inputFields[nextIndex].gameObject);
                    break;
                }
            }
        }
    }
}
