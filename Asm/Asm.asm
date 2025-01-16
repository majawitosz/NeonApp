.data
    r_weight dd 0.299       ; Changed to float
    g_weight dd 0.587       ; Changed to float
    b_weight dd 0.114       ; Changed to float
    r_mask dd 8 dup(00FF0000h) 
    g_mask dd 8 dup(0000FF00h)  
    b_mask dd 8 dup(000000FFh)  
    white_pixel dd 8 dup(0FFFFFFFFh)
    black_pixel dd 8 dup(0)
    alpha_val dd 255 
    treshold dd 8 dup(30)
    sobel_gx_tb dd 0, 0, 0, 0, 0, 1, 0, -1
    sobel_gx_m dd 0, 0, 0, 0, 0, 2, 0, -2
    sobel_gy_t dd 0, 0, 0, 0, 0, -1, -2, -1
    sobel_gy_m dd 0, 0, 0, 0, 0, 0, 0, 0
    sobel_gy_b dd 0, 0, 0, 0, 0, 1, 2, 1
   

.code
; 1st param (inputRowPrev) -> rcx
; 2nd param (inputRowCurrent) -> rdx  
; 3rd param (inputRowNext) -> r8
; 4th param (outputPixels) -> r9

DetectEdges proc
    push rbp
    mov rbp, rsp

    vmovdqu ymm0, ymmword ptr [rcx]    ; Previous row
    vmovdqu ymm1, ymmword ptr [rdx]   ; Current row
    vmovdqu ymm2, ymmword ptr [r8]     ; Next row

    vmovdqu ymm3, ymmword ptr [r_mask]
    vmovdqu ymm4, ymmword ptr [g_mask]
    vmovdqu ymm5, ymmword ptr [b_mask]

    ; === Color extraction for ymm0 (Previous Row) ===
    vpand ymm6, ymm0, ymm3             ; Extract red channel
    vpand ymm7, ymm0, ymm4             ; Extract green channel
    vpand ymm8, ymm0, ymm5             ; Extract blue channel

    vpsrld ymm6, ymm6, 16              ; Shift red
    vpsrld ymm7, ymm7, 8               ; Shift green

    vcvtdq2ps ymm6, ymm6               ; Convert red to float
    vcvtdq2ps ymm7, ymm7               ; Convert green to float
    vcvtdq2ps ymm8, ymm8               ; Convert blue to float

    ; === Color extraction for ymm1 (Current Row) ===
    vpand ymm9, ymm1, ymm3   ; red and
    vpand ymm10, ymm1, ymm4  ; green and
    vpand ymm11, ymm1, ymm5  ; blue and

    vpsrld ymm9, ymm9, 16    ; Shift red
    vpsrld ymm10, ymm10, 8     ; Shift green

    vcvtdq2ps ymm9, ymm9     ; Convert red to float
    vcvtdq2ps ymm10, ymm10     ; Convert green to float
    vcvtdq2ps ymm11, ymm11     ; Convert blue to float

    ; === Color extraction for ymm2 (Next Row) ===
    vpand ymm12, ymm2, ymm3   ; red and
    vpand ymm13, ymm2, ymm4  ; green and
    vpand ymm14, ymm2, ymm5  ; blue and

    vpsrld ymm12, ymm12, 16    ; Shift red
    vpsrld ymm13, ymm13, 8     ; Shift green

    vcvtdq2ps ymm12, ymm12     ; Convert red to float
    vcvtdq2ps ymm13, ymm13     ; Convert green to float
    vcvtdq2ps ymm14, ymm14     ; Convert blue to float

    ; === Greyscale ===
    vbroadcastss ymm3, dword ptr [r_weight]
    vbroadcastss ymm4, dword ptr [g_weight]
    vbroadcastss ymm5, dword ptr [b_weight]
   ; vbroadcastss ymm15, dword ptr [alpha_val]  ; Load and broadcast alpha value
   ; vpslld ymm15, ymm15, 24    ; Shift left by 24 for alpha

    ; === Greyscale mul for ymm0 (Previous Row) ===
    vmulps ymm6, ymm6, ymm3  ; red * 0.299
    vmulps ymm7, ymm7, ymm4  ; green * 0.587
    vmulps ymm8, ymm8, ymm5  ; blue * 0.114

    vaddps ymm6, ymm6, ymm7  ; Add red and green
    vaddps ymm6, ymm6, ymm8  ; Add blue
   
    vcvtps2dq ymm6, ymm6     ; Convert to integer with rounding
   
    vxorps ymm0, ymm0, ymm0  
    vmovdqu ymm0, ymm6 

 
    ; === Greyscale mul for ymm1 (Current Row) ===
    vmulps ymm9, ymm9, ymm3  ; red * 0.299
    vmulps ymm10, ymm10, ymm4  ; green * 0.587
    vmulps ymm11, ymm11, ymm5  ; blue * 0.114

    vaddps ymm9, ymm9, ymm10  ; Add red and green
    vaddps ymm9, ymm9, ymm11  ; Add blue
   
    vcvtps2dq ymm9, ymm9     ; Convert to integer with rounding

    vxorps ymm1, ymm1, ymm1  ; Clear ymm1
    vmovdqu ymm1, ymm9    
   
    ; === Greyscale mul for ymm2 (Next Row) ===
    vmulps ymm12, ymm12, ymm3  ; red * 0.299
    vmulps ymm13, ymm13, ymm4  ; green * 0.587
    vmulps ymm14, ymm14, ymm5  ; blue * 0.114

    vaddps ymm12, ymm12, ymm13 ; Add red and green
    vaddps ymm12, ymm12, ymm14 ; Add blue
   
    vcvtps2dq ymm12, ymm12     ; Convert to integer with rounding

    vxorps ymm2, ymm2, ymm2    ; Clear ymm2
    vmovdqu ymm2, ymm12  

    ; === Load Sobel masks for gx and gy ===
    vmovdqu ymm3, ymmword ptr [sobel_gx_tb]
    vmovdqu ymm4, ymmword ptr [sobel_gx_m]
    vmovdqu ymm5, ymmword ptr [sobel_gy_t]
    vmovdqu ymm6, ymmword ptr [sobel_gy_m]
    vmovdqu ymm7, ymmword ptr [sobel_gy_b]

    mov r10, 5
    mov eax, 01111111b
    vxorps ymm14, ymm14, ymm14
    vxorps ymm13, ymm13, ymm13
    vxorps ymm12, ymm12, ymm12
    vxorps ymm15, ymm15, ymm15

Sobel:
    ; === Obliczanie Gx ===
    vpmulld ymm8, ymm0, ymm3  ; top row * mask gx_tb
    vpmulld ymm9, ymm1, ymm4  ; middle row * mask gx_m
    vpmulld ymm10, ymm2, ymm3 ; bottom row * mask gx_tb

    vpaddd ymm8, ymm8, ymm9   
    vpaddd ymm8, ymm8, ymm10  

    vpslldq ymm9, ymm8, 4   ; Przesun w lewo o 4 bajty (1 piksel)
    vpslldq ymm10, ymm8, 8  ; Przesun w lewo o 8 bajtow (2 piksele)
      
    vpaddd ymm8, ymm8, ymm9   
    vpaddd ymm8, ymm8, ymm10 

   

    

    vpblendd ymm8, ymm8, ymm13, 01111111b
    vextracti128 xmm8, ymm8, 1 ; przenies gorna czesc ymm8 do xmm8
    vpsrldq ymm8, ymm8, 12 ; przesun na przedostnie 4 bajty
   
    VPBROADCASTD ymm8, xmm8

   VPMASKMOVD ymm14, ymm12, ymmword ptr [ymm8]

    ror eax, 1
   ; VPBLENDVB ymm14, ymm8, 



 


    ; shl r10, 2                ; Pozycja w bitach (4 bajty na piksel)
    ror eax, 1            ; Przesun maske
    vmovd xmm12, eax           ; Zaladuj maske do XMM
    
    VINSERTI128 ymm12, ymm12, xmm12, 1 ; Poszerz do YMM
    VPBLENDVB ymm14, ymm14, ymm8, ymm12


    

    ; cmp r10, 3
    ; jb Move
    ; Back:
 
    ;vpor ymm14, ymm8, ymm14 ; zapisz do ymm14
    ;vpslldq ymm14, ymm14, 4 ; przesun aby moc zapisywac nastpene


    ; === Obliczanie Gy ===

    vpmulld ymm8, ymm0, ymm5  ; top row * mask gx_tb
    vpmulld ymm9, ymm1, ymm6  ; middle row * mask gx_m
    vpmulld ymm10, ymm2, ymm7 ; bottom row * mask gx_tb

    vpaddd ymm8, ymm8, ymm9   
    vpaddd ymm8, ymm8, ymm10  

    vpslldq ymm9, ymm8, 4   ; Przesun w lewo o 4 bajty (1 piksel)
    vpslldq ymm10, ymm8, 8  ; Przesun w lewo o 8 bajtow (2 piksele)

    vpaddd ymm8, ymm8, ymm9   
    vpaddd ymm8, ymm8, ymm10 

    vpblendw ymm8, ymm8, ymm13, 00111111b
    vextracti128 xmm8, ymm8, 1 ; przenies gorna czesc ymm8 do xmm8
    vpsrldq ymm8, ymm8, 8 ; przesun na przedostnie 4 bajty

    vpor ymm15, ymm8, ymm15 ; zapisz do ymm15
    vpslldq ymm15, ymm15, 4 ; przesun aby moc zapisywac nastpene

    ; === Przesuniecie na nastepny piksel ===
   
    vpslldq ymm0, ymm0, 4
    vpslldq ymm1, ymm1, 4
    vpslldq ymm2, ymm2, 4

    dec r10                
    jnz Sobel        
    jmp Store
; Move:
 ;   VEXTRACTI128 xmm12, ymm14, 0     ; Pobierz dolna polowe ymm14 do xmm0
 ;   VPSLLDQ xmm12, xmm12, 4           
 ;   VINSERTI128 ymm14, ymm14, xmm12, 1 ; Wstaw zmodyfikowana dolna polowe na gore ymm14
 ;   jmp Back

Store:
    vpmulld ymm14, ymm14, ymm14
    vpmulld ymm15, ymm15, ymm15

    vpaddd ymm15, ymm14, ymm15
    VCVTDQ2PS ymm15, ymm15
    VSQRTPS ymm15, ymm15
    VCVTPS2DQ ymm15, ymm15
    vbroadcastss ymm14, dword ptr [treshold]
    VPCMPGTD ymm13, ymm15, ymm14  ; porownanie z progiem jesli wiekjsze to 1
    vbroadcastss ymm12, dword ptr [white_pixel]
    vpand ymm15, ymm12, ymm13

    vmovdqu ymmword ptr [r9], ymm15   ; Store processed pixels
    
    mov rsp, rbp
    pop rbp
    ret
DetectEdges endp
end