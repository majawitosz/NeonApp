.data
align 16
BlueMask    dd 4 dup(0FFh)      ; Mask for blue channel
GreenMask   dd 4 dup(0FF00h)    ; Mask for green channel
RedMask     dd 4 dup(0FF0000h)  ; Mask for red channel
AlphaMask   dd 4 dup(0FF000000h); Mask for alpha channel
Threshold   dd 4 dup(15)        ; Lower threshold for better edge detection
BlackPixels dd 4 dup(0)         ; Black pixels (0,0,0,255)
WhitePixels dd 4 dup(0FFFFFFFFh); White pixels (255,255,255,255)

.code
DetectEdges proc
    push rbp
    mov rbp, rsp
    push rsi
    push rdi
    push rbx
    
    mov rsi, rcx        ; Source image pointer
    mov rdi, rdx        ; Destination image pointer
    
    ; Initialize XMM registers with constants
    pxor xmm7, xmm7              
    movdqu xmm6, xmmword ptr [BlueMask]   
    movdqu xmm5, xmmword ptr [GreenMask] 
    movdqu xmm4, xmmword ptr [RedMask]  
    movd xmm3, dword ptr [Threshold]     
    pshufd xmm3, xmm3, 0
    
    xor rcx, rcx        ; Initialize counter
    
align 16
process_loop:
    cmp rcx, r8
    jge done
    
    ; Load current pixel and pixel to the right
    mov rax, rcx
    shl rax, 2          ; Multiply by 4 (bytes per pixel)
    
    ; Load current and next pixels
    movd xmm0, dword ptr [rsi + rax]
    movd xmm1, dword ptr [rsi + rax + 4]
    
    ; Extract RGB components for current pixel
    movdqa xmm8, xmm0
    pand xmm8, xmm6    ; Blue
    movdqa xmm9, xmm0
    pand xmm9, xmm5    ; Green
    psrld xmm9, 8
    movdqa xmm10, xmm0
    pand xmm10, xmm4   ; Red
    psrld xmm10, 16
    
    ; Extract RGB components for next pixel
    movdqa xmm11, xmm1
    pand xmm11, xmm6   ; Blue
    movdqa xmm12, xmm1
    pand xmm12, xmm5   ; Green
    psrld xmm12, 8
    movdqa xmm13, xmm1
    pand xmm13, xmm4   ; Red
    psrld xmm13, 16
    
    ; Calculate absolute differences
    psubd xmm8, xmm11  ; Blue diff
    pabsd xmm8, xmm8
    psubd xmm9, xmm12  ; Green diff
    pabsd xmm9, xmm9
    psubd xmm10, xmm13 ; Red diff
    pabsd xmm10, xmm10
    
    ; Sum up the differences
    paddd xmm8, xmm9
    paddd xmm8, xmm10
    
    ; Compare with threshold
    pcmpgtd xmm8, xmm3
    
    ; Create output pixel
    pxor xmm0, xmm0    ; Start with black
    movd xmm1, dword ptr [AlphaMask]
    por xmm0, xmm1     ; Add alpha channel
    
    ; If edge detected (xmm8 > 0), set to white
    pand xmm8, xmmword ptr [WhitePixels]
    por xmm0, xmm8
    
    ; Store result
    movd dword ptr [rdi + rax], xmm0
    
    inc rcx
    jmp process_loop
    
done:
    pop rbx
    pop rdi
    pop rsi
    mov rsp, rbp
    pop rbp
    ret
DetectEdges endp
end