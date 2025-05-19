## Floomp - Real-Time Strategy Auto Battler

# Overview

This Real-Time Strategy (RTS) Auto Battler is a game where players place buildings that automatically spawn units. These units travel across the map, battling opposing units spawned by an enemy player. The game’s core mechanics revolve around unit placement, countering enemy units, and strategic decision-making.

# Features

Players place buildings that spawn units automatically.

Each unit type has strengths and weaknesses against other units, adding strategic depth.

Units travel from the player’s side of the map to the enemy’s side, engaging in automatic combat.

A* Pathfinding (based on Code Monkey’s tutorial) ensures efficient and intelligent unit movement.

# How It Works

Players build structures that continuously spawn units over time.

Units automatically follow paths (calculated using A* Pathfinding) to reach the enemy side.

When units meet, they engage in combat, with damage calculations based on type strengths/weaknesses.

# Technical Details

The A* Pathfinding is implemented using Code Monkey’s tutorial, providing efficient and reliable pathfinding.

Units have configurable stats (damage, health, speed) and type-based strengths/weaknesses.

The game is designed for scalability, with plans for a larger battlefield similar in scope to Supreme Commander.

# Future Improvements

Add more unit types and structure types for greater strategic depth.

Implement a zoom-out feature to view a large-scale battlefield (inspired by Supreme Commander).

Improve unit AI for better combat behavior.

# License

This project is licensed under the MIT License.

# Acknowledgments

Code Monkey for the A* Pathfinding tutorial.

Supreme Commander for the large-scale battlefield inspiration.
