# The SASH System.
by Petar Angelov - [Petaaar](https://github.com/Petaaar)

### The SASH (or Simple Automated Script Headers) System Project.

The SASH (or Simple Automated Script Headers) System is a project for me to test my coding skills.
The project in a nutshell is a command-based system, like Linux or MacOS, but much more simplified,  so everybody can easely understand the commands and use them.  

### Commands.  
All commands are separated in different folders(namespaces), so everybody can easely navigate through the project.

###### IO folder.  
> For fast navigation to there click [here](https://github.com/Petaaar/SASH/blob/master/SASH/IO/).  

```  
1. RUN, DELETE, CREATE, COPY command specs are in the SASH.IO folder.  

2. CLEAR -> Clears the console.  

3. EXIT -> Kills the program. Determines all processes, connected to it.

```  

The [SASH.IO](https://github.com/Petaaar/SASH/blob/master/SASH/IO/) folder contains a number of commands, useful for 
creating, deleting and running an external programs / files.

###### "Hidden" folder  
> For fast navigation to there click [here](https://github.com/Petaaar/SASH/tree/master/SASH/Hidden).  

The [Hidden](https://github.com/Petaaar/SASH/tree/master/SASH/Hidden) folder contains the structures, controlling the program - the [Internal](https://github.com/Petaaar/SASH/blob/master/SASH/Hidden/Internal.cs) class and the [GetCommand](https://github.com/Petaaar/SASH/blob/master/SASH/Hidden/GetCommand.cs) struct.

###### "Parser" folder
> For fast navigation to there click [this](https://github.com/Petaaar/SASH/tree/master/SASH/Parser).

The [Parser](https://github.com/Petaaar/SASH/tree/master/SASH/Parser) folder is just a slightly-simplified version of the [XMLParser](https://github.com/Petaaar/xmlparser) project.  
It works with the "append" command, traverses a __.sashs__ file and outputs an automatically generated __*C# class*__.    

For more info go to the XMLParser page in my [GitHub profile](https://github.com/Petaaar) or click [here](https://github.com/Petaaar/xmlparser).