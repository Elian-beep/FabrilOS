n  = int(input('tamanho: '))
primes = []
prime = 2

while len(primes) < n:
  is_prime = True
  for i in range(2, prime):
    if prime % i == 0:
        is_prime = False
        break
    
  if is_prime:
    primes.append(prime)
  prime += 1

print(primes)

mult = 1
for i in primes:
  mult *= i

print(mult)