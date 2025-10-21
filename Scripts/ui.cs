using Godot;
using System.Collections.Generic;

// define partial class for ui inheriting from canv
public partial class UI : CanvasLayer
{
	
	// export path
	[Export]
	public NodePath FinishLinePath;
	
	// ref FinishLine script
	// ref heart icons
	// ref the container for win UI
	// ref remaining heart label
	// ref play again button
	private FinishLine _finishLine;
	private List<TextureRect> _hearts;
	private Control _gameWin;
	private Label _heartsLeft;
	private Button _playAgain;
	
	public override void _Ready()
	{
		// initialize list with 3 hearts
		_hearts = new()
		{
			GetNode<TextureRect>("HPContainer/Heart1"),
			GetNode<TextureRect>("HPContainer/Heart2"),
			GetNode<TextureRect>("HPContainer/Heart3"),
		};
		
		// get UI container
		// get label
		// get button
		// get goal node
		_gameWin = GetNode<Control>("GameWin");
		_heartsLeft = _gameWin.GetNode<Label>("VBoxContainer/HeartsLeft");
		_playAgain = _gameWin.GetNode<Button>("VBoxContainer/PlayAgain");
		_finishLine = GetNode<FinishLine>(FinishLinePath);
		
		// hide win ui at start
		// when button is pressed, call game reset through finish line script
		_gameWin.Visible = false;
		_playAgain.Pressed += () => _finishLine.OnPlayAgainPressed();
	}
	
	// only shows amount of hearts player has
	public void UpdateHearts(int hp)
	{
		for (int i = 0; i < _hearts.Count; i++)
		{
			_hearts[i].Visible = i < hp;
		}
	}
	
	// make win ui visible
	// fill remaining hearts label
	// pause the game 
	public void ShowWinScreen(int heartsRemaining)
	{
		_gameWin.Visible = true;
		_heartsLeft.Text = $"You Won with {heartsRemaining} heart{(heartsRemaining == 1 ? "" : "s")} left!";
		GetTree().Paused = true;
	}
}
