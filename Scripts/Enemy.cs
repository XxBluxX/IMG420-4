using Godot;

// define partial enemy class inheriting from char2d
public partial class Enemy : CharacterBody2D
{
	// export/set speed and target path
	[Export]
	public float Speed = 120f;
	[Export]
	public NodePath TargetPath;

	// ref navagent2d for navigation
	// ref player for target
	// ref timer for pause
	// ref animsprite2d for animations
	// track if enemy is paused
	private NavigationAgent2D _navAgent;
	private CharacterBody2D _target;
	private Timer _pauseTimer;
	private AnimatedSprite2D _anim;
	private bool _isPaused = false;

	public override void _Ready()
	{
		// get navagent for path finding
		// get timer for pausing
		// get sprite for animations
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_pauseTimer = GetNode<Timer>("PauseTimer");
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// takes assigned target path and gives it to _target
		// sets the target position to initiate path finding
		if (TargetPath != null)
		{
			_target = GetNode<Node2D>(TargetPath);
			_navAgent.TargetPosition = _target.GlobalPosition;
		}
		
		// connects signal to OnPauseTimeout
		_pauseTimer.Timeout += OnPauseTimeout;
		// plays idle animation
		_anim.Play("Idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		// if target or navagent dont exist, skip
		if (_target == null || _navAgent == null)
		{
			return;
		}
		// if enemy is paused:
		// reset velocity
		// play idle animation if not already playing
		if (_isPaused)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			
			if (_anim.CurrentAnimation != "Idle")
				_anim.Play("Idle");
				
			return;
		}
		
		// continuously update target pos
		_navAgent.TargetPosition = _target.GlobalPosition;
		
		// if enemy is within 5 px
		if (_navAgent.GetRemainingDistance() < 5f)
		{
			// reset velocity
			Velocity = Vector2.Zero;
			MoveAndSlide();
			
			// if it is not paused:
			// pause
			// start pause timer
			if (!_isPaused)
			{
				_isPaused = true;
				_pauseTimer.Start();
			}
			
			// play idle animation if not already playing
			if (_anim.CurrentAnimation != "Idle")
				_anim.Play("Idle");
			
			return;
		}

		// get next pos
		Vector2 nextPoint = _navAgent.GetNextPathPosition();

		// calc noramlized dir to nextPoint
		Vector2 direction = (nextPoint - GlobalPosition).Normalized();

		// calc velocity
		// call enemy movement
		Velocity = direction * Speed;
		MoveAndSlide();
		
		// if anim is not playing, play it
		if (_anim.CurrentAnimation != "Moving")
			_anim.Play("Moving");
	}
	
	// when pause timer completes:
	// unpause the enemy
	// update pos
	private void OnPauseTimeout()
	{
		_isPaused = false;
		if (_target != null)
		{
			_navAgent.TargetPosition = _target.GlobalPosition;
		}
	}
	
	public void Reset(Vector2 startPosition)
	{
		// move enemy to start position
		// reset velocity
		// unpause
		GlobalPosition = startPosition;
		Velocity = Vector2.Zero;
		_isPaused = false;
		
		// stop timer if running
		if (_pauseTimer != null && _pauseTimer.IsStopped() == false)
		{
			_pauseTimer.Stop();
		}
		
		// reset targeting
		if (_target != null)
		{
			_navAgent.TargetPosition = _target.GlobalPosition;
		}
		
		// play idle anim if not already
		if (_anim != null)
		{
			_anim.Play("Idle");
		}
		
		// reset physics to true
		SetPhysicsProcess(true);
	}
}
