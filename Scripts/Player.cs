using Godot;

// define a partial player class inheriting from char2d
public partial class Player : CharacterBody2D
{
	// export/set speed and max health variables
	// var to track player hp
	[Export]
	public float Speed = 200f;
	[Export]
	public int MaxHealth = 3;
	public int CurrentHP;

	// reference anim2d for animations
	// ref particles that play on death
	// ref ui script to update hp
	private AnimatedSprite2D _anim;
	private CpuParticles2D _deathParticles;
	private UI _ui;

	public override void _Ready()
	{
		// reset hp to max
		ResetHealth();
		// store sprite for animations
		// store particles for death
		// store UI node
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_deathParticles = GetNode<CpuParticles2D>("DeathParticles");
		_ui = GetTree().Root.GetNode<UI>("Main/UI");
		// update ui with current hp
		_ui.UpdateHearts(CurrentHP);
	}

	public override void _PhysicsProcess(double delta)
	{
		// get current player velocity
		Vector2 v = Velocity;

		// get hor and ver input
		float inputX = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		float inputY = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
		
		// input of the combined inputs
		Vector2 input = new Vector2(inputX, inputY);
		
		// normalize diagonal input
		if (input.Length > 1)
		{
			input = input.Normalized();
		}
		
		// calc movement vector
		v = input * Speed;

		// set the velocity
		// apply movement to player
		Velocity = v;
		MoveAndSlide();
		
		// checks collisions
		// if collision is with enemy, take hp
		for (int i =0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			var collider = collision.GetCollider();
			
			if (collider is Enemy)
			{
				TakeDamage();
				break;
			}
		}
		
		// check movement dir and play corresponding anim
		if (_anim != null)
		{
			if (input.Length > 0.01f)
			{
				if (Mathf.Abs(inputX) > Mathf.Abs(inputY))
				{
					if (inputX < 0)
					{
						_anim.Play("Left_A");
					}
					else
					{
						_anim.Play("Right_D");
					}
				}
				else
				{
					if (inputY < 0)
					{
						_anim.Play("Up_W");
					}
					else
					{
						_anim.Play("Down_S");
					}
				}
			}
			else
			{
				_anim.Play("Idle");
			}
		}
	}
	
	// reduce player hp
	// update UI hp
	// kill player if hp is empty
	private void TakeDamage()
	{
		CurrentHP--;
		_ui.UpdateHearts(CurrentHP);
		
		if (CurrentHP <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		// hide player
		// disable physics
		Visible = false;
		SetPhysicsProcess(false);
		
		// display death particles
		_deathParticles.Emitting = true;
		
		// create temp timer for respawn
		var deathTimer = new Timer();
		deathTimer.WaitTime = 1.0f;
		deathTimer.OneShot = true;
		AddChild(deathTimer);
		deathTimer.Timeout += () =>
		{
			// reset the player
			Reset(_startPosition);
		};
		deathTimer.Start();
	}
	
	// reset hp to max
	public void ResetHealth()
	{
		CurrentHP = MaxHealth;
	}
	
	public void Reset(Vector2 startPosition)
	{
		// reset player pos
		// reset hp
		// make visible and allow physics
		// reset velocity
		GlobalPosition = startPosition;
		ResetHealth();
		Visible = true;
		SetPhysicsProcess(true);
		Velocity = Vector2.Zero;
		
		// idle animation if not already
		if (_anim != null)
		{
			_anim.Play("Idle");
		}
		
		// remove particles if needed
		if (_deathParticles != null)
		{
			_deathParticles.Emitting = false;
		}
	}
}
