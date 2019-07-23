prog = ReadAlphabets("BPMaxW.ab");
system = "bpmaxw";
outDir = "./workspace/"+system;

CheckProgram(prog);

AShow(prog, system);

NormalizeReduction(prog, system, "FTable");
RenameVariable(prog, system, "NR_FTable", "Main");
SerializeReduction(prog, system, "Main", "(i1,j1,i2,j2,k1,k2->i1,j1,i2,j2,k1,k2)");
NormalizeReduction(prog, system, "P1");
NormalizeReduction(prog, system, "P2");
SerializeReduction(prog, system, "NR_P1", "(i,j,k->i,j,k)");
SerializeReduction(prog, system, "NR_P2", "(i,j,k->i,j,k)");
ForceCoB(prog, system, "FTable", "(i1,j1,i2,j2 -> i1+1,j1+1,i2+1,j2+1)");
ForceCoB(prog, system, "_serMain", "(i1,j1,i2,j2,k1,k2 -> i1+1,j1+1,i2+1,j2+1,k1+1,k2+1)");
Normalize(prog);
#correctness of an Alphabets program through static analysis
CheckProgram(prog);
AShow(prog, system);
ASave(prog, "BPMaxW-v1.ab");

####
setMemoryMap(prog, system, "_serMain", "Q","(i1,j1,i2,j2,k1,k2->i1,j1,i2,j2)");
setMemoryMap(prog, system, "FTable", "Q","(i1,j1,i2,j2->i1,j1,i2,j2)");
setMemoryMap(prog, system, "Main", "Main","(i1,j1,i2,j2->i1,j1,i2,j2)");

options=createCGOptionForScheduledC();
setCGOptionFlattenArrays(options,1);

setSpaceTimeMap(prog, system, "FTable", "(i1, j1, i2, j2 -> 		  -i1+j1+1, j1, -i2+j2+1, j2, 0, 0, 2)");
setSpaceTimeMap(prog, system, "Main", "(i1, j1, i2, j2-> 			  -i1+j1+1, j1+1, -i2+j2+1, j2+1, 0, 0, 1)");
setSpaceTimeMap(prog, system, "_serMain", "(i1, j1, i2, j2, k1, k2->  -i1+j1+1, j1, -i2+j2+1, j2, -j1+k1, -j2+k2, 0)");



setSpaceTimeMap(prog, system, "NR_P1", "(i, j -> 0, 0, 0, -i, j, j, 0)");
setSpaceTimeMap(prog, system, "NR_P2", "(i, j -> 0, 0, 0, -i, j, j, 0)");
setSpaceTimeMap(prog, system, "P1", "(i, j -> 0, 0, 0, -i, j, j, 2)");
setSpaceTimeMap(prog, system, "P2", "(i, j -> 0, 0, 0, -i, j, j, 2)");
setSpaceTimeMap(prog, system, "_serNR_P1", "(i, j, k -> 0, 0, 0, -i, j, k, 1)");
setSpaceTimeMap(prog, system, "_serNR_P2", "(i, j, k -> 0, 0, 0, -i, j, k, 1)");

#you have to consider that dimention starts from 0. 1 represents the secend outer loop
setParallel(prog,system,"",1); 
generateScheduledCode(prog, system, options, outDir);

####

generateWrapper(prog, system, options, outDir);
generateMakefile(prog, system, options, outDir);

