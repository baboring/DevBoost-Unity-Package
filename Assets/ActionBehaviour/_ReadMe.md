Version 0.0.8 [2018.11.03]
- Updated ActionNode ( changed permission level to access the OnUpdate from public to protected)
- Added SingleActionNode ( singleton start point to run) 

Version 0.0.7 [2018.10.27]
- Updated SingletonMono ( removed virtual on Awake in order to keep alone, if a child class want to use Awake, then it should be used by new keyword on it )
- Updated Log ( added option saving path for persistent data path )

Version 0.0.6 [2018.10.24]
- Updated BaseNode ( updated OnUpdate logic)
- Added ActiveScene, Activate giving scene to active
- Updated SequenceNode ( fixed coroutine work in other coroutine, infinity loop)
- Separated scenes to load

Version 0.0.6 [2018.10.22]
- Added ExecuteInvoke on Execute Node
- Added Log for log tracing
- Updated SingletonMono 

Version 0.0.5 [2018.06.09]
- Updated ScrollSnap 
- Added RectTransformExtensions

Version 0.0.4 [2018.06.03]
- Added ActionScript from the project
- ActionScript have notifier and object base on scriptObject

Version 0.0.3 [2018.06.02]
- Updated AssignParentTransform ( Added GroupBox and hide condition in it  for showing field properly )
- Deprecated ActionController and Replaced ActionStarter instead
- Updated SceneLoader ( Added GroupBox and rename set field from Set to LevelNameSet )

- Updated InactiveGameObject ( added reorderable attribute )
- Added ScrollSnap into UI


Version 0.0.2 [2018.06.01]

* Updated ActionBehaviour Scripts
- Merged Standard Library
- Added Extensions to transform



Version 0.0.1 [2018.05.31]
- Base Node & actions
- Composite
- Data
- ScriptableObjects
- System

* /Data/StringSet
 - Scriptable Object, Saving the string list 