n  = int(input('tamanho: '))
fibo = [0, 1]

for i in range(n-2):
  last = fibo[len(fibo)-1]
  penultimate = fibo[len(fibo)-2]
  fibo.append(last+penultimate)
  
print(fibo)