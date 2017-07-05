# MorrisMaze
This is a simulation of the water maze experiment written in Unity C#.
The way that the system is designed is for maximum configuration

##### Python files
Python files go starting in relative path to Assets/InputFiles.
The python script expects input as follows:

Given an integer that represents the the type of trial, the program has to output
exactly 1 x, y pair for the pickup object.

See [this file](Assets/InputFiles/Example.py)
for an example.


##### Configuration Detail
Note the initial configuration file is input.json located in
[here](Assets/InputFiles)


##### List of Available Commands:
1. [1] to INCREASE number of walls by offset
2. [2] to DECREASE number of walls by offset
3. [3] to get a random wall
4. [space] to commit your changes
5. [WASD and arrow keys] will work to control character
6. [G] to take a screen shot of the current game system
7. [H] to run through screen shots of the entire system
8. [F] to save the current file