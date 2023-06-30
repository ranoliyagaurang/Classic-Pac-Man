1. Project Structure

Assets
    Audio (Contains all audio files)
    Models (Contains all fbx, materials and textures)
    Particles (Contains all particles asset)
    Prefabs
        AI (Contains the AI_Ghost)
        Maze (Contains the obstacles assets used in the designing of maze)
        Player (Contains the Players)
    Scenes (Contains the gamePlay scene)
    Scripts
        Controller (Contains all controllers like AI & Player)
        Managers (Contains the managers)
        Other (Contains the common scripts)

2. Scene Structure

Environment
Setup
Player
Ghosts
UI
Managers

3. Script Structure

=> All Managers are using the Singleton pattern to define Global variables and classes and use their methods and properties in Global.
=> Functions, variables are written using the standard coding style.
=> GameManager handle gamePlay states, update score, and show GameOver when hit to AI_Ghost.
=> AudioManager handle all audio clips to be played when required like collect, click, backgroundMusic & BlastSound.
=> PoolingManager handle all objects which are use repeatedly like collect, blast Particles.

4. AI Structure

=> AI Ghost uses the Navmesh Path to move randomly into the maze through the Agent.
=> When AI Ghost collides with other AI then find different point in Navmesh Path and will move to that point.
