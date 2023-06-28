# Open XR Experimental Boundary

This project aims to replicate the quest's experimental boundary setting: [Video of person using quest](https://www.youtube.com/watch?v=4t1CdmDeBhA)
The main focus is to allow this to be used on any openxr setup therefore proprietary software should not be used unless it is needed for a specific export.

## Requirements
The following and their dependencies are required for the project:
- [Unity](https://unity.com/) (Made on 2021.3.27f1)
- [Newtonsoft Json](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0/manual/index.html) 
- [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.3/manual/index.html)

| Headset           	| Supported ✔️❌🤔 	|
|-------------------	|----------------	|
| Quest 2 / Quest Pro    	| 🤔              	|
| Pico 4  	| 🤔              	|
| Htc Vive / Cosmos 	| ✔️              	|
| Index       	| ✔️              	|

🤔 is untested

## What's Implemented?

| Feature           	| Implemented ✔️🚧❌ 	|
|-------------------	|----------------	|
| Placing Corner    	| ✔️              	|
| Drawing Objects   	| ✔️              	|
| Deleting Objects  	| ✔️              	|
| Modifying Objects 	| 🚧              	|
| Object Tags       	| ❌              	|
| User Interface    	| ❌              	|
| SDK for getting objects    	| ❌              	|
| Passthrough    	| ❌              	|

## Overview of the system

The user first selects a corner then after they select a corner they then select a new point on the floor this spawns a selector which adjusts for the height of the object once this is put to the correct height the user deselects this and then picks the final point to draw the cuboid. 

```
                    ┌─────────────────────────────────┐
                    │                                 │
                    │        HandleRayTable.cs        │
                    │                ▲                │
                    │                │                │
                    │        ┌───────┤                │
                    │  spawns│       │                │
                    │        │       ▼                │spawns
                    │        │  DrawObject.cs         │
                    ▼        │                        │
       ┌────────►object ◄────┘                        │
       │         │ │                                  │
       │         │ └───────────►SaveObjects.cs        │
deletes│         │                    │               │          starts
       ├─────────objectdata.cs        │         LoadObjects.cs◄──────────FindAndStartSave.cs
       │  needs                       │               ▲
       │                  Serialises  │               │
   DeleteObject.cs              into  │      ┌────────┴──────┐
                                      ▼      │               │
                                SerialObject.cs ◄───────────SerialObjects.cs
                                                 stores
 ```