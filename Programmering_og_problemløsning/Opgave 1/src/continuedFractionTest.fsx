open continuedFraction

//I use the following function compareFloat to compare float values:
let compareFloat (x : float) (y : float) : bool =
    System.Math.Abs(x - y) < 0.001

//Blackbox test for cfrac2float:

printfn "Blackbox test cfrac2float:"
printfn "  %b: [1;4;2;5] = %f" (compareFloat (cfrac2float [1;4;2;5]) 1.224)
 (cfrac2float [1;4;2;5])
printfn "  %b: [] = %f" (cfrac2float [] = 0.0)
 (cfrac2float [])
printfn "  %b: [12;2;2;1] = %f" (compareFloat (cfrac2float [12;2;2;1]) 12.428)
 (cfrac2float [12;2;2;1])
printfn "  %b: [-1;7;3;9] = %f" (compareFloat (cfrac2float [-1;7;3;9]) 0.0)
 (cfrac2float [-1;7;3;9])
printfn "  %b: [0;9;9;8] = %f" (compareFloat (cfrac2float [0;9;9;8]) 0.0)
 (cfrac2float [0;9;9;8])

//Whitebox test for cfrac2float
//Testing branch 1a
//The first 1a is my if-statement which is executed when the length
//of the list or the first number in the the list is 0.
//I do this test by giving an empty [] and then a list with the first
//element being 0.

printfn "Whitebox test for cfrac2float"
printfn "Testing branch 1a:"
printfn "  %b: [] = %f" (cfrac2float [] = 0.0) (cfrac2float [])
printfn "  %b: [0] = %f" (cfrac2float [0] = 0.0) (cfrac2float [0])

//Testing branch 2a
//The second branch is my else statement which gets executed everytime
//the list has more than zero elements and the first one isn't 0
printfn "Testing branch 2a:"
printfn "  %b: [3;5;2;3] = %f" (compareFloat (cfrac2float [3;5;2;3]) (3.184))
 (cfrac2float [3;5;2;3])

//Blacbox test for float2cfrac

printfn "Blackox test float2cfrac"
printfn "  %b: 1.489" (float2cfrac 1.489 = [1;2;22;4;2])
printfn "  %b: 100.232" (float2cfrac 100.232 = [100;4;3;4])

//Whitebox test for float2cfrac
//There is two branches 1a and 2a to test here, the first one being the if statement
//that checks whether or not it is larger or not than 0.
//The other branch 2a is run when x, the float value, is larger than 0

printfn "Whitebox test for float2cfrac"
printfn "Testing branch 1a"
printfn "  %b: 0.0" (float2cfrac 0.0 = [])
printfn "Testing branch 2a"
printfn "  %b: 3.245" (float2cfrac 3.245 = [3;4;12;4])

