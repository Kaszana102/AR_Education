using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

	[SerializeField] private GameObject[] vumarks;

	//[SerializeField] private GameObject UIOverlay;

	[SerializeField] private List<Color> colorMixerList = new List<Color>();
	[SerializeField] private string currentColorText;

	private float similarity;


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

		CalculateColorAndSimilarity();

		if (colorMixerList.Count==0)
		{
			currentColorImage.color = Color.white;
			similarityValue.text = "0%";
		}
		else
		{
			currentColorImage.color = currentColor;
			similarityValue.text = Math.Round(CompareColors(targetColor, currentColor), 0).ToString() + "%";
		}
		currentColorText = UnityEngine.ColorUtility.ToHtmlStringRGB(currentColor);
	}

	
	/// <summary>
	/// Mixes new color with current one
	/// </summary>
	/// <param name="newColor"> new color to be added to mix</param>
	public void AddColor(Color newColor)
	{
		colorMixerList.Add(newColor);
		Debug.Log("Added color #" + UnityEngine.ColorUtility.ToHtmlStringRGB(newColor) + " to mixer");

		updateUI();
		CheckWinCondition();	
	}

	/// <summary>
	/// Removes color from mixer if it exists
	/// </summary>
	/// <param name="colorToRemove">Color to be removed</param>
	public void RemoveColor(Color colorToRemove)
	{
		if (colorMixerList.Count != 0 && colorMixerList.Contains(colorToRemove))
		{
			colorMixerList.Remove(colorToRemove);
			Debug.Log("Removed color #" + UnityEngine.ColorUtility.ToHtmlStringRGB(colorToRemove) + " from mixer");

			updateUI();
			CheckWinCondition();
		}
	}

	/// <summary>
	/// Calculates current color and it's similarity to target one
	/// </summary>
	private void CalculateColorAndSimilarity()
	{
		// if list is empty
		if(colorMixerList.Count==0)
		{
			currentColor = Color.white;
		}
		// if list contains any colors
		else 
		{ 
			currentColor = MixColorsFromList();
			similarity = CompareColors(targetColor, currentColor);
		}

	}

	/// <summary>
	/// Mixes 2 colors 
	/// </summary>
	/// <param name="color1"></param>
	/// <param name="color2"></param>
	/// <returns>Return color mix</returns>
	private Color MixColors(Color color1, Color color2)
	{
		float red = ((color1.r + color2.r) / 2.0f);
		float green = ((color1.g + color2.g) / 2.0f);
		float blue = ((color1.b + color2.b) / 2.0f);

		return new Color(red, green, blue);
	}



	/// <summary>
	/// Mixes all colors from list
	/// </summary>
	/// <returns>Mixed color from list</returns>
	private Color MixColorsFromList()
	{

		if (colorMixerList.Count == 1)
		{
			return colorMixerList[0];
		}
		else
		{
			/*Color colorFromList = colorMixerList[0];

			for (int i = 1; i < colorMixerList.Count; i++)
			{
				colorFromList = MixColors(colorFromList, colorMixerList[i]);
			}
			return colorFromList;
			*/
			float sumR =0, sumG=0, sumB=0;

			foreach (Color color in colorMixerList)
			{
				sumR += color.r;
				sumG += color.g;
				sumB += color.b;
			}
			sumR/= colorMixerList.Count; 
			sumG/= colorMixerList.Count;
			sumB/= colorMixerList.Count;

			return new Color(sumR, sumG, sumB);
		}
	}


	/// <summary>
	/// Resets whole level
	/// </summary>
	public void ResetMixer()
	{
		colorMixerList.Clear();

		foreach(GameObject vumark in vumarks)
		{
			if(vumark != null)
			{ 
				vumark.gameObject.GetComponent<ColorDisplayController>().ResetColor();
			}
			
		}

		Debug.Log("Reseted whole level");
		updateUI();
		CheckWinCondition();
	}


	/// <summary>
	/// Checks if win condition is satisfied
	/// </summary>
	private void CheckWinCondition()
	{
		// if color is accurate enough
		if (similarity > thresholdPercentage && colorMixerList.Count>1)
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
