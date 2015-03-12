﻿using System;
using System.Collections.Generic;
using UnityEngine;

// displays all the current stuff for the players
public class GameUI : MonoBehaviour
{
	Level level;
	Panel panel;
	public void Setup(Level _level){
		panel = Panel.Create();
		level = _level;
	}
	public void Update(){
		float vPosition = 0.35f;
		foreach (RLCharacter c in level.players) {
			VectorGui.SetPosition (new Vector2(10.65f, vPosition));
			vPosition -= 1;
			Color color = Color.white;
			if (c == level.currentPlayer)
				color = GameColors.GetColor("player");
			VectorGui.Label("HP:"+c.healthPoints, 0.1f, color);
			VectorGui.Label("AP:"+c.actionPoints, 0.1f, color);

			VectorGui.SetPosition (new Vector2 (10.65f, vPosition));
			// display the powers the player can use
			for (int i = 0; i < c.powerups.Count; i++) {
				VectorGui.Label ((i+1)+":"+c.powerups[i].DisplayText(), 0.1f, color);
			}
			vPosition -= 2;
		}
		// determine if there is a character underneath the cursor, and display the range highlights if so
		Vector2i mp = Grid.WorldToGrid (Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y)));
		if (
			mp.x >= 0 && mp.x < level.sx
			&& mp.y >= 0 && mp.y < level.sy
		){
			if (level.playerMap [mp.x, mp.y] != null) {
				level.playerMap [mp.x, mp.y].DisplayFireRadius (level.map, level.monsterMap, level.highlights);
			} else if(level.monsterMap [mp.x, mp.y] != null) {
				level.monsterMap [mp.x, mp.y].DisplayFireRadius (level.map, level.playerMap, level.highlights);
			}else {
				level.HideHighlights ();
			}
		}
	}
	public void AddToPanelAndTransform(DisplayElement de){
		panel.Add(de);
		de.transform.SetParent (transform);
	}
	void OnDestroy(){
		foreach (DisplayElement de in panel.elements) {
			if(de.enabled)
				de.Destroy ();
		}
	}
}