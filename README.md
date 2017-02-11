# WorkWork

Name is a reference to [Warcraft 3](https://www.youtube.com/watch?v=-y6wXYeZk8A).

## Summary

This is a bot I created for an outdated version of the game World of Warcraft (2.4.3).

This is my first real interaction with C# and "reverse engineering", using [Cheat Engine](http://www.cheatengine.org/).

This project was created and is meant for educational purposes only and took about a week to reach its current state.
I am not responsible for any use of this software.

## In depth

* The bot uses a waypoint system for navigation, where points are mapped in the world and the bot moves from one to another in a straight line. 

* The bot can loop a route or traverse it once. 

* It can attack creatures and/or players and defend itself in combat of medium complexity and can perform complex combat rotations if set up correctly. It can also perform abilities and spells outside of combat, as well as drink and eat. It can loot creatures, as well as farm minerals and herbs. It can sell trash to vendors.

* If the player character dies, it can release spirit, walk back, resurrect, and continue from where it left off. 

* If a monster or creature is unreachable or otherwise stuck, it can blacklist it and avoid it in the future. It can jump randomly to imitate normal human behavior. 

* It keeps track of kills, deaths and other relevant stats that make optimizing paths easier. It supports multiple classes and multiple characters of each class. Lastly, it has its own path editor.

## The GUI

![bot1](https://cloud.githubusercontent.com/assets/6977074/14067184/1ca6e0c6-f45f-11e5-8381-e8a4359ced22.png)
![bot2](https://cloud.githubusercontent.com/assets/6977074/14067185/22ea92fc-f45f-11e5-8fdf-c8e3301b1fde.png)

The images above are screen-shots of how the actual bot UI looks and feels.

### Note:
The linear option defines whether the bot will continue from the waypoint it left off after combat or look for the closest waypoint instead. If the environment you are in is not cluttered, this is a good option to have on, and vice versa.

### I am not actively working on this project anymore.


