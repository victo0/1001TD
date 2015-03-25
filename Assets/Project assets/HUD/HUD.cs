using UnityEngine;
using System.Collections;
//Ces deux importations sont necessaires pour les fonctions utilisées dans mes codes.
using System.Collections.Generic;
using TD;

public class HUD : MonoBehaviour {

	//Importation des textures via l'inspector unity.
	public GUISkin resourceSkin, ordersSkin, selectBoxSkin, mouseCursorSkin;
	public Texture2D activeCursor;
	public Texture2D selectCursor, leftCursor, rightCursor, upCursor, downCursor;
	public Texture2D[] moveCursors, attackCursors, harvestCursors;
	public Texture2D[] resources;
	public Texture2D healthy, damaged, critical;
	public Texture2D buttonHover, buttonClick;
	public Texture2D buildFrame, buildMask;	

	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
	private const int SELECTION_NAME_HEIGHT = 15, SCROLL_BAR_WIDTH = 22, BUTTON_SPACING = 7;
	private Player player;

	private CursorState activeCursorState;
	private int currentFrame = 0, buildAreaHeight = 0;

	private Dictionary< ResourceType, int > resourceValues, resourceLimits;
	private Dictionary< ResourceType, Texture2D > resourceImages;
	private const int ICON_WIDTH = 32, ICON_HEIGHT = 32, TEXT_WIDTH = 128, TEXT_HEIGHT = 32;

	private WorldObject lastSelection;
	private float sliderValue;


	private const int BUILD_IMAGE_WIDTH = 64, BUILD_IMAGE_HEIGHT = 64;

	private const int BUILD_IMAGE_PADDING = 8;


	public void Start () {
		player = transform.root.GetComponent < Player >(); //obtention du joueur et de ses paramètres.
		resourceValues = new Dictionary< ResourceType, int >();
		resourceLimits = new Dictionary< ResourceType, int >();
		resourceImages = new Dictionary< ResourceType, Texture2D >();
		ResourceManager.StoreSelectBoxItems(selectBoxSkin, healthy, damaged, critical);
		for(int i = 0; i < resources.Length; i++) {
			switch(resources[i].name) { //Creation des différentes ressources, permet de les afficher.
			case "Money":
				resourceImages.Add(ResourceType.Money, resources[i]);
				resourceValues.Add(ResourceType.Money, 0);
				resourceLimits.Add(ResourceType.Money, 0);
				break;
			case "Power":
				resourceImages.Add(ResourceType.Power, resources[i]);
				resourceValues.Add(ResourceType.Power, 0);
				resourceLimits.Add(ResourceType.Power, 0);
				break;
			default: break;
			}
		}
		buildAreaHeight = Screen.height - RESOURCE_BAR_HEIGHT - SELECTION_NAME_HEIGHT - 2 * BUTTON_SPACING;
		ResourceManager.StoreSelectBoxItems(selectBoxSkin);
		SetCursorState(CursorState.Select);
	}
	
	public void OnGUI () {
		if (player && player.human) { //si le joueur est en train de jouer et si c'est un humain : dessiner le HUD.
			DrawOrdersBar ();
			DrawResourcesBar ();
			DrawMouseCursor();
		}
	}
	private void DrawOrdersBar() { //Dessine la barre de HUD de droite.
		GUI.skin = ordersSkin;
			GUI.BeginGroup(new Rect(Screen.width-ORDERS_BAR_WIDTH-BUILD_IMAGE_WIDTH,RESOURCE_BAR_HEIGHT,ORDERS_BAR_WIDTH+BUILD_IMAGE_WIDTH,Screen.height-RESOURCE_BAR_HEIGHT));
			GUI.Box(new Rect(BUILD_IMAGE_WIDTH+SCROLL_BAR_WIDTH,0,ORDERS_BAR_WIDTH,Screen.height-RESOURCE_BAR_HEIGHT),"");
		string selectionName = "";
		if(player.SelectedObject) {
			selectionName = player.SelectedObject.objectName;
			if(player.SelectedObject.IsOwnedBy(player)){
				if(lastSelection && lastSelection != player.SelectedObject) {
					sliderValue = 0.0f;
				}
				DrawActions(player.SelectedObject.GetActions());
				lastSelection = player.SelectedObject;
				Building selectedBuilding = lastSelection.GetComponent<Building>();
					if(selectedBuilding) {
						DrawBuildQueue(selectedBuilding.getBuildQueueValues(), selectedBuilding.getBuildPercentage());
					}
			}
		}
		if(!selectionName.Equals("")) {
			int leftPos = BUILD_IMAGE_WIDTH + SCROLL_BAR_WIDTH / 2;
			int topPos = buildAreaHeight + BUTTON_SPACING;
			GUI.Label(new Rect(leftPos,topPos,ORDERS_BAR_WIDTH,SELECTION_NAME_HEIGHT), selectionName);
		}
		GUI.EndGroup();
	}
	private void DrawResourcesBar() { //Dessine la barre de HUD du haut.
		GUI.skin = resourceSkin;
		GUI.BeginGroup (new Rect (0, 0, Screen.width, RESOURCE_BAR_HEIGHT));
		GUI.Box (new Rect (0, 0, Screen.width, RESOURCE_BAR_HEIGHT), "");

		int topPos = 4, iconLeft = 4, textLeft = 20;
		DrawResourceIcon(ResourceType.Money, iconLeft, textLeft, topPos);
		iconLeft += TEXT_WIDTH;
		textLeft += TEXT_WIDTH;
		DrawResourceIcon(ResourceType.Power, iconLeft, textLeft, topPos);

		GUI.EndGroup ();
	}


	private void DrawResourceIcon(ResourceType type, int iconLeft, int textLeft, int topPos) { //Sers à dessiner les icones des ressources.
		Texture2D icon = resourceImages[type];
		string text = resourceValues[type].ToString() + "/" + resourceLimits[type].ToString();
		GUI.DrawTexture(new Rect(iconLeft, topPos, ICON_WIDTH, ICON_HEIGHT), icon);
		GUI.Label (new Rect(textLeft, topPos, TEXT_WIDTH, TEXT_HEIGHT), text);
	}
	
	public bool MouseInBounds() { // Définis si la souris est ou non dans le HUD.
		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
		bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;
		return insideWidth && insideHeight;
	}

	public Rect GetPlayingArea() { // Récupère la "zone de jeu" (hors du HUD).
		return new Rect (0, RESOURCE_BAR_HEIGHT, Screen.width- ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT);
	
	}

	private void DrawMouseCursor() { // Dessine les curseurs personnalisés.
		bool mouseOverHud = !MouseInBounds() && activeCursorState != CursorState.PanRight && activeCursorState != CursorState.PanUp;
		if(mouseOverHud) {
			Screen.showCursor = true;
		} else {
			Screen.showCursor = false;
			if (!player.IsFindingBuildingLocation()) {
				GUI.skin = mouseCursorSkin;
				GUI.BeginGroup(new Rect(0,0,Screen.width,Screen.height));
				UpdateCursorAnimation();
				Rect cursorPosition = GetCursorDrawPosition();
				GUI.Label(cursorPosition, activeCursor);
				GUI.EndGroup();
			}
		}
	}

	private void UpdateCursorAnimation() { // Génère l'animation du curseur.
		if(activeCursorState == CursorState.Move) {
			currentFrame = (int)Time.time % moveCursors.Length;
			activeCursor = moveCursors[currentFrame];
		} else if(activeCursorState == CursorState.Attack) {
			currentFrame = (int)Time.time % attackCursors.Length;
			activeCursor = attackCursors[currentFrame];
		} else if(activeCursorState == CursorState.Harvest) {
			currentFrame = (int)Time.time % harvestCursors.Length;
			activeCursor = harvestCursors[currentFrame];
		}
	}

	private Rect GetCursorDrawPosition() { //ajuste la position du curseur en fonction de son type.
		float leftPos = Input.mousePosition.x;
		float topPos = Screen.height - Input.mousePosition.y;
		if(activeCursorState == CursorState.PanRight) leftPos = Screen.width - activeCursor.width;
		else if(activeCursorState == CursorState.PanDown) topPos = Screen.height - activeCursor.height;
		else if(activeCursorState == CursorState.Move || activeCursorState == CursorState.Select || activeCursorState == CursorState.Harvest) {
			topPos -= activeCursor.height / 2;
			leftPos -= activeCursor.width / 2;
		}
		return new Rect(leftPos, topPos, activeCursor.width, activeCursor.height);
	}

	public void SetCursorState(CursorState newState) { // Définis quel curseur/animation afficher.
		activeCursorState = newState;
		switch(newState) {
		case CursorState.Select:
			activeCursor = selectCursor;
			break;
		case CursorState.Attack:
			currentFrame = (int)Time.time % attackCursors.Length;
			activeCursor = attackCursors[currentFrame];
			break;
		case CursorState.Harvest:
			currentFrame = (int)Time.time % harvestCursors.Length;
			activeCursor = harvestCursors[currentFrame];
			break;
		case CursorState.Move:
			currentFrame = (int)Time.time % moveCursors.Length;
			activeCursor = moveCursors[currentFrame];
			break;
		case CursorState.PanLeft:
			activeCursor = leftCursor;
			break;
		case CursorState.PanRight:
			activeCursor = rightCursor;
			break;
		case CursorState.PanUp:
			activeCursor = upCursor;
			break;
		case CursorState.PanDown:
			activeCursor = downCursor;
			break;
		default: break;
		}
	}
	public void SetResourceValues(Dictionary< ResourceType, int > resourceValues, Dictionary< ResourceType, int > resourceLimits) {
		this.resourceValues = resourceValues;
		this.resourceLimits = resourceLimits;
	}
	private void DrawActions(string[] actions) { //Dessine les boutons d'actions dans la barre de droite.
		GUIStyle buttons = new GUIStyle();
		buttons.hover.background = buttonHover;
		buttons.active.background = buttonClick;
		GUI.skin.button = buttons;
		int numActions = actions.Length;
		GUI.BeginGroup(new Rect(BUILD_IMAGE_WIDTH, 0, ORDERS_BAR_WIDTH, buildAreaHeight));
		if(numActions >= MaxNumRows(buildAreaHeight)) DrawSlider(buildAreaHeight, numActions / 2.0f);
		for(int i = 0; i < numActions; i++) {
			int column = i % 2;
			int row = i / 2;
			Rect pos = GetButtonPos(row, column);
			Texture2D action = ResourceManager.GetBuildImage(actions[i]);
			if(action) {
				if(GUI.Button(pos, action)) {
					if(player.SelectedObject) player.SelectedObject.PerformAction(actions[i]);
				}
			}
		}
		GUI.EndGroup();
	}
	
	private int MaxNumRows(int areaHeight) {
		return areaHeight / BUILD_IMAGE_HEIGHT;
	}
	
	private Rect GetButtonPos(int row, int column) {
		int left = SCROLL_BAR_WIDTH + column * BUILD_IMAGE_WIDTH;
		float top = row * BUILD_IMAGE_HEIGHT - sliderValue * BUILD_IMAGE_HEIGHT;
		return new Rect(left, top, BUILD_IMAGE_WIDTH, BUILD_IMAGE_HEIGHT);
	}
	
	private void DrawSlider(int groupHeight, float numRows) { //dessine un slider pour les actions si necessaire.
		sliderValue = GUI.VerticalSlider(GetScrollPos(groupHeight), sliderValue, 0.0f, numRows - MaxNumRows(groupHeight));
	}
	private Rect GetScrollPos(int groupHeight) {
		return new Rect(BUTTON_SPACING, BUTTON_SPACING, SCROLL_BAR_WIDTH, groupHeight - 2 * BUTTON_SPACING);
	}
	private void DrawBuildQueue(string[] buildQueue, float buildPercentage) { // Dessine la file de production.
		for(int i = 0; i < buildQueue.Length; i++) {
			float topPos = i * BUILD_IMAGE_HEIGHT - (i+1) * BUILD_IMAGE_PADDING;
			Rect buildPos = new Rect(BUILD_IMAGE_PADDING, topPos, BUILD_IMAGE_WIDTH, BUILD_IMAGE_HEIGHT);
			GUI.DrawTexture(buildPos, ResourceManager.GetBuildImage(buildQueue[i]));
			GUI.DrawTexture(buildPos, buildFrame);
			topPos += BUILD_IMAGE_PADDING;
			float width = BUILD_IMAGE_WIDTH - 2 * BUILD_IMAGE_PADDING;
			float height = BUILD_IMAGE_HEIGHT - 2 * BUILD_IMAGE_PADDING;
			if(i==0) {
				topPos += height * buildPercentage;
				height *= (1 - buildPercentage);
			}
			GUI.DrawTexture(new Rect(2 * BUILD_IMAGE_PADDING, topPos, width, height), buildMask);
		}
	}
}	
