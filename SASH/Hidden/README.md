
# SASH.Hidden namespace  

### In this namespace are contained the algorythmic strructures, controlling how the program works.  

### Classes  
> Currently, there is only one class added:    
The [Internal](https://github.com/Petaaar/SASH/blob/master/SASH/Hidden/Internal.cs) class is a __static__ class, cannot be inherited. Contains 7 useful functions as follows:  

###### KillCmd() -> Kills the program. Useful when thrown exceptions and/or we want to close the program.  
###### Error(message) -> Writes a custom message on the screen.  
###### Sleep(millis) -> Makes the program "sleep" for certain milliseconds(millis).  
###### Starter() -> Creates a new instance of the [Starter](https://github.com/Petaaar/SASH/blob/master/Sash/Starter.cs) class.  
###### ToList<T>(this T[] args) -> An __*EXTENSION METHOD*__ for all __arrays_. Returns List<T>.  
###### Ask(message) -> Writes a custom "question".  

### Structs  
> Currently, there is only one struct added:  
The [GetCommand](https://github.com/Petaaar/SASH/blob/master/SASH/Hidden/GetCommand.cs) struct contains the ReadCommand()
method, wich reads a comand from the command prompt and runs the entire program, depending on it.
