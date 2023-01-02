GCC=gcc -g -Wall -Wextra -pedantic -std=gnu11 #-fsanitize=address
LD_FLAGS= -lpthread -lm


all: sim

rebuild: clean all

assembly.o : assembly.c assembly.h
	$(CC) $(CFLAGS) -c $< -o $@

memory.o : memory.c memory.h
	$(CC) $(CFLAGS) -c $< -o $@

read_exec.o : read_exec.c read_exec.h
	$(CC) $(CFLAGS) -c $< -o $@

simulate.o : simulate.c simulate.h
	$(CC) $(CFLAGS) -c $< -o $@

sim: main.c memory.o read_exec.o assembly.o simulate.o
	$(GCC) $< *.o -o $@ $(LD_FLAGS)

zip: ../src.zip

../src.zip: clean
	cd .. && zip -r src.zip src/Makefile src/*.c src/*.h

clean:
	rm -rf *.o sim vgcore