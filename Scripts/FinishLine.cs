using Godot;

// define partial class for goal inheriting from area2d
public partial class FinishLine : Area2D
{
	// export paths
	[Export] 
	public NodePath gameWinPath;
	[Export] 
	public NodePath playerPath;
	[Export] 
	public NodePath enemyPath;
	[Export]
	public NodePath UIPath;
	
	// ref control for win
	// ref node for player
	// ref node for enemy
	// link ui to ui script
	private Control _gameWin;
	private CharacterBody2D _player;
	private CharacterBody2D _enemy;
	private UI _ui;
	
	// store ref to start pos
	private Vector2 _playerStartPos;
	private Vector2 _enemyStartPos;
	
	public override void _Ready()
	{
		// connect signal to OnBodyEntered
		// get node refs for paths
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		_gameWin = GetNode<Control>(gameWinPath);
		_player = GetNode<Node2D>(playerPath);
		_enemy = GetNode<Node2D>(enemyPath);
		_ui = GetNode<UI>(UIPath);
		
		// store initial pos of player and enemy
		_playerStartPos = _player.GlobalPosition;
		_enemyStartPos = _enemy.GlobalPosition;
	}
	
	// when body enters area, check for player, if yes trigger win
	private void OnBodyEntered(Node body)
	{
		if (body == _player)
		{
			ShowWinScreen();
		}
	}
	
	private void ShowWinScreen()
	{
		// show win UI
		// Pause game
		_gameWin.Visible = true;
		getTree().Paused = true;
		
		// if player is player obj
		// show win screen and pass current hp
		if (_player is Player playerscript)
		{
			_ui.ShowWinScreen(playerscript.CurrentHP);
		}
	}
	
	public void OnPlayAgainPressed()
	{
		// unpause the game
		// hide win ui
		GetTree().Paused = false;
		_gameWin.Visible = false;
		
		// if player is player obj:
		// reset player
		if (_player is Player playerscript)
		{
			playerscript.Reset(_playerStartPos);
		}
		// if enemy is enemy obj:
		// reset enemy
		if (_enemy is Enemy enemyscript)
		{
			enemyscript.Reset(_enemyStartPos);
		}
		
		// reset hp to max
		_ui.UpdateHearts(3);
	}
}
