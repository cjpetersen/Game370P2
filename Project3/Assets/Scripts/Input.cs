using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input : MonoBehaviour
{
	public InputField inputField;
	public List<string> articles, prepositions;

	Manager m;

	void Awake()
	{
		string[] temp;
		m = GetComponent<Manager>();
		inputField.onEndEdit.AddListener(AcceptStringInput);
		temp = System.IO.File.ReadAllLines("Assets/Resources/Lists/Articles.txt");
		for (int i = 0; i < temp.Length; i++)
			articles.Add(temp[i].ToLower());
		temp = System.IO.File.ReadAllLines("Assets/Resources/Lists/Prepositions.txt");
		for (int i = 0; i < temp.Length; i++)
			prepositions.Add(temp[i].ToLower());
	}

	void AcceptStringInput(string userInput)
	{
		m.AddToLog(userInput);
		userInput = userInput.ToLower();

		char[] delimiterCharacters = { ' ' };
		List<string> separatedInputWords = new List<string>();
		string[] temp = userInput.Split(delimiterCharacters);
		for (int i = 0; i < temp.Length; i++)
			if (!articles.Contains(temp[i]) && !prepositions.Contains(temp[i]))
				separatedInputWords.Add(temp[i]);

		for (int i = 0; i < m.actions.Count; i++)
		{
			if (separatedInputWords[0] == m.actions[i])
			{
				m.TakeAction(separatedInputWords);
			}
		}

		m.DisplayLoggedText();
		inputField.ActivateInputField();
		inputField.text = null;
	}
}
