.data
    r_weight dd 0.299       ; Changed to float
    g_weight dd 0.587       ; Changed to float
    b_weight dd 0.114       ; Changed to float
    r_mask dd 8 dup(00FF0000h) 
    g_mask dd 8 dup(0000FF00h)  
    b_mask dd 8 dup(000000FFh)  
    alpha_val dd 255 
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
    vbroadcastss ymm15, dword ptr [alpha_val]  ; Load and broadcast alpha value
    vpslld ymm15, ymm15, 24    ; Shift left by 24 for alpha

    ; === Greyscale mul for ymm0 (Previous Row) ===
    vmulps ymm6, ymm6, ymm3  ; red * 0.299
    vmulps ymm7, ymm7, ymm4  ; green * 0.587
    vmulps ymm8, ymm8, ymm5  ; blue * 0.114

    vaddps ymm6, ymm6, ymm7  ; Add red and green
    vaddps ymm6, ymm6, ymm8  ; Add blue
   
    vcvtps2dq ymm6, ymm6     ; Convert to integer with rounding

    vpslld ymm7, ymm6, 16    ; Shift left by 16 for red channel
    vpslld ymm8, ymm6, 8     ; Shift left by 8 for green channel

    vpor ymm7, ymm7, ymm8     
    vpor ymm7, ymm7, ymm6     
   
    vxorps ymm0, ymm0, ymm0  
    vpor ymm0, ymm7, ymm15   
 
    ; === Greyscale mul for ymm1 (Current Row) ===
    vmulps ymm9, ymm9, ymm3  ; red * 0.299
    vmulps ymm10, ymm10, ymm4  ; green * 0.587
    vmulps ymm11, ymm11, ymm5  ; blue * 0.114

    vaddps ymm9, ymm9, ymm10  ; Add red and green
    vaddps ymm9, ymm9, ymm11  ; Add blue
   
    vcvtps2dq ymm9, ymm9     ; Convert to integer with rounding

    vpslld ymm10, ymm9, 16    ; Shift left by 16 for red channel
    vpslld ymm11, ymm9, 8     ; Shift left by 8 for green channel

    vpor ymm10, ymm10, ymm11     
    vpor ymm10, ymm10, ymm9     
    
    vxorps ymm1, ymm1, ymm1  ; Clear ymm1
    vpor ymm1, ymm10, ymm15     
   
    ; === Greyscale mul for ymm2 (Next Row) ===
    vmulps ymm12, ymm12, ymm3  ; red * 0.299
    vmulps ymm13, ymm13, ymm4  ; green * 0.587
    vmulps ymm14, ymm14, ymm5  ; blue * 0.114

    vaddps ymm12, ymm12, ymm13 ; Add red and green
    vaddps ymm12, ymm12, ymm14 ; Add blue
   
    vcvtps2dq ymm12, ymm12     ; Convert to integer with rounding

    vpslld ymm13, ymm12, 16    ; Shift left by 16 for red channel
    vpslld ymm14, ymm12, 8     ; Shift left by 8 for green channel

    vpor ymm13, ymm13, ymm14     
    vpor ymm13, ymm13, ymm12     
   
    vxorps ymm2, ymm2, ymm2    ; Clear ymm1
    vpor ymm2, ymm13, ymm15    

    vxorps ymm3, ymm3, ymm3
    vxorps ymm4, ymm4, ymm4
    vxorps ymm5, ymm5, ymm5
    vxorps ymm6, ymm6, ymm6
    vxorps ymm7, ymm7, ymm7
    vxorps ymm8, ymm8, ymm8
    vxorps ymm9, ymm9, ymm9
    vxorps ymm10, ymm10, ymm10
    vxorps ymm11, ymm11, ymm11
    vxorps ymm12, ymm12, ymm12
    vxorps ymm13, ymm13, ymm13
    vxorps ymm14, ymm14, ymm14
    vxorps ymm15, ymm15, ymm15

    ; === Sobel Gx ===


    vmovdqu ymmword ptr [r9], ymm1    ; Store processed pixels
    
    mov rsp, rbp
    pop rbp
    ret
DetectEdges endp
end