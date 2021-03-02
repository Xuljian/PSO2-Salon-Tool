# PSO2 SALON TOOL  
Console version of the tool made by Shadowth117 [Link to his repo](https://github.com/Shadowth117/PSO2-Salon-Tool)  
-o for output path (No filename specified required, it will use the input filename as the output filename)  
-ext is the type of character file (fhp, fnp, fcp, fdp, mhp, mnp, mcp, mdp), if this option is not provided the default race and gender will be given.  
-na is for NA height fix, if you want the original height ignore this option

## Example
Argument example: "C:\Data\npc_91.cml" -o "C:\Data" -ext fdp  
The program will take the npc_91.cml in C:\Data\npc_91.cml and convert it to npc_91.fdp and put it in C:\Data\npc_91.fdp  

There are other files the library supports but for now this program only supports cml files