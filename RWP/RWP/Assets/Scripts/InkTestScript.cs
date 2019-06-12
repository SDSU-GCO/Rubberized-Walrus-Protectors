using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class InkTestScript : MonoBehaviour
{
    public string currentDialogueLine = "";

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
    [SerializeField]
    private TextAsset inkJSONAsset;
#pragma warning restore IDE0044, CS0649 // Add readonly modifier

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
    [SerializeField]
    private Canvas canvas;
#pragma warning restore IDE0044, CS0649 // Add readonly modifier
    // UI Prefabs
#pragma warning disable IDE0044, CS0649 // Add readonly modifier
    [SerializeField]
    private Text textPrefab;
#pragma warning restore IDE0044, CS0649 // Add readonly modifier

#pragma warning disable IDE0044, CS0649 // Add readonly modifier
    [SerializeField]
    private Button buttonPrefab;
#pragma warning restore IDE0044, CS0649 // Add readonly modifier
    private Story story;

    private List<Choice> currentChoices = new List<Choice>();

    private void Awake()
    {
        // Remove the default message
        RemoveChildren();
        StartStory();
    }

    // Creates a new Story object with the compiled story which we can then play!
    private void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        RefreshView();
    }

    // This is the main function called every time the story changes. It does a few things:
    // Destroys all the old content and choices.
    // Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
    private void RefreshView()
    {
        // Remove all the UI on screen
        RemoveChildren();

        // Read all the content until we can't continue any more
        while (story.canContinue)
        {
            // Continue gets the next line of the story
            string text = story.Continue();

            // This removes any white space from the text.
            currentDialogueLine = text;
            // Display the text on screen!
            CreateContentView(text);
        }

        // Display all the choices, if there are any!
        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                currentChoices.Add(choice);
                Button button = CreateChoiceView(choice.text.Trim());
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate
                {
                    OnClickChoiceButton(choice);
                });
            }
        }
        // If we've read all the content and there's no choices, the story is finished!
        else
        {
            Button choice = CreateChoiceView("End of story.\nRestart?");
            choice.onClick.AddListener(delegate
            {
                StartStory();
            });
        }
    }

    // When we click the choice button, tell the story to choose that choice!
    private void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    private void AdvanceStoryToChoice(Story story, Choice choice) => story.ChooseChoiceIndex(choice.index);

    // Creates a button showing the choice text
    private void CreateContentView(string text)
    {
        Text storyText = Instantiate(textPrefab) as Text;
        storyText.text = text;
        storyText.transform.SetParent(canvas.transform, false);
    }

    // Creates a button showing the choice text
    private Button CreateChoiceView(string text)
    {
        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(canvas.transform, false);

        // Gets the text from the button prefab
        Text choiceText = choice.GetComponentInChildren<Text>();
        choiceText.text = text;

        // Make the button expand to fit the text
        HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childForceExpandHeight = false;

        return choice;
    }

    // Destroys all the children of this gameobject (all the UI)
    private void RemoveChildren()
    {
        currentChoices.Clear();
        int childCount = canvas.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(canvas.transform.GetChild(0).gameObject);
        }
    }
}