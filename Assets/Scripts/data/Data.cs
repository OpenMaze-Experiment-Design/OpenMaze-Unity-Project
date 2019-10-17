﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace data
{
    [Serializable]
    public class Data
    {
        // whether or not to turn on timing diagnostics
        public bool TimingVerification;

        // value from which trials start incrementing in the config file. 
        public int TrialInitialValue;

        // The amount of delay (in seconds) before a trial loads in. This value is used to 
        // ensure a consitent loading time between trials.
        public float TrialLoadingDelay = 3.0f;

        // The amount of time at the beginning of a trial when user input is ignore. This value
        // is used to avoid the user accidentally ending the trial by pressing the key pre-maturely.
        public float IgnoreUserInputDelay = 1.0f;

        // how often data is outputted. 
        public int OutputTimesPerSecond;

        //This is the object (defined below) which contains all data available for the main player
        public Character CharacterData;

        // The output file of the character's movements during an experiment
        public string OutputFile;

        //This is a LIST of the different types of pickup items that are defined below
        public List<Goal> Goals;

        // Runtime objects to load into the trials
        public List<LandMark> Landmarks;

        // Contains all pre-defined trials
        public List<Trial> TrialData;

        // Contains all Maze config objects
        public List<Maze> Mazes;

        public Dictionary<string, Maze> MazesDictionary;

        public List<BlockData> BlockList;

        public List<int> BlockOrder;

        [Serializable]
        public class BlockData
        {
            public string EndGoal; // percentage ___SPACE___ number. This is very arbitrary.
            public string EndFunction; // The function name (if not present, we assume its always true)

            public string BlockName; // Name (outputed at the end of the Block)
            public string Notes; // Notes about the given block
            public int Replacement; //Integer value representing replacement
            public List<RandomData> RandomTrialType; //Array that contains all the possible random values
            public List<int> TrialOrder; //Trial order (-1 means random)

            public bool ShowNumSuccessfulTrials; // Whether or not to display the number of successful trials
            public bool ShowCollectedPerTrial; // Whether or not to display the amount of goals/pickups collected (resets each trial)
        }

        [Serializable]
        public class RandomData
        {
            public List<int> RandomGroup;
        }

        [Serializable]
        public class Trial
        {
            public int TimeToRotate; // How long the delay can last in the rotation
            public int TwoDimensional; // Set to 1 iff trial is two dimensional
            public int Instructional; // Set to 1 iff trial is instructional
            public string FileLocation; // Is not null iff FileLocation exists (Image for 1D trials)
            public int EnvironmentType; // This is the environment type referenced.
            public int TimeAllotted; // Allotted amount of time
            public string TrialEndKey; // The key press which will end the current trial.
            public string Header; // Note outputted out of the trial.
            public MazeData Map; // The Map saved mazedata
            public List<int> InvisibleGoals; //The goal that are active and invisible
            public List<int> ActiveGoals; // Goals that are active and visible
            public List<int> InactiveGoals; // Goals that are visible but inactive
            public string MazeName; // Name of the maze to load
            public List<int> LandMarks; // List of all land marks
            public int Quota; // The quota that the person needs to pick up before the next trial is switched too
            public List<float> CharacterStartPos; //The start position of the character (usually 0, 0)
            public float CharacterStartAngle; // The starting angle of the character (in degrees).

            public bool ShowCollectedPerBlock; // Whether or not to display the amount of goals/pickups collected (resets each block)
        }

        // Represents a Maze for the user. Mazes can be prebuilt; located in:
        // Mazes can be 'Simple' with basic dimensions specified in the config file
        [Serializable]
        public class Maze
        {
            public string MazeName; // Must be unique
            public int GroundTileSides; // Number of sides on the ground pattern shapes - 0 for no tiles, 1 for solid color.
            public double GroundTileSize; // Relative size of the floor tiles - Range from 0 to 1
            public string GroundColor; // Colour of the ground
            public int Sides; // Number of sides present in the trial.
            public string WallColor; // HEX color of the walls
            public float WallHeight; // This is the wall height
            public int Radius; // Radius of the walls
        }

        // This is the given PickupItem. Note that in the JSON, this is contained with an ARRAY
        [Serializable]
        public class Goal
        {
            // In general this will always be one unless necessary to implement in the future.
            public string Tag; // The name of the pickup item
            public string Color; // The colour in Hex without #
            public string SoundLocation; // The file path of the sound
            public string PythonFile; // The python file that will generate the position
            public string Type; // The name of the prefab object
            public string ImageLoc; // The location of the image file associated with the goal

            // Use list for serialization purposes
            public List<float> Position;
            public List<float> Rotation;
            public List<float> Scale;
            public int InitialRotation;

            public Vector3 PositionVector
            {
                get
                {
                    return Position.Count == 0 ? Vector3.zero : new Vector3(Position[0], Position[1], Position[2]);
                }
            }

            public Vector3 RotationVector
            {
                get
                {
                    return Rotation.Count == 0 ? Vector3.zero : new Vector3(Rotation[0], Rotation[1], Rotation[2]);
                }
            }

            public Vector3 ScaleVector
            {
                get
                {
                    return Scale.Count == 0 ? Vector3.zero : new Vector3(Scale[0], Scale[1], Scale[2]);
                }
            }
        }

        [Serializable]
        public class Character
        {
            public float MovementSpeed; //The movement speed of the character
            public float RotationSpeed; //The rotation speed of the character
            public float GoalRotationSpeed; //The rotation speed of the goals
            public float Height; //The height of the character
            public float DistancePickup; //The min distance of the pickup to the character
        }

        // This is a pillar object.
        [Serializable]
        public class LandMark
        {
            public string Type;
            public string Color; // Color as a hex value
            public string ImageLoc; // The location (for 2Dimagedisplayer)

            // Use list for serialization purposes
            public List<float> Position;
            public List<float> Rotation;
            public List<float> Scale;

            public int InitialRotation;

            public Vector3 PositionVector
            {
                get
                {
                    return Position.Count == 0 ? Vector3.zero : new Vector3(Position[0], Position[1], Position[2]);
                }
            }

            public Vector3 RotationVector
            {
                get
                {
                    return Rotation.Count == 0 ? Vector3.zero : new Vector3(Rotation[0], Rotation[1], Rotation[2]);
                }
            }

            public Vector3 ScaleVector
            {
                get
                {
                    return Scale.Count == 0 ? Vector3.zero : new Vector3(Scale[0], Scale[1], Scale[2]);
                }
            }
        }

        [Serializable]
        public class MazeData
        {
            public List<float> TopLeft; //The top left of the maze
            public float TileWidth;
            public List<string> Map;
            public string Color;
        }
        //=========================== END OF JSON FIELDS ==========================================

        // This function converts the hex value to Colour.
        public static Color GetColour(string hex)
        {
            float[] l = { 0, 0, 0 };
            for (var i = 0; i < 6; i += 2)
            {
                float decValue = int.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                l[i / 2] = decValue / 255;
            }
            return new Color(l[0], l[1], l[2]);

        }

        public class Point
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }
    }
}