<<<<<<< HEAD
# IMG420-4
## Tile-based world:
Used a tile sheet and godot tools to create a simple map that offers coliisions (the player/enemy may not "fall off" of the grass bridge into the water) and enemy navigation along the grass bridge targeting the player.
## Player character/Sprite animation:
The player character is made up of a AnimatedSprite2D, CollisionShape2D and CpuParticles2D, all of which work together to create an animated character that collides with the map/enemy/goal and displays a particle effect upon death.
## Enemy with pathfinding:
The enemy is coded so that it follows the provided path and targets the player for its movement, it is made up of an AnimatedSprite2D and has minimal animations for movement, a ColissionShape2D, a NavigationAgent2D and a Timer, all of which are used to allow the enemy to stay on the established path while targeting the player and damaging the player by 1 health each time a collision occurs.
## Particle effects:
There are 2 particles in the game, the first is constantly visible as FinishLine and once collided with gives the user the win. The second is the death particles that upon player death play once for 2.0s before the game resets to the begining.
## Interactions/Collisions/Collectibles:
The interaction/Collectable that the player has in this game is with the FinishLine, the player collides with the finish line and gets the game win screen with the option to press the play again button and reset the game and play from the start. There is also collision with the edge of the grass bridge that doesn't allow players to "fall into" the water portions of the map.
## UI and feedback:
There is health UI provided for the player to track their health, and the GameWin screen UI that displays upon successful collision with the FinishLine.

## Controls:
The player uses W, A, S, D to move around the screen, the only other control method needed is a mouse to click play again. Using the control keys the player can move around the map while trying to avoid the enemy chasing them, they have 3 health and each time the enemy touches them they lose 1 heart. If the enemy kills the player before they reach the finish line the player and enemy are reset to the start of the map and the players hearts are replenished. If the player reaches the FinishLine before the enemy hits them 3 times, the player wins the game and is given the option to play again which will reset the player and enemy positions and reset the players health.

### Assets:
[Raou Small Adventure Assets](https://raou.itch.io/small-adventure)
<br>
Small Adventure :: Base Top-Down Tileset
#### Includes: 
* Roofs
* Walls
* Trees
* Animated characters
* Terrain
#### Used:
* Animated characters
* Terrain