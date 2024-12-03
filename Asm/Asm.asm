.data
BlueMask    dd 4 dup(0FFh)      ; Mask for blue channel
GreenMask   dd 4 dup(0FF00h)    ; Mask for green channel
RedMask     dd 4 dup(0FF0000h)  ; Mask for red channel
AlphaMask   dd 4 dup(0FF000000h); Mask for alpha channel
Threshold   dd 4 dup(15)        ; Edge detection threshold
BlackPixels dd 4 dup(0)         ; Black pixels (0,0,0,255)
WhitePixels dd 4 dup(0FFFFFFFFh); White pixels (255,255,255,255)

.code
DetectEdges proc
    ; Function prologue
    push rbp
    mov rbp, rsp
    push rsi
    push rdi
    push rbx
    push r12
    push r13
    push r14
    
    ; Parameters:
    ; rcx = source pointer
    ; rdx = destination pointer
    ; r8 = image width in pixels
    
    mov rsi, rcx        ; Source pointer
    mov rdi, rdx        ; Destination pointer
    mov r12, r8         ; Width
    
    ; Initialize SIMD registers
    pxor xmm7, xmm7              
    movdqa xmm6, xmmword ptr [BlueMask]   
    movdqa xmm5, xmmword ptr [GreenMask] 
    movdqa xmm4, xmmword ptr [RedMask]  
    movd xmm3, dword ptr [Threshold]     
    pshufd xmm3, xmm3, 0
    
    ; Calculate row length (aligned to 4 bytes)
    mov rax, r12        ; Width
    shl rax, 2          ; Multiply by 4 (bytes per pixel)
    add rax, 3          ; Add 3 for rounding up
    and rax, 0FFFFFFFCh ; Align to 4 bytes
    mov r13, rax        ; Store aligned row length in r13
    
    xor rcx, rcx        ; Pixel counter
    
process_loop:
    cmp rcx, r8
    jge done
    
    ; Calculate current position
    mov rax, rcx        ; Current pixel
    mov r14, rax        ; Copy for division
    xor rdx, rdx        ; Clear for division
    div r12             ; rax = row, rdx = column
    
    ; Calculate offset
    mul r13             ; Multiply row by stride
    add rax, rdx        ; Add column
    shl rax, 2          ; Multiply by 4 (bytes per pixel)
    
    ; Load pixels
    movd xmm0, dword ptr [rsi + rax]
    ; Check if we're at the last pixel in row
    mov rbx, rdx
    inc rbx
    cmp rbx, r12
    jge edge_pixel
    movd xmm1, dword ptr [rsi + rax + 4]
    jmp process_pixels
    
edge_pixel:
    movd xmm1, dword ptr [rsi + rax]      ; Use same pixel for edge
    
process_pixels:
    ; Extract and process color components (same as before)
    movdqa xmm8, xmm0
    pand xmm8, xmm6    ; Blue
    movdqa xmm9, xmm0
    pand xmm9, xmm5    ; Green
    psrld xmm9, 8
    movdqa xmm10, xmm0
    pand xmm10, xmm4   ; Red
    psrld xmm10, 16
    
    ; Process next pixel
    movdqa xmm11, xmm1
    pand xmm11, xmm6   ; Blue
    movdqa xmm12, xmm1
    pand xmm12, xmm5   ; Green
    psrld xmm12, 8
    movdqa xmm13, xmm1
    pand xmm13, xmm4   ; Red
    psrld xmm13, 16
    
    ; Calculate differences
    psubd xmm8, xmm11  ; Blue diff
    pabsd xmm8, xmm8
    psubd xmm9, xmm12  ; Green diff
    pabsd xmm9, xmm9
    psubd xmm10, xmm13 ; Red diff
    pabsd xmm10, xmm10
    
    ; Sum differences
    paddd xmm8, xmm9
    paddd xmm8, xmm10
    
    ; Compare with threshold
    pcmpgtd xmm8, xmm3
    
    ; Create output pixel
    pxor xmm0, xmm0    ; Start with black
    movd xmm1, dword ptr [AlphaMask]
    por xmm0, xmm1     ; Add alpha channel
    
    ; Set to white if edge detected
    pand xmm8, xmmword ptr [WhitePixels]
    por xmm0, xmm8
    
    ; Store result
    mov rax, rcx        ; Current pixel
    mov r14, rax        ; Copy for division
    xor rdx, rdx        ; Clear for division
    div r12             ; rax = row, rdx = column
    
    ; Calculate destination offset
    mul r13             ; Multiply row by stride
    add rax, rdx        ; Add column
    shl rax, 2          ; Multiply by 4 (bytes per pixel)
    
    movd dword ptr [rdi + rax], xmm0

    ; Store to right neighbor if not at right edge
    mov rbx, rdx
    inc rbx
    cmp rbx, r12
    jge skip_right
    movd dword ptr [rdi + rax + 4], xmm0
    
skip_right:
    ; Store to bottom neighbor if not at bottom edge
    mov rax, rcx
    add rax, r12
    cmp rax, r8
    jge next_pixel
    
    ; Calculate offset for bottom pixel
    mov rax, rcx
    mov r14, rax
    xor rdx, rdx
    div r12
    add rax, 1          ; Next row
    mul r13
    add rax, rdx
    shl rax, 2
    movd dword ptr [rdi + rax], xmm0
    
next_pixel:
    inc rcx
    jmp process_loop
    
done:
    pop r14
    pop r13
    pop r12
    pop rbx
    pop rdi
    pop rsi
    mov rsp, rbp
    pop rbp
    ret
DetectEdges endp
end