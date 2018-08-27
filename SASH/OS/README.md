# OS folder
Contains a simple OS, environment and thread operations, including creating your own __environment variable__.
1. Os class  

##### Simple commands:  

__*os -curpath*__ -> Shows the current path of the executable.  
__*os -curthread*__ -> Shows the current thread __id__.  
__*os -bit*__ -> Shows the bits of your operation system(either 32 or 64).  
__*os -traverse*__ -> Traverses the current directory.   

##### os env commands:
The "env" sub-command works in conjunction with the [Environments](https://github.com/Petaaar/SASH/blob/master/SASH/OS/Environments.cs) class in order ot modify environment variables.  
__*os env var [VARIABLE_NAME]*__ -> Shows the value of the given variable name.  
__*os env var create [VARIABLE_NAME] [VARIABLE_VALUE]*__ -> Creates an environment variable with the specified name and value.  
__*os env var create [VARIABLE_NAME] [VARIABLE_VALUE] [TARGET]*__ -> Creates an environment variable with the specified name, value and target(either mashine, process or user).    

##### os mkd:
__*os mkd DIRNAME*__ -> Creates a directory using the [IO.Create](https://github.com/Petaaar/SASH/blob/master/SASH/IO) class.  

##### os show:
__*os show this*__ -> Traverses the current directory. Same as __os -traverse__.      
__*os show DIRNAME*__ -> Traverses a given directory by name - DIRNAME.   

##### Example usage:
__os env var path__ -> Displays __*every*__ value of the corresponding variable name, "path" in this case.  
__os env var create MY_VAR c:\Users\Public__ -> Creates environment variable MY_VAR with the value of "C:\Users\Public" and the current __*user*__ as a target.  
__os env var create MY_VAR c:\Users\Public machine__ ->Creates environment variable MY_VAR with the value of "C:\Users\Public" and the current __*machine*__ as a target.
__os mkd myFolder__ -> Creates a new folder called "myFolder" in the boot directory of the program.  
__os show c:\\__ -> Traverses the c:\ directory of your pc and displays it's contents.  
