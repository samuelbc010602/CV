# Galaga
This was an exam project I had in connection with the Software Development course at KU in my first year.
We were tasked with making a game very reminiscent of the classic "Space Invaders" video game from the 80s,
where we had to implement many of the techniques and methods we have learned throughout the course.
The game itself is built on top of a game engine that was made available to us by DIKU, I have not linked it here, this folder only contains the files I have made myself.

The code follows the object-oriented paradigm, where "Solid Principles" have been followed very closely to make the code modular and adaptable to new changes,
which came at two-week intervals over a six-week period. I have put a lot of emphasis on fulfilling the dependency inversion principle,
by creating a facade (facade.cs) through which high-level code communicates through a facade that hides and simplifies
many of the various low-level functionalities from the game engine itself that are necessary for the game to function.
In addition to this, much emphasis has also been placed on testing to be able to verify that the functionality of the program meets the specifications.
Here I strived to achieve C-0 and C-1 test coverage.

In addition to this, I have also included a pdf file with the report documenting the entire work process that has been behind the program, if it could be of interest.

Here I have only included the code that I myself have helped to create.
