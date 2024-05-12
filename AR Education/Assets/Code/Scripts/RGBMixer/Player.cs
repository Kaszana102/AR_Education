using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	[Range(0f, 100f)]
	[SerializeField] private float thresholdPercentage;

	[SerializeField] private Color targetColor;
	[SerializeField] private Color currentColor;


	[SerializeField] private Image targetColorImage;
    [SerializeField] private Image currentColorImage;
    [SerializeField] private Image winImage;
	[SerializeField] private TextMeshProUGUI similarityValue;

	private float similarity;

	int mixedColors = 0;


	// Start is called before the first frame update
	void Start()
    {
		similarity = CompareColors(targetColor, currentColor);
		targetColorImage.color = targetColor;
		currentColorImage.color = currentColor;

		winImage.gameObject.SetActive(false);

		CheckWinCondition();

		updateUI();
    }

	/// <summary>
	/// Compares 2 RGB colors based on their distance on RGB space
	/// </summary>
	/// <param name="color1"></param>
	/// <param name="color2"></param>
	/// <returns> Returns similarity of given colors as % (0% - different, 100% exactly the same)</returns>
	private float CompareColors(Color color1, Color color2)
	{
		// calcute differences
		float diffR = Mathf.Abs(color1.r - color2.r);
		float diffG = Mathf.Abs(color1.g - color2.g);
		float diffB = Mathf.Abs(color1.b - color2.b);

		// calculate distance squared
		float distanceSquared = diffR * diffR + diffG * diffG + diffB * diffB;

		// sqrt distance
		float distance = Mathf.Sqrt(distanceSquared);

		// normalize result, max distance is sqrt(3)
		float normalizedDistance = distance / Mathf.Sqrt(3f);

		// convert to % and reverse it
		float similarityPercent = (1f - normalizedDistance) * 100f;

		return similarityPercent;
	}


	/// <summary>
	/// Updates UI
	/// </summary>
	private void updateUI()
	{
		targetColorImage.color = targetColor;

		if (mixedColors==0)
		{
			currentColorImage.color = Color.white;
			similarityValue.text = "0%";
		}
		else
		{
			currentColorImage.color = currentColor;
			similarityValue.text = Math.Round(CompareColors(targetColor, currentColor), 0).ToString() + "%";
		}
		
	}

	
	/// <summary>
	/// Mixes new color with current one
	/// </summary>
	/// <param name="newColor"> new color to be added to mix</param>
	public void MixColors(Color newColor)
	{
		if (mixedColors == 0)
		{
			currentColor = newColor;
		}
		else
		{
			float red = ((currentColor.r + newColor.r) / 2.0f);
			float green = ((currentColor.g + newColor.g) / 2.0f);
			float blue = ((currentColor.b + newColor.b) / 2.0f);

			currentColor = new Color(red, green, blue);
		}
		similarity = CompareColors(targetColor, currentColor);
		mixedColors++;

		updateUI();
		CheckWinCondition();	
	}


	/// <summary>
	/// Resets whole level
	/// </summary>
	public void ResetMixer()
	{
		currentColor = Color.white;
		mixedColors = 0;

		updateUI();
	}


	/// <summary>
	/// Checks if win condition is satisfied
	/// </summary>
	private void CheckWinCondition()
	{
		// if color is accurate enough
		if (similarity > thresholdPercentage)
		{
			winImage.gameObject.SetActive(true);
			Debug.Log("You have won!!!");
			updateUI();
		}
	}

	/// <summary>
	///  Changes scene and goes back to main menu
	/// </summary>
	public void GoBackToMenu()
    {
		//go back to main menu
		LevelLoader.LoadMainMenu();
    }

}
