using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.IAJ.Unity.Util;
using Random = UnityEngine.Random;

public class SceneManager : MonoBehaviour
{
    public const float X_WORLD_SIZE = 55;
    public const float Z_WORLD_SIZE = 32.5f;
    public const float AVOID_MARGIN = 4.0f;
    public const float MAX_SPEED = 5.0f;
    public const float MAX_ACCELERATION = 40.0f;
    public const float DRAG = 0.1f;

    public GameObject mainCharacterGameObject;
    public GameObject secondaryCharacterGameObject;
    public Text movementText;

    private BlendedMovement Blended { get; set; }
    private PriorityMovement Priority { get; set; }

    private MainCharacterController mainCharacterController;
    private List<MainCharacterController> characterControllers;
 

	// Use this for initialization
	void Start () 
	{
        this.mainCharacterController = this.mainCharacterGameObject.GetComponent<MainCharacterController>();

		var textObj = GameObject.Find ("InstructionsText");
		if (textObj != null) 
		{
			textObj.GetComponent<Text>().text = 
				"Instructions\n\n" +
				this.mainCharacterController.blendedKey + " - Blended\n" +
				this.mainCharacterController.priorityKey + " - Priority\n"+
                this.mainCharacterController.stopKey + " - Stop"; 
		}
	    
        
	    var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

	    this.characterControllers = this.CloneCharacters(this.mainCharacterGameObject, 50, obstacles);
        this.characterControllers.Add(this.mainCharacterGameObject.GetComponent<MainCharacterController>());

        //LINQ expression with a lambda function, returns an array with the DynamicCharacter for each character controler
        var characters = this.characterControllers.Select(cc => cc.character).ToList();
        //add the character corresponding to the main character
        characters.Add(this.mainCharacterController.character);

        //initialize all characters
	    foreach (var characterController in this.characterControllers)
	    {
            characterController.InitializeMovement(obstacles, characters);
	    }
	}

    private List<MainCharacterController> CloneCharacters(GameObject objectToClone,int numberOfCharacters, GameObject[] obstacles)
    {
        var characters = new List<MainCharacterController>();
        var deltaColor = 1.0f / numberOfCharacters;
        var color = 0.0f + deltaColor;
        var deltaAngle = MathConstants.MATH_2PI / numberOfCharacters;
        var angle = 0.0f + deltaAngle;

        for (int i = 1; i < numberOfCharacters; i++)
        {
            var clone = GameObject.Instantiate(objectToClone);
            var renderer = clone.GetComponent<Renderer>();

            renderer.material.SetColor("_Color", new Color(1-color,1-color,color));
            color += deltaColor;

            var characterController = clone.GetComponent<MainCharacterController>();
            characterController.character.KinematicData.Position = new Vector3(Mathf.Sin(angle)*30, 0, -Mathf.Cos(angle)*30);
            angle += deltaAngle;
            
            characters.Add(characterController);
        }

        return characters;
    }

    public void Update()
    {
        if(this.mainCharacterController.character.Movement != null)
        {
            movementText.text = "Movement:\n" + this.mainCharacterController.character.Movement.Name;
        }
        else
        {
            movementText.text = "Movement:\n ---";
        }
    }
}
