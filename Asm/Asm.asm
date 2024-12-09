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

    vpand ymm6, ymm1, ymm3   ; red and
    vpand ymm7, ymm1, ymm4  ; green and
    vpand ymm8, ymm1, ymm5  ; blue and

    vpsrld ymm6, ymm6, 16    ; Shift red
    vpsrld ymm7, ymm7, 8     ; Shift green

    vcvtdq2ps ymm6, ymm6     ; Convert red to float
    vcvtdq2ps ymm7, ymm7     ; Convert green to float
    vcvtdq2ps ymm8, ymm8     ; Convert blue to float

    vbroadcastss ymm3, dword ptr [r_weight]
    vbroadcastss ymm4, dword ptr [g_weight]
    vbroadcastss ymm5, dword ptr [b_weight]

    vmulps ymm6, ymm6, ymm3  ; red * 0.299
    vmulps ymm7, ymm7, ymm4  ; green * 0.587
    vmulps ymm8, ymm8, ymm5  ; blue * 0.114

    vaddps ymm6, ymm6, ymm7  ; Add red and green
    vaddps ymm6, ymm6, ymm8  ; Add blue
   
    vcvtps2dq ymm6, ymm6     ; Convert to integer with rounding

    vbroadcastss ymm9, dword ptr [alpha_val]  ; Load and broadcast alpha value
    vpslld ymm7, ymm6, 16    ; Shift left by 16 for red channel
    vpslld ymm8, ymm6, 8     ; Shift left by 8 for green channel
    vpslld ymm9, ymm9, 24    ; Shift left by 24 for alpha
    vpor ymm7, ymm7, ymm8    ; Combine red and green
    vpor ymm7, ymm7, ymm6    ; Add blue channel
    vpor ymm7, ymm7, ymm9 ; Add alpha
     
    vmovdqu ymmword ptr [r9], ymm7    ; Store processed pixels
    
    mov rsp, rbp
    pop rbp
    ret
DetectEdges endp
end