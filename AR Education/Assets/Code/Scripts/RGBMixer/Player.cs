using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Drawing;
using TMPro;
using Unity.Mathematics;
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
	[SerializeField] private TextMeshProUGUI targetColorText;
	[SerializeField] private TextMeshProUGUI currentColorText;


	[SerializeField] private GameObject[] vumarks;

	[SerializeField] private List<Color> colorMixerList = new List<Color>();

	[SerializeField] private int minMixerSteps;
	[SerializeField] private int maxMixerSteps;

	private List<Color> startingColors = new List<Color>();
	private float similarity;


	// Start is called before the first frame update
	void Start()
    {
		StartNewLevel();

		CheckWinCondition();

		updateUI();
    }

	/// <summary>
	/// Starts new random level
	/// </summary>
	public void StartNewLevel()
	{
		// Randomize random colors for each vumark
		startingColors = RandomizeStartingColors(vumarks.Length);

		GenerateTargetColor(minMixerSteps, maxMixerSteps);

		similarity = CompareColors(targetColor, currentColor);

		ResetMixer();
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

		targetColorText.text = "#" + UnityEngine.ColorUtility.ToHtmlStringRGB(targetColor);
		


		CalculateColorAndSimilarity();

		if (colorMixerList.Count==0)
		{
			currentColorImage.color = Color.white;
			currentColorText.text = "#" + UnityEngine.ColorUtility.ToHtmlStringRGB(Color.white);

			similarityValue.text = "0%";
		}
		else
		{
			currentColorImage.color = currentColor;
			currentColorText.text = "#" + UnityEngine.ColorUtility.ToHtmlStringRGB(currentColor);

			similarityValue.text = Math.Round(CompareColors(targetColor, currentColor), 0).ToString() + "%";
		}
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
			currentColor = MixColorsFromList(colorMixerList);
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
	private Color MixColorsFromList(List<Color> colorsList)
	{
		// if list has 1 element return it
		if (colorsList.Count == 1)
		{
			return colorsList[0];
		}
		else
		{
			
			float sumR =0, sumG=0, sumB=0;

			// mixed color is average R, G, and B value of all its elements
			foreach (Color color in colorsList)
			{
				sumR += color.r;
				sumG += color.g;
				sumB += color.b;
			}
			sumR/= colorsList.Count; 
			sumG/= colorsList.Count;
			sumB/= colorsList.Count;

			return new Color(sumR, sumG, sumB);
		}
	}


	/// <summary>
	/// Generates random target color from initial colors
	/// </summary>
	/// <param name="minSteps">Min amount of steps during generating color</param>
	/// <param name="maxSteps">Max amount of steps during generating color</param>
	private void GenerateTargetColor(int minSteps, int maxSteps)
	{
		int steps = UnityEngine.Random.Range(minSteps, maxSteps);

		List<Color> targetList= new List<Color>();

		// create list of randomly picked colors from starting colors
		for(int i = 0; i < steps; i++)
		{
			targetList.Add(startingColors[UnityEngine.Random.Range(0, startingColors.Count)]);
		}

		targetColor = MixColorsFromList(targetList);
	}


	/// <summary>
	/// Resets whole level
	/// </summary>
	public void ResetMixer()
	{
		winImage.gameObject.SetActive(false);

		// if color list contaions elements reset all
		if (colorMixerList.Count != 0)
		{
			colorMixerList.Clear();

			foreach (GameObject vumark in vumarks)
			{
				if (vumark != null)
				{
					vumark.transform.GetChild(0).GetComponent<ColorDisplayController>().ResetColor();
				}
			}

			Debug.Log("Reseted whole level");
			updateUI();
			CheckWinCondition();

		}	
	}

	


	/// <summary>
	/// Randomizes starting colors
	/// </summary>
	/// <param name="count"> Amount of colors to be generated </param>
	/// <returns> List of generated random colors </returns>
	private List<Color> RandomizeStartingColors(int count)
	{
		List<Color> colors = new List<Color>();

		while(colors.Count < count) 
		{
			Color newColor = GenerateRandomColor();

			// if color is true black then randomize again
			if(newColor.r == 0f && newColor.g == 0f && newColor.b == 0f)
			{
				continue;
			}

			colors.Add(newColor);
		}

		// set starting color for all vumarks
		for (int i = 0;i<colors.Count;i++)
		{
			vumarks[i].transform.GetChild(0).GetComponent<ColorDisplayController>().SetColor(colors[i]);
		}

		return colors;
	}


	/// <summary>
	/// Generates random RGB color
	/// </summary>
	/// <returns> random generated RGB color</returns>
	private Color GenerateRandomColor()
	{
		float r = UnityEngine.Random.Range(0f, 1f);
		float g = UnityEngine.Random.Range(0f, 1f);
		float b = UnityEngine.Random.Range(0f, 1f);
		return new Color(r, g, b);	
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
