#include "memory.h"
#include "assembly.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

//her har jeg bare lagt alle hjælpefunktionerne ind

//inklusiv start ekslusiv slut
int extractSegments(int instruction, int start, int length){
    return (instruction >> start) & ((int)pow(2,length)-1);
}

int add(int first, int second){
    return (first << ((int)log2(second)+1)) | second;
}

int sub(int first, int second)
{
    
    while (second != 0) 
    {
        int borrow = (~first) & second;
        first = first ^ second;
        second = borrow << 1;
    }
    return first;
}


int bit_segment(int p, int k, int instruction){
    int bit_mask = pow(2, k) - 1;
    int shifted_instruction = instruction >> p;
    int bit_segment = shifted_instruction & bit_mask;

    return bit_segment;
}

int get_opcode(int instruction){
    return bit_segment(0, 7, instruction);
}

int get_rd(int instruction){
    return bit_segment(7, 5, instruction);
}

int get_funct3(int instruction){
    return bit_segment(12, 3, instruction);
}

int get_rs1(int instruction){
    return bit_segment(15, 5, instruction);
}

int get_rs2(int instruction){
    return bit_segment(20, 5, instruction);
}

int get_funct7(int instruction){
    return bit_segment(25, 7, instruction);
}

int get_imm1(int instruction){
    return extractSegments(instruction,7,5);
}
int get_imm2(int instruction){
    return extractSegments(instruction,25,7);
}


int sign_extend(int num, int bitcount){
    int i = 30;
    if((num & (int)pow(2, bitcount-1)) == (int)pow(2, bitcount-1)){
        num = num | 0b10000000000000000000000000000000;
        while(i >= bitcount){
            num = num | ((int)pow(2,i));
            i--;
        }
    }
    return num;
}

