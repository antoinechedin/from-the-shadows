#### Play Mode Saver ###

## To Use:

Make sure UnityPlayModeSaver.cs is inside a folder called 'Editor' somewhere in your projects Assets folder.
Then whilst the game is playing in the editor, right click on any Component in the Inspector Panel or Game Object in the hierarchy view and select 'Save Play Mode Changes' to keep that Objects values upon exiting playmode.

All changes will be applied when you exit Play Mode. You can undo/re-apply them with Ctrl-Z and Ctrl-Y.
You can also select 'Forget Play Mode Changes' on any object you saved changes for to stop them being applied.

## Known Issues.

- Right clicking on multiple Components or GameObjects will only save the values for the actual object you right clicked on rather than the group.