using UnityEngine;
using System.Collections;
 
public class MenuSlide : MonoBehaviour {
	public GameObject pauseMenuPanel;
	private Animator anim;
	private bool isPaused = false;


	void Start () {
		anim = pauseMenuPanel.GetComponent<Animator>();
		//disable it on start to stop it from playing the default animation
		anim.enabled = false;
	}
	
	//function to pause the game
	public void Unfold(){
		anim.enabled = true;
		anim.Play("MiniCamSlideIn");
		isPaused = true;


	}
	public void Fold(){
		isPaused = false;
		anim.Play("MiniCamSlideOut");

	}
	
}
