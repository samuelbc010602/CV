#include "memory.h"
#include "assembly.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>

#include "utils.c" // File including utility-functions.

int registers[32] = {0}; // registers to be used
int PC; // Program Counter
int done = 0; // indicator for if the program should be terminated

// Decode instructions of type I and execute them
void handleI(int instruction, struct memory*mem){
    int funct3 = get_funct3(instruction);
    int funct7 = get_funct7(instruction);
    int rs1 = get_rs1(instruction);
    int rs2 = get_rs2(instruction);
    int rd = get_rd(instruction);
    int imm = bit_segment(20, 12, instruction);
    int immsigned = sign_extend(bit_segment(20, 12, instruction), 12);
    int addr = immsigned + registers[rs1];

    switch (get_opcode(instruction)){
    case 3:
        switch (funct3)
            {
            case 0: // lb
                registers[rd] = sign_extend(memory_rd_b(mem, addr), 8);
                break;
            case 1: //lh
                registers[rd] = sign_extend(memory_rd_h(mem, addr), 16);
                break;
            case 2: //lw
                registers[rd] = memory_rd_w(mem, addr);
                break;
            case 4: // lbu
                registers[rd] = extractSegments(memory_rd_b(mem, addr), 0, 8);
                break;
            case 5: // lhu
                registers[rd] = extractSegments(memory_rd_h(mem, addr),0,16);
                break;
            default:
                break;
            }
        break;
    case 19:
        switch (funct3)
            {
            case 0: // addi
                registers[rd] = registers[rs1] + immsigned;
                break;
            case 1: // slli
                registers[rd] = registers[rs1] << rs2;
                break;
            case 2: // slti
                registers[rd] = (registers[rs1] < immsigned) ? 1 : 0;
                break;
            case 3: // sltiu
                registers[rd] = (registers[rs1] < imm) ? 1 : 0;
                break;
            case 4: // xori
                registers[rd] = registers[rs1] ^ immsigned;
                break;
            case 5: 
                if(funct7 == 0){ // srli
                    registers[rd] = registers[rs1] >> rs2;
                }else{ // == 1 srai
                    registers[rd] = registers[rs1];
                    while(rs2 > 0){
                        if(registers[rd] & 2147483648){
                            registers[rd] = registers[rd] >> 1;
                            registers[rd] = registers[rd] | 2147483648;
                        }else{
                            registers[rd] = registers[rd] >> 1;   
                        }
                        rs2--;
                    }
                }
                break;
            case 6: // ori
                registers[rd] = registers[rs1] | immsigned;
                break;
            case 7: // andi
                registers[rd] = registers[rs1] & immsigned;
                break;
            default:
                break;
            }
        break;
    case 103: // jalr
        if(rd != 0){
            registers[rd] = PC + 4;
        }
        PC = sign_extend((imm + registers[rs1]), 1);
        break;
    case 115: // ecall
        if(registers[17] == 1){
            registers[17] = getchar();
        }
        else if(registers[17] == 2){
            putchar(registers[16]);
        }
        else if(registers[17] == 3 || registers[17] == 93){
            done = 1;
        }
    default:
        break;
    }
}

// Decode instructions of type S and execute them
void handleS(int instruction, struct memory*mem){
    int offset = (get_imm2(instruction)<<5) | get_imm1(instruction);
    int rs1 = get_rs1(instruction);
    int rs2 = get_rs2(instruction);
    int addr = registers[rs1] + offset;
    int funct3 = get_funct3(instruction);

    switch(funct3){
        case 0: //sb 
            memory_wr_b(mem,addr,extractSegments(registers[rs2], 0, 8));
            break;
        case 1: //sh
            memory_wr_h(mem,addr,extractSegments(registers[rs2], 0, 16));
            break;
        case 2: //sw
            memory_wr_w(mem,addr,registers[rs2]);
            break;
    }
}

// Decode instructions of type U and execute them
void handleU(int instruction){
    int imm = extractSegments(instruction, 12, 20);
    int rd = get_rd(instruction);

    if(get_opcode(instruction) == 23){ //auipc
        imm = imm << 12;
        registers[rd] = PC + imm;

    }else{ // == 55 // lui
        imm = imm << 12;
        registers[rd] = imm;
    }

}

// Decode instructions of type R and execute them
void handleR(int instruction,int opcode){
    int rs1 = get_rs1(instruction);
    int rs2 = get_rs2(instruction);
    int rd = get_rd(instruction);
    int funct3 = get_funct3(instruction);
    int funct7 = get_funct7(instruction);

    if(funct7 != 1){
        switch(funct3){
            case 0:
                if(funct7 == 0){ // add
                    registers[rd] = registers[rs1]+registers[rs2];
                }else{ // sub
                        registers[rd] = registers[rs1]-registers[rs2];
                }
                break;
                case 1: // sll
                    registers[rd] = registers[rs1] << registers[rs2];
                    break;
                case 2: // slt
                    if(registers[rs1] < registers[rs2]){
                        registers[rd] = 1;
                    }else{
                        registers[rd] = 0;
                    }
                    break;
                case 3: // sltu
                    if(abs(registers[rs1]) < abs(registers[rs2])){
                        registers[rd] = 1;
                    }else{
                        registers[rd] = 0;
                    }
                    break;
                case 4: // xor
                    registers[rd] = registers[rs1]^registers[rs2];
                    break;
                case 5: // srl
                    if(funct7 == 0){
                        registers[rd] = abs(registers[rs1]) >> (registers[rs2] & 0b11111); 

                    }else{ // sra
                        registers[rd] = registers[rs1] >> (registers[rs2] & 0b11111);
                    }
                    break;
                case 6: // or
                    registers[rd] = registers[rs1] | registers[rs2];
                    break;
                case 7: // and
                    registers[rd] = registers[rs1] & registers[rs2];
                    break;
            }
    }else{ // M-expansion
        switch(funct3){
            case 0: // mul
                registers[rd] = registers[rs1]*registers[rs2];
                break;
            case 1: // mulh
                registers[rd] = (registers[rs1]*registers[rs2]) >> 31;
                break;
            case 2: // mulhsu
                registers[rd] = (registers[rs1]*abs(registers[rs2])) >> 31;
                break;
            case 3: // mulhu
                registers[rd] = (abs(registers[rs1])*abs(registers[rs2])) >> 31;
                break;
            case 4: //div
                registers[rd] = registers[rs1]/registers[rs2];
                break;
            case 5: //divu
                registers[rd] = abs(registers[rs1])/abs(registers[rs1]);
                break;
            case 6: //rem
                registers[rd] = registers[rs1]%registers[rs2];
                break;
            case 7: //remu
                registers[rd] = abs(registers[rs1])%abs(registers[rs2]);
                break;
            default:
                break;
        }
    }
}


// Decode instructions of type SB and execute them
void handleSB(int instruction){
    int funct3 = get_funct3(instruction);
    int rs1 = get_rs1(instruction);
    int rs2 = get_rs2(instruction);
    int imm11 = extractSegments(instruction,7,1);
    int imm2 = extractSegments(instruction,8,4);
    int imm3 = extractSegments(instruction, 25, 6);
    int sign = extractSegments(instruction, 31, 1);

    imm11 = imm11 << 10;
    imm3 = imm3 << 4;
    sign = sign << 11;
    int imm = (sign | imm11 | imm3 | imm2) << 1;
    imm = sign_extend(imm, 13);

    switch(funct3){
        case 0: // beq
            if(registers[rs1] == registers[rs2]){
                PC = PC+imm;
            }
            break;
        case 1: // bne
            if(registers[rs1] != registers[rs2]){
                PC = PC+imm;
            }
            break;
        case 4: // blt
            if(registers[rs1] < registers[rs2]){
                PC = PC+imm;  
            }
            break;
        case 5: // bge
            if(registers[rs1] > registers[rs2]){ 
                PC = PC+imm;
            }
            break;
        case 6: // bltu 
            if(abs(registers[rs1]) < abs(registers[rs2])){
                PC = PC+imm;             
            }
            break;
        case 7: // bgeu
            if(abs(registers[rs1]) >= abs(registers[rs2])){
                PC = PC+imm;           
            }
            break;
        default:
            break;
    }
}


// Divide the instructions in to there distinct types
void segmentHandler(int instruction,struct memory *mem){
    int opcode = extractSegments(instruction,0,7);
    switch(opcode){
        case 3: case 19: case 103: case 115: // I-type
            handleI(instruction, mem);
            break;
        case 23: case 55: // U-type
            handleU(instruction);
            break;
        case 111: // UJ-type: jal 
            registers[get_rd(instruction)] = PC + 4;
            int imm = 0;
            int imm1 = extractSegments(instruction, 21, 10);
            int imm3 = extractSegments(instruction, 12, 8);
            int wide = extractSegments(instruction, 20, 1);
            int sign = extractSegments(instruction, 31, 1);
            if(wide){
                imm3 = imm3 << 10;
                imm = imm3 | imm1;
                imm = imm << 1;
            }else{
                imm = imm1 << 1;
            }

            if(sign){
                imm = sign_extend(imm, 19);
            }

            PC = PC+imm;
            break;
        case 35: // S-type
            handleS(instruction,mem);
            break;
        case 51: case 59: // R-type
            handleR(instruction,opcode);
            break;
        case 99: // SB-type
            handleSB(instruction);
            break;
        default:
            break;
    }
}




long int simulate(struct memory *mem, struct assembly *as, int start_addr, FILE *log_file){
    PC = start_addr;
    long int num_insn;

    while(1){
        registers[0] = 0; // "hardcoded" 0
        int current_addr = PC;
        int instruction = memory_rd_w(mem,current_addr);
        if (!instruction || done) break; // check if we are finished

        segmentHandler(instruction,mem); // execute instruction

        if(PC == current_addr){ // if we did not branch we add 4 to program counter
            PC = PC+4;
        }
        num_insn++;
    }
    return num_insn;
}



// ----TEST----
// Test addi
// int instruction1 = 0b00010011100100000000001000010011;
// printf("value in registers[4] before: %d\n", registers[4]);
// segmentHandler(instruction1,mem);
// printf("value in registers[4] after: %d\n", registers[4]);



// Test slli
// int instruction1 = 0b00010011100100000000001000010011;
// printf("value in registers[4] before: %d\n", registers[4]);
// segmentHandler(instruction1,mem);
// printf("value in registers[4] after: %d\n", registers[4]);
// int instruction2 = 0b00000000010100100001000010010011;
// printf("value in registers[1] before: %d\n", registers[1]);
// segmentHandler(instruction2,mem);
// printf("value in registers[1] after: %d\n", registers[1]);



// Test slti
// int instruction1 = 0b00010011100100000000001000010011;
// printf("value in registers[4] before: %d\n", registers[4]);
// segmentHandler(instruction1,mem);
// printf("value in registers[4] after: %d\n", registers[4]);
// int instruction2 = 0b00010011101000100010000010010011; // 314
// // int instruction2 = 0b11101100011000100010000010010011; // -314
// printf("value in registers[1] before: %d\n", registers[1]);
// segmentHandler(instruction2,mem);
// printf("value in registers[1] after: %d\n", registers[1]);


// Test sltiu 
// int instruction1 = 0b00010011100100000000001000010011;
// printf("value in registers[4] before: %d\n", registers[4]);
// segmentHandler(instruction1,mem);
// printf("value in registers[4] after: %d\n", registers[4]);
// int instruction2 = 0b11101100100000100011000010010011;
// printf("value in registers[1] before: %d\n", registers[1]);
// segmentHandler(instruction2,mem);
// printf("value in registers[1] after: %d\n", registers[1]);



// Test xori
// printf("runnin risc V command: addi x4 x0 313\n")
// int instruction1 = 0b00010011100100000000001000010011;
// printf("value in registers[4] before: %d\n", registers[4]);
// segmentHandler(instruction1,mem);
// printf("value in registers[4] after (should be 313): %d\n", registers[4]);
// printf("runnin risc V command: xori x1 x4 413\n")
// int instruction2 = 0b00011001110100100100000010010011;
// printf("value in registers[1] before: %d\n", registers[1]);
// segmentHandler(instruction2,mem);
// printf("value in registers[1] after (should be 164): %d\n", registers[1]);



// Test srai
// printf("running risc V command: addi x4 x0 -500\n");
// int instruction1 = 0b11100000110000000000001000010011;
// printf("value in registers[4] before: %d\n", registers[4]);
// segmentHandler(instruction1,mem);
// printf("value in registers[4] after (should be -500): %d\n", registers[4]);
// printf("running risc V command: srai x1 x4 3\n");
// int instruction2 = 0b01000000001100100101000010010011;
// printf("value in registers[1] before: %d\n", registers[1]);
// segmentHandler(instruction2,mem);
// printf("value in registers[1] after (should be -63): %d\n", registers[1]);


/*
Test Add
int instruction1 = 0xfff58593; //addi	a1,a1,-1
int instruction2 = 0x00b506b3; //add	a3,a0,a1
segmentHandler(instruction1,mem);
assert(registers[11] == -1);
*/

/* 
Test sub
int instructions[] = {0b00000000001000000000000010010011,0b00000000010100000000000100010011,0b01000000000100010000000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    assert(registers[3] == 3);
*/

/*
Test slt 
int instructions[] = {0b00000110010000000000000010010011,0b00000000000100000000000100010011,0b0000000000100010010000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    assert(registers[3] == 1);
*/
/*
Test sll
int instructions[] = {0b00000000000100000000000010010011,0b00000111101100000000000100010011,0b00000000000100010001000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    printf("registers = %d \n",registers[3]);
    assert(registers[3] == 246);
*/

/*
test sltu
int instructions[] = {0b11111000010100000000000010010011,0b00000000000100000000000100010011,0b00000000000100010011000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    printf("registers = %d \n",registers[3]);
    assert(registers[3] == 1);
*/
/*
test xor 
int instructions[] = {0b00000111101100000000000010010011,0b00010100000100000000000100010011,0b00000000000100010100000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    printf("registers = %d \n",registers[3]);
    assert(registers[3] == 314);
*/
/*
test slt
int instructions[] = {0b00000000000100000000000010010011,0b00000111101100000000000100010011,0b00000000000100010101000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    printf("registers = %d \n",registers[3]);
    assert(registers[3] == 61);
*/
/*
test or
int instructions[] = {0b00010100000100000000000010010011,0b00000111101100000000000100010011,0b00000000000100010110000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    printf("registers = %d \n",registers[3]);
    assert(registers[3] == 379);
*/
/*
test and 
int instructions[] = {0b00000000111100000000000010010011,0b00000000110000000000000100010011,0b00000000000100010111000110110011};
    for(int i = 0;i<3;i++){
        segmentHandler(instructions[i],mem);
    }   
    printf("registers = %d \n",registers[3]);
    assert(registers[3] == 12);
*/
/*
test beq   
int instructions[] = {0b00010101100100000000000010010011,0b00000111101100000000000100010011,0b00000000001000001000010001100011,0b00000000100000000000000011101111,0b00000000001000000000000010010011,0b00000010100000000000000100010011};
    for(int i = 0;i<(sizeof(instructions)/sizeof(int));i++){
        segmentHandler(instructions[i],mem);
    }  
    printf("registers = %d \n",registers[2]);
    assert(registers[2] == 40);
*/

/*
test blt
int instructions[] = {0b00010101100100000000000010010011,0b00000111101100000000000100010011,0b00000000000100010001010001100011,0b00000000100000000000000011101111,0b00000000001000000000000010010011,0b00000010100000000000000100010011};
    for(int i = 0;i<(sizeof(instructions)/sizeof(int));i++){
        segmentHandler(instructions[i],mem);
    }  
    printf("registers = %d \n",registers[2]);
    assert(registers[1] == 2);
*/

/*
bge
int instructions[] = {0b00010101100100000000000010010011,0b00000111101100000000000100010011,0b00000000000100010101010001100011,0b00000000100000000000000011101111,0b00000000001000000000000010010011,0b00000010100000000000000100010011};
    for(int i = 0;i<(sizeof(instructions)/sizeof(int));i++){
        segmentHandler(instructions[i],mem);
    }  
    printf("registers = %d \n",registers[2]);
    assert(registers[2] == 40);
*/

/*
test bltu
int instructions[] = {0b11101010011100000000000010010011,0b11111000010100000000000100010011,0b00000000000100010110010001100011,0b00000000100000000000000011101111,0b00000000001000000000000010010011,0b00000010100000000000000100010011};
    for(int i = 0;i<(sizeof(instructions)/sizeof(int));i++){
        segmentHandler(instructions[i],mem);
    }  
    printf("registers = %d \n",registers[2]);
    assert(registers[2] == 40);
*/

/*
test bgeu
int instructions[] = {0b11110001011000000000000010010011,0b11111000010100000000000100010011,0b00000000001000001111010001100011,0b00000000100000000000000011101111,0b00000000001100000000000010010011,0b00000000001100000000000100010011};
    for(int i = 0;i<(sizeof(instructions)/sizeof(int));i++){
        segmentHandler(instructions[i],mem);
    }  
    printf("registers = %d \n",registers[2]);
    assert(registers[2] == 3);
*/
/*
Test MUL, REM, DIV
PC = start_addr;
    int instructions[] = {0b00000000001000000000010110010011,
                            0b00000000100000000000011000010011,
                            0b00000010110001011000011010110011,
                            0b00000010101101100100011100110011,
                            0b00000000001100000000010110010011,
                            0b00000010101101100110011110110011};
    for(int i = 0;i<sizeof(instructions)/sizeof(int);i++){
        printf("Executing instruction = %d\n",i);
        segmentHandler(instructions[i],mem);
        printf("All registers \n");
        for(int i = 0;i<sizeof(registers)/sizeof(int);i++){
            printf("Register %d = %d \n",i,registers[i]);
        }
    }   
    
    assert(registers[13] == 16);
    assert(registers[14] == 4);
    assert(registers[15] == 2);
*/

